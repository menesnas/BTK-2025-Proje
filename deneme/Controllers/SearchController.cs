using deneme.Services;
using deneme.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Pinecone;

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

        public SearchController(EmbeddingService embedSvc,
                                PineconeClient pinecone,
                                IConfiguration config,
                                ApplicationDbContext db)
        {
            _embedSvc = embedSvc;
            _pinecone = pinecone;
            _indexName = config["Pinecone:IndexName"]
                         ?? throw new ArgumentNullException("Pinecone:IndexName is not configured");
            _db = db;
        }

        [HttpGet]
        public async Task<IActionResult> Get(string query, int topK = 10)
        {
            // 1) Sorguyu embed et
            var embD = await _embedSvc.GetEmbeddingAsync(query);
            var embF = embD.Select(d => (float)d).ToArray();

            // 2) Pinecone query isteği yap
            var index = _pinecone.Index(_indexName);
            var queryRequest = new QueryRequest
            {
                Vector = embF,
                TopK = (uint)topK,
                IncludeValues = false,
                IncludeMetadata = false
            };
            var queryRes = await index.QueryAsync(queryRequest);

            // 3) ID listesini al
            var orderedIds = queryRes.Matches.Select(m => int.Parse(m.Id)).ToList();

            // 4) SQL'den ürünleri çek
            var products = await _db.Products.Where(p => orderedIds.Contains(p.Id)).ToListAsync();

            // 5) Pinecone sırasına göre yeniden sırala
            var ordered = orderedIds.Select(id => products.Single(p => p.Id == id)).ToList();

            return Ok(ordered);
        }
    }
}
