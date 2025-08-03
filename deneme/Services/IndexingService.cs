using deneme.Data;
using Microsoft.EntityFrameworkCore;
using Pinecone;  // Pinecone .NET SDK

namespace deneme.Services    
{
    public class IndexingService
    {
        private readonly EmbeddingService _embedSvc;
        private readonly PineconeClient _pinecone;
        private readonly string _indexName;
        private readonly ApplicationDbContext _db;

        public IndexingService(EmbeddingService embedSvc,
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

        public async Task UpsertAllProductsAsync()
        {
            // 1) SQL'den tüm ürünleri çek
            var products = await _db.Products.ToListAsync();

            // 2) Vector listesi oluştur
            var vectors = new List<Vector>();
            foreach (var p in products)
            {
                // Embedding servisten gelen double[] -> float[]
                var embD = await _embedSvc.GetEmbeddingAsync(p.SearchableText);
                // *** LOG IT ***
                Console.WriteLine($"Product {p.Id}: embedding.Length = {embD.Length}");
                var embF = embD.Select(d => (float)d).ToArray();

                vectors.Add(new Vector
                {
                    Id = p.Id.ToString(),
                    Values = embF
                });
            }

            // 3) Pinecone'a upsert isteği gönder
            var index = _pinecone.Index(_indexName);
            var upsertRequest = new UpsertRequest
            {
                Vectors = vectors.ToArray()
            };
            try
            {
                await index.UpsertAsync(upsertRequest);
            }
            catch (PineconeApiException ex)
            {
                Console.WriteLine("❌ Pinecone upsert failed!");
                Console.WriteLine($"  gRPC status: {ex.StatusCode}");    // should be StatusCode.InvalidArgument
                Console.WriteLine($"  Message    : {ex.Message}");       // usually contains something like “vector dimension x does not match index dimension y”
                throw;
            }
            
        }
    }
}
