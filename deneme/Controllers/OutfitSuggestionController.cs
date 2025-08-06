using Microsoft.AspNetCore.Mvc;
using deneme.Services;
using deneme.Data;
using deneme.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Globalization;
using Pinecone;


namespace deneme.Controllers
{
    
    [ApiController]
    [Route("api/[controller]")]
    public class OutfitSuggestionController : ControllerBase
    {
        private readonly IAIService _aiService;
        private readonly ApplicationDbContext _context;
        private readonly EmbeddingService _embedSvc; // Yeni ekleme
        private readonly PineconeClient _pinecone; // Yeni ekleme
        private readonly string _indexName; // Yeni ekleme

        public OutfitSuggestionController(
            IAIService aiService, 
            ApplicationDbContext context,
            EmbeddingService embedSvc, // Yeni parametre
            PineconeClient pinecone, // Yeni parametre
            IConfiguration config) // Yeni parametre
        {
            _aiService = aiService;
            _context = context;
            _embedSvc = embedSvc; // Yeni atama
            _pinecone = pinecone; // Yeni atama
            _indexName = config["Pinecone:IndexName"] 
                         ?? throw new ArgumentNullException("Pinecone:IndexName is not configured");
        }

        [HttpGet]
        [Route("test")]
        public IActionResult Test()
        {
            return Ok(new { message = "API çalışıyor!", timestamp = DateTime.Now });
        }

