using Microsoft.AspNetCore.Mvc;
using deneme.Services;
using deneme.Data;
using deneme.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Globalization;

namespace deneme.Controllers
{
    
    [ApiController]
    [Route("api/[controller]")]
    public class OutfitSuggestionController : ControllerBase
    {
        private readonly IAIService _aiService;
        private readonly ApplicationDbContext _context;

        public OutfitSuggestionController(IAIService aiService, ApplicationDbContext context)
        {
            _aiService = aiService;
            _context = context;
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

            // Gelen fotoğrafı analiz et
            var imageDescription = await AnalyzeImage(imageFile);
            Console.WriteLine($"Image description: {imageDescription}");

            // Giyim tipi tespiti (basit)
            string giyimTipi = "";
            var fileName = imageFile.FileName?.ToLower() ?? "";
            if (fileName.Contains("shirt") || fileName.Contains("gomlek") || fileName.Contains("tisort") || fileName.Contains("kazak") || fileName.Contains("ceket") || fileName.Contains("ust"))
            {
                giyimTipi = "ust";
            }
            else if (fileName.Contains("jean") || fileName.Contains("pantolon") || fileName.Contains("alt"))
            {
                giyimTipi = "alt";
            }

            var preferences = new OutfitPreferences
            {
                Occasion = occasion,
                BudgetRange = budgetRange,
                PreferredColors = preferredColors
            };

            // Veritabanından ürün listesini al
            var productList = await _context.Products.Take(30).ToListAsync();

            Console.WriteLine("AI servisini çağırıyor...");
            var suggestion = await _aiService.GetOutfitSuggestionAsync(imageDescription, preferences, productList);
            Console.WriteLine($"AI yanıtı alındı: {suggestion.Style}");

            // Yüklenen giyim tipine göre öneri alanını sıfırla
            if (giyimTipi == "alt") {
                suggestion.AltGiyim = "";
            }
            if (giyimTipi == "ust") {
                suggestion.UstGiyim = "";
            }
            if (giyimTipi == "ayakkabi") {
                suggestion.Ayakkabi = "";
            }
            if (giyimTipi == "aksesuar") {
                suggestion.Aksesuar = "";
            }

            // Veritabanından eşleşen ürünleri bul
            var matchingProducts = await FindMatchingProducts(suggestion);

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
                products = matchingProducts
            };
            
            Console.WriteLine($"Frontend'e gönderilen yanıt: {System.Text.Json.JsonSerializer.Serialize(response)}");
            return Ok(response);
        }

        private Task<string> AnalyzeImage(IFormFile? image)
        {
            
            if (image == null || image.Length == 0)
            {
                return Task.FromResult("kıyafet");
            }

            var fileName = image.FileName?.ToLower() ?? "";
            var fileSize = image.Length;

            // Dosya adı ve boyutuna göre basit tahminler
            if (fileName.Contains("shirt") || fileName.Contains("gomlek") || fileName.Contains("tisort"))
            {
                return Task.FromResult("gömlek veya tişört");
            }
            else if (fileName.Contains("jean") || fileName.Contains("pantolon"))
            {
                return Task.FromResult("pantolon");
            }
            else if (fileName.Contains("dress") || fileName.Contains("elbise"))
            {
                return Task.FromResult("elbise");
            }
            else if (fileName.Contains("jacket") || fileName.Contains("ceket"))
            {
                return Task.FromResult("ceket");
            }

            // Dosya boyutuna göre tahmin (çok basit)
            if (fileSize > 500000) // 500KB'dan büyükse
            {
                return Task.FromResult("üst giyim (detaylı)");
            }
            else
            {
                return Task.FromResult("kıyafet parçası");
            }
        }
        
