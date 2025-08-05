using deneme.Data;
using deneme.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Pinecone;
using System.Text.RegularExpressions;

namespace deneme.Controllers
{

    [ApiController]
    [Route("api/search")]
    public class SearchController : ControllerBase
    {
        private readonly EmbeddingService _embedSvc;
        private readonly PineconeClient _pinecone;
        private readonly string _indexName;
        private readonly ApplicationDbContext _db;
        private readonly IMemoryCache _cache;
        public SearchController(EmbeddingService embedSvc,
                                PineconeClient pinecone,
                                IConfiguration config,
                                ApplicationDbContext db,
                                IMemoryCache cache)
        {
            _embedSvc = embedSvc;
            _pinecone = pinecone;
            _indexName = config["Pinecone:IndexName"]
                         ?? throw new ArgumentNullException("Pinecone:IndexName is not configured");
            _db = db;
            _cache = cache;
        }
        [HttpGet]
        public async Task<IActionResult> Get(string query, int topK = 10)
        {
            // 7ci kısımda kullanacağım category'leri cacheye attım
            var allCategories = await _cache.GetOrCreateAsync<List<string>>("allCategories", async entry =>
            {
                // expire after 10 minutes
                entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(10);

                // actually load from the database
                return await _db.Products
                                .Select(p => p.Category)
                                .Distinct()
                                .ToListAsync();
            });


            // 0) Normalize
            var q = query.Trim();

            // 1) Price extraction 
            decimal? targetPrice = null;
            var priceMatch = System.Text.RegularExpressions.Regex.Match(q, @"(\d+(?:\.\d+)?)");
            if (priceMatch.Success)
            {
                if (decimal.TryParse(priceMatch.Groups[1].Value, out var p))
                    targetPrice = p;
                // remove it from the query so embedding focuses on semantics
                q = q.Replace(priceMatch.Value, "");
            }

            // 2) Category extraction
            string? categoryFilter = null;
            foreach (var cat in allCategories)
            {
                if (q.Contains(cat, StringComparison.OrdinalIgnoreCase))
                {
                    categoryFilter = cat;
                    // strip it out for better embedding
                    q = System.Text.RegularExpressions.Regex
                          .Replace(q, cat, "", RegexOptions.IgnoreCase);
                    break;
                }
            }

            // 3) Now trim the cleaned query
            q = q.Trim();
            if (string.IsNullOrEmpty(q))
                q = query;  // fallback to original if we stripped everything

            // 4) Embed the cleaned query
            var embD = await _embedSvc.GetEmbeddingAsync(q);
            var embF = embD.Select(d => (float)d).ToArray();

            // 5) Build Pinecone filter from what we extracted
            var filter = new Metadata();
            if (categoryFilter != null)
                filter["category"] = new Metadata { ["$eq"] = categoryFilter };
            if (targetPrice.HasValue)
            {
                // e.g. ±20% around the target price
                var min = targetPrice.Value * 0.8m;
                var max = targetPrice.Value * 1.2m;
                filter["price"] = new Metadata
                {
                    ["$gte"] = (double)min,
                    ["$lte"] = (double)max
                };
            }

            // 6) Query Pinecone with both semantic vector + metadata filter
            var index = _pinecone.Index(_indexName);
            var req = new QueryRequest
            {
                Vector = embF,
                TopK = (uint)topK,
                IncludeValues = false,
                IncludeMetadata = false,
                Filter = filter
            };
            var result = await index.QueryAsync(req);

            // 7a) Extract IDs pinecone returned ordered by score
            var ids = result.Matches.Select(m => int.Parse(m.Id)).ToList();// ids the list of product IDs in exact Pinecone relevance order.

            // 7b) Fetch matching products from SQL
            var products = await _db.Products
                                    .Where(p => ids.Contains(p.Id))
                                    .ToListAsync();

            // 7c) using dictionary for O(1) access
            var productById = products.ToDictionary(p => p.Id);

            // 7d) Reorder and skip any missing
            var ordered = ids
                .Where(id => productById.ContainsKey(id))
                .Select(id => productById[id])
                .ToList();
            // Log final ordered products to console
            Console.WriteLine($"[Search] Final ordered products: {string.Join(", ", ordered.Select(p => p.Id))}");
            return Ok(ordered);

        }

    }
}