        [HttpPost]
        [Route("test-json")]
        public IActionResult TestJson([FromBody] OutfitRequestBase64 request)
        {
            try
            {
                Console.WriteLine("=== TEST JSON ===");
                Console.WriteLine($"Request null mu: {request == null}");
                if (request != null)
                {
                    Console.WriteLine($"Base64 length: {request.ClothingImageBase64?.Length}");
                    Console.WriteLine($"FileName: {request.FileName}");
                    Console.WriteLine($"Occasion: {request.Occasion}");
                }
                
                return Ok(new { 
                    message = "JSON deserialization başarılı!", 
                    receivedData = new {
                        hasBase64 = !string.IsNullOrEmpty(request?.ClothingImageBase64),
                        base64Length = request?.ClothingImageBase64?.Length,
                        fileName = request?.FileName,
                        occasion = request?.Occasion
                    }
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        [HttpPost]
        [Route("form")]
        public async Task<IActionResult> GetOutfitSuggestionForm([FromForm] OutfitRequest request)
        {
            try
            {
                Console.WriteLine("=== FORM API ÇAĞRISI BAŞLADI ===");
                Console.WriteLine($"Gelen dosya: {request.ClothingImage?.FileName}");
                Console.WriteLine($"Dosya boyutu: {request.ClothingImage?.Length}");
                
                return await ProcessOutfitSuggestion(request.ClothingImage, request.Occasion, request.BudgetRange, request.PreferredColors);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"=== FORM API HATASI ===");
                Console.WriteLine($"Hata: {ex.Message}");
                return BadRequest(new { success = false, message = ex.Message });
            }
        }

        [HttpPost]
        [Route("json")]
        public async Task<IActionResult> GetOutfitSuggestionJson([FromBody] OutfitRequestBase64 request)
        {
            try
            {
                Console.WriteLine("=== JSON API ÇAĞRISI BAŞLADI ===");
                
                // Null check
                if (request == null)
                {
                    Console.WriteLine("Request null!");
                    return BadRequest(new { success = false, message = "Request is null" });
                }
                
                Console.WriteLine($"Request object alındı");
                Console.WriteLine($"ClothingImageBase64 null mu: {request.ClothingImageBase64 == null}");
                Console.WriteLine($"ClothingImageBase64 boş mu: {string.IsNullOrEmpty(request.ClothingImageBase64)}");
                Console.WriteLine($"Gelen base64 uzunluğu: {request.ClothingImageBase64?.Length}");
                Console.WriteLine($"Dosya adı: {request.FileName}");
                Console.WriteLine($"Dosya tipi: {request.FileType}");
                Console.WriteLine($"Occasion: {request.Occasion}");
                Console.WriteLine($"Budget: {request.BudgetRange}");
                
                // Validation
                if (string.IsNullOrEmpty(request.ClothingImageBase64))
                {
                    Console.WriteLine("Base64 image boş!");
                    return BadRequest(new { success = false, message = "ClothingImageBase64 is required" });
                }
                
                if (string.IsNullOrEmpty(request.Occasion))
                {
                    Console.WriteLine("Occasion boş!");
                    return BadRequest(new { success = false, message = "Occasion is required" });
                }
                
                Console.WriteLine("Base64'ü FormFile'a çevriliyor...");
                // Base64'ü IFormFile'a çevir
                var imageFile = ConvertBase64ToFormFile(request.ClothingImageBase64, request.FileName, request.FileType);
                Console.WriteLine($"FormFile oluşturuldu, boyut: {imageFile.Length}");
                
                return await ProcessOutfitSuggestion(imageFile, request.Occasion, request.BudgetRange, request.PreferredColors);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"=== JSON API HATASI ===");
                Console.WriteLine($"Hata: {ex.Message}");
                Console.WriteLine($"Stack: {ex.StackTrace}");
                return BadRequest(new { success = false, message = ex.Message, stackTrace = ex.StackTrace });
            }
        }

        private async Task<IActionResult> ProcessOutfitSuggestion(IFormFile? imageFile, string occasion, string budgetRange, string preferredColors)
        {
            // Null check
            if (imageFile == null)
            {
                Console.WriteLine("ImageFile null!");
                return BadRequest(new { success = false, message = "Image file is required" });
            }

            Console.WriteLine("Gemini ile görsel analiz yapılıyor...");
            // Gemini ile görsel analiz yap - açıklama, kıyafet tipi ve cinsiyet bilgisi al
            var (imageDescription, clothingType, gender) = await _aiService.AnalyzeClothingImageAsync(imageFile);
            Console.WriteLine($"Image description: {imageDescription}");
            Console.WriteLine($"Clothing type: {clothingType}");
            Console.WriteLine($"Gender: {gender}");

            var preferences = new OutfitPreferences
            {
                Occasion = occasion,
                BudgetRange = budgetRange,
                PreferredColors = preferredColors
            };

            // Veritabanından ürün listesini al
            var productList = await _context.Products.Take(30).ToListAsync();

            Console.WriteLine("AI servisini çağırıyor...");
            // Cinsiyet bilgisini de AI servisine gönder
            if (_aiService is GeminiAIService geminiService)
            {
                var suggestion = await geminiService.GetOutfitSuggestionAsync(imageDescription, preferences, productList, clothingType, gender);
                Console.WriteLine($"AI yanıtı alındı: {suggestion.Style}");
                
                // Yüklenen kıyafet tipine göre öneri alanını sıfırla (çünkü kullanıcının zaten o kıyafeti var)
                switch (clothingType.ToLower())
                {
                    case "ust":
                        suggestion.UstGiyim = "";
                        break;
                    case "alt":
                        suggestion.AltGiyim = "";
                        break;
                    case "ayakkabi":
                        suggestion.Ayakkabi = "";
                        break;
                    case "aksesuar":
                        suggestion.Aksesuar = "";
                        break;
                }

                // Veritabanından eşleşen ürünleri bul
                var matchingProducts = await FindMatchingProductsAdvanced(suggestion);

                Console.WriteLine("=== API ÇAĞRISI BAŞARILI ===");
                var response = new
                {
                    success = true,
                    suggestion.Style,
                    suggestion.UstGiyim,
                    suggestion.AltGiyim,
                    suggestion.Ayakkabi,
                    suggestion.Aksesuar,
                    suggestion.ColorScheme,
                    suggestion.Occasion,
                    uploadedClothingType = clothingType, // Frontend'e hangi tip kıyafet yüklendiğini bildir
                    detectedGender = gender, // Frontend'e algılanan cinsiyeti bildir
                    products = matchingProducts
                };
                Console.WriteLine("\n\nGeminiresponse: " + response);
                Console.WriteLine($"Frontend'e gönderilen yanıt: {System.Text.Json.JsonSerializer.Serialize(response)}");
                return Ok(response);
            }
            else
            {
                // Fallback: Eski method
                var suggestion = await _aiService.GetOutfitSuggestionAsync(imageDescription, preferences, productList);
                Console.WriteLine($"AI yanıtı alındı: {suggestion.Style}");

                // Yüklenen kıyafet tipine göre öneri alanını sıfırla (çünkü kullanıcının zaten o kıyafeti var)
                switch (clothingType.ToLower())
                {
                    case "ust":
                        suggestion.UstGiyim = "";
                        break;
                    case "alt":
                        suggestion.AltGiyim = "";
                        break;
                    case "ayakkabi":
                        suggestion.Ayakkabi = "";
                        break;
                    case "aksesuar":
                        suggestion.Aksesuar = "";
                        break;
                }

                // Veritabanından eşleşen ürünleri bul
                var matchingProducts = await FindMatchingProductsAdvanced(suggestion);

                Console.WriteLine("=== API ÇAĞRISI BAŞARILI ===");
                var response = new
                {
                    success = true,
                    suggestion.Style,
                    suggestion.UstGiyim,
                    suggestion.AltGiyim,
                    suggestion.Ayakkabi,
                    suggestion.Aksesuar,
                    suggestion.ColorScheme,
                    suggestion.Occasion,
                    uploadedClothingType = clothingType, // Frontend'e hangi tip kıyafet yüklendiğini bildir
                    detectedGender = gender, // Frontend'e algılanan cinsiyeti bildir
                    products = matchingProducts
                };
                Console.WriteLine("\n\nGeminiresponse: " + response);
                Console.WriteLine($"Frontend'e gönderilen yanıt: {System.Text.Json.JsonSerializer.Serialize(response)}");
                return Ok(response);
            }
        }


        
        private async Task<dynamic> FindMatchingProductsAdvanced(OutfitSuggestion suggestion)
        {
            // Her kategori için ayrı arama stratejileri
            var tasks = new List<Task<(string category, List<Product> products)>>();

            // Üst giyim araması
            if (!string.IsNullOrEmpty(suggestion.UstGiyim))
            {
                tasks.Add(SearchProductsWithCategory("ustGiyim", suggestion.UstGiyim, 
                    new[] { "gömlek", "tisort", "ceket", "kazak"}));
            }

            // Alt giyim araması
            if (!string.IsNullOrEmpty(suggestion.AltGiyim))
            {
                tasks.Add(SearchProductsWithCategory("altGiyim", suggestion.AltGiyim, 
                    new[] { "pantolon", "jean", "etek", "şort" }));
            }

            // Ayakkabı araması  
            if (!string.IsNullOrEmpty(suggestion.Ayakkabi))
            {
                tasks.Add(SearchProductsWithCategory("ayakkabi", suggestion.Ayakkabi,
                    new[] { "ayakkabı", "bot", "sandalet", "terlik", "spor ayakkabı" }));
            }

            // Aksesuar araması
            if (!string.IsNullOrEmpty(suggestion.Aksesuar))
            {
                tasks.Add(SearchProductsWithCategory("aksesuar", suggestion.Aksesuar,
                    new[] { "çanta", "kemer", "şapka", "gözlük", "takı", "saat" }));
            }

            var results = await Task.WhenAll(tasks);
            
            var response = new Dictionary<string, object>();
            foreach (var (category, products) in results)
            {
                response[category] = products.Select(p => new {
                    p.Id,
                    p.Name,
                    p.Price,
                    p.ImageUrl,
                    p.Colour,
                    p.Category,
                    DetailUrl = $"/Product/Details/{p.Id}",
                    Similarity = CalculateSimilarity(category == "altGiyim" ? suggestion.AltGiyim : 
                                                   category == "ayakkabi" ? suggestion.Ayakkabi : 
                                                   category == "ustGiyim" ? suggestion.UstGiyim :
                                                   category == "aksesuar" ? suggestion.Aksesuar :
                                                   suggestion.Aksesuar, p.Name)
                })
                .Where(x => x.Similarity > 0.6) // Sadece 0.6'dan büyük benzerlik oranına sahip ürünleri filtrele
                .OrderByDescending(x => x.Similarity)
                .ToArray();
            }

            return response;
        }

        private async Task<(string category, List<Product> products)> SearchProductsWithCategory(
            string category, string query, string[] categoryKeywords)
        {
            // Önce embedding ile ara
            var embeddingResults = await SearchProductsByEmbedding(query, 5);
            
            // Kategori kelimelerini de ekleyerek genişletilmiş arama
            var expandedQuery = $"{query} {string.Join(" ", categoryKeywords)}";
            var expandedResults = await SearchProductsByEmbedding(expandedQuery, 3);
            
            // Sonuçları birleştir ve tekrarları kaldır
            var allResults = embeddingResults.Concat(expandedResults)
                .GroupBy(p => p.Id)
                .Select(g => g.First())
                .ToList();
            
            return (category, allResults);
        }

        private double CalculateSimilarity(string aiSuggestion, string productName)
        {
            // Basit benzerlik hesaplaması (Levenshtein veya daha gelişmiş yöntemler kullanılabilir)
            if (string.IsNullOrEmpty(aiSuggestion) || string.IsNullOrEmpty(productName))
                return 0;
                
            var suggestion = aiSuggestion.ToLower().Trim();
            var product = productName.ToLower().Trim();
            
            // Kelime bazlı eşleşme kontrolü
            var suggestionWords = suggestion.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            var productWords = product.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            
            var matchCount = suggestionWords.Count(sw => 
                productWords.Any(pw => pw.Contains(sw) || sw.Contains(pw)));
                
            return (double)matchCount / Math.Max(suggestionWords.Length, productWords.Length);
        }

        // Yeni method: Embedding tabanlı ürün arama
        private async Task<List<Product>> SearchProductsByEmbedding(string query, int topK = 5)
        {
            if (string.IsNullOrEmpty(query) || query.Trim() == "")
            {
                return new List<Product>();
            }

            try
            {
                Console.WriteLine($"Embedding arama yapılıyor: '{query}'");
                
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
                Console.WriteLine($"Bulunan ürün ID'leri: {string.Join(", ", orderedIds)}");

                // 4) SQL'den ürünleri çek
                var products = await _context.Products.Where(p => orderedIds.Contains(p.Id)).ToListAsync();

                // 5) Pinecone sırasına göre yeniden sırala
                var ordered = orderedIds
                    .Select(id => products.FirstOrDefault(p => p.Id == id))
                    .Where(p => p != null)
                    .Cast<Product>()
                    .ToList();

                Console.WriteLine($"'{query}' için {ordered.Count} ürün bulundu");
                return ordered;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Embedding arama hatası ({query}): {ex.Message}");
                // Fallback: Basit text arama
                return await _context.Products
                    .Where(p => p.Name.ToLower().Contains(query.ToLower()) || 
                               p.Category.ToLower().Contains(query.ToLower()))
                    .Take(topK)
                    .ToListAsync();
            }
        }

        private IFormFile ConvertBase64ToFormFile(string base64String, string fileName, string contentType)
        {
            var bytes = Convert.FromBase64String(base64String);
            var stream = new MemoryStream(bytes);
            
            return new CustomFormFile(stream, fileName, contentType);
        }
    }

    // Basit FormFile implementation
    public class CustomFormFile : IFormFile
    {
        private readonly Stream _stream;
        
        public CustomFormFile(Stream stream, string fileName, string contentType)
        {
            _stream = stream;
            FileName = fileName;
            ContentType = contentType;
            Length = stream.Length;
        }

        public string ContentType { get; }
        public string ContentDisposition => $"form-data; name=\"file\"; filename=\"{FileName}\"";
        public IHeaderDictionary Headers { get; } = new HeaderDictionary();
        public long Length { get; }
        public string Name => "file";
        public string FileName { get; }

        public void CopyTo(Stream target) => _stream.CopyTo(target);
        public Task CopyToAsync(Stream target, CancellationToken cancellationToken = default) => _stream.CopyToAsync(target, cancellationToken);
        public Stream OpenReadStream() => _stream;
    }

    public class OutfitRequest
    {
        public IFormFile ClothingImage { get; set; } = null!;
        public string Occasion { get; set; } = string.Empty;
        public string BudgetRange { get; set; } = string.Empty;
        public string PreferredColors { get; set; } = string.Empty;
    }

    public class OutfitRequestBase64
    {
        public string ClothingImageBase64 { get; set; } = string.Empty;
        public string FileName { get; set; } = string.Empty;
        public string FileType { get; set; } = string.Empty;
        public string Occasion { get; set; } = string.Empty;
        public string BudgetRange { get; set; } = string.Empty;
        public string PreferredColors { get; set; } = string.Empty;
    }
} 