        private async Task<dynamic> FindMatchingProducts(OutfitSuggestion suggestion)
        {
            // Yardımcı: Türkçe karakterleri normalize et
            string Normalize(string input)
            {
                if (string.IsNullOrEmpty(input)) return "";
                var normalized = input.ToLower(new CultureInfo("tr-TR", false));
                normalized = normalized.Replace('ç', 'c').Replace('ğ', 'g').Replace('ı', 'i').Replace('ö', 'o').Replace('ş', 's').Replace('ü', 'u');
                normalized = normalized.Replace('â', 'a').Replace('î', 'i').Replace('û', 'u');
                return normalized;
            }
            // Yardımcı: Levenshtein mesafesi
            int Levenshtein(string s, string t)
            {
                if (string.IsNullOrEmpty(s)) return t?.Length ?? 0;
                if (string.IsNullOrEmpty(t)) return s.Length;
                int[,] d = new int[s.Length + 1, t.Length + 1];
                for (int i = 0; i <= s.Length; i++) d[i, 0] = i;
                for (int j = 0; j <= t.Length; j++) d[0, j] = j;
                for (int i = 1; i <= s.Length; i++)
                    for (int j = 1; j <= t.Length; j++)
                        d[i, j] = Math.Min(
                            Math.Min(d[i - 1, j] + 1, d[i, j - 1] + 1),
                            d[i - 1, j - 1] + (s[i - 1] == t[j - 1] ? 0 : 1));
                return d[s.Length, t.Length];
            }
            // AI'nın önerdiği ürün adlarını normalize et
            var altGiyim = Normalize(suggestion.AltGiyim);
            var ayakkabi = Normalize(suggestion.Ayakkabi);
            var aksesuar = Normalize(suggestion.Aksesuar);
            // Tüm ürünleri çek
            var allProducts = await _context.Products.ToListAsync();
            // En yakın alt giyim ürünü
            var altGiyimProduct = allProducts
                .Where(p => !string.IsNullOrEmpty(altGiyim))
                .OrderBy(p => Levenshtein(Normalize(p.Name), altGiyim))
                .FirstOrDefault();
            // En yakın ayakkabı ürünü
            var ayakkabiProduct = allProducts
                .Where(p => !string.IsNullOrEmpty(ayakkabi))
                .OrderBy(p => Levenshtein(Normalize(p.Name), ayakkabi))
                .FirstOrDefault();
            // En yakın aksesuar ürünü
            var aksesuarProduct = allProducts
                .Where(p => !string.IsNullOrEmpty(aksesuar))
                .OrderBy(p => Levenshtein(Normalize(p.Name), aksesuar))
                .FirstOrDefault();
            // Listeler
            var altGiyimProducts = altGiyimProduct != null ? new[] { new {
                altGiyimProduct.Id,
                altGiyimProduct.Name,
                altGiyimProduct.Price,
                altGiyimProduct.ImageUrl,
                altGiyimProduct.Colour,
                altGiyimProduct.Category,
                DetailUrl = $"/Product/Details/{altGiyimProduct.Id}"
            }} : new object[0];
            var ayakkabiProducts = ayakkabiProduct != null ? new[] { new {
                ayakkabiProduct.Id,
                ayakkabiProduct.Name,
                ayakkabiProduct.Price,
                ayakkabiProduct.ImageUrl,
                ayakkabiProduct.Colour,
                ayakkabiProduct.Category,
                DetailUrl = $"/Product/Details/{ayakkabiProduct.Id}"
            }} : new object[0];
            var aksesuarProducts = aksesuarProduct != null ? new[] { new {
                aksesuarProduct.Id,
                aksesuarProduct.Name,
                aksesuarProduct.Price,
                aksesuarProduct.ImageUrl,
                aksesuarProduct.Colour,
                aksesuarProduct.Category,
                DetailUrl = $"/Product/Details/{aksesuarProduct.Id}"
            }} : new object[0];
            return new
            {
                altGiyim = altGiyimProducts,
                ayakkabi = ayakkabiProducts,
                aksesuar = aksesuarProducts
            };
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