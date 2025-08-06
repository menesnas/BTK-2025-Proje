using System.Text;
using System.Text.Json;
using deneme.Models;
using deneme.Services;
using Microsoft.AspNetCore.Http;

namespace deneme.Services
{
    public class GeminiAIService : IAIService
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;
        private readonly string _apiKey;

        public GeminiAIService(IHttpClientFactory httpClientFactory, IConfiguration configuration)
        {
            _httpClient = httpClientFactory.CreateClient();
            _configuration = configuration;
            _apiKey = _configuration["Gemini:ApiKey"] ?? "";
            Console.WriteLine("Gemini API Key: " + (_apiKey.Length > 0 ? "OK" : "YOK!"));
        }

        public async Task<OutfitSuggestion> GetOutfitSuggestionAsync(string imageDescription, OutfitPreferences preferences, List<Product> productList, string giyimTipi = "", string gender = "")
        {
            if (string.IsNullOrEmpty(_apiKey))
            {
                throw new Exception("Gemini API key ayarlanmamış. Lütfen appsettings.json dosyasında Gemini:ApiKey değerini güncelleyin.");
            }

            var prompt = BuildPrompt(imageDescription, preferences, productList, giyimTipi, gender);
            var response = await CallGeminiAPI(prompt);
            return ParseResponse(response);
        }

        // Eski interface imzası için overload
        public async Task<OutfitSuggestion> GetOutfitSuggestionAsync(string imageDescription, OutfitPreferences preferences, List<Product> productList)
        {
            return await GetOutfitSuggestionAsync(imageDescription, preferences, productList, "");
        }

        private string BuildPrompt(string imageDescription, OutfitPreferences preferences, List<Product> productList, string giyimTipi = "", string gender = "")
        {
            var promptBuilder = new StringBuilder();

            promptBuilder.AppendLine("Sen bir moda uzmanısın. Aşağıdaki bilgilere göre kombin önerisi ver:");
            promptBuilder.AppendLine($"Kıyafet: {imageDescription}");
            promptBuilder.AppendLine($"Durum: {preferences.Occasion}");
            promptBuilder.AppendLine($"Bütçe: {preferences.BudgetRange}");
            
            if (!string.IsNullOrEmpty(gender))
            {
                promptBuilder.AppendLine($"Hedef cinsiyet: {gender}");
            }

            if (!string.IsNullOrEmpty(preferences.PreferredColors))
            {
                promptBuilder.AppendLine($"Tercih edilen renkler: {preferences.PreferredColors}");
            }

            promptBuilder.AppendLine("\nÖnemli: Önerdiğin ürünler aşağıdaki kategorilerden olmalıdır ve hedef cinsiyet ile uyumlu olmalıdır:");
            
            if (!string.IsNullOrEmpty(gender))
            {
                if (gender == "kadin")
                {
                    promptBuilder.AppendLine("- Sadece KADIN kıyafetlerini öner");
                }
                else if (gender == "erkek")
                {
                    promptBuilder.AppendLine("- Sadece ERKEK kıyafetlerini öner");
                }
                else
                {
                    promptBuilder.AppendLine("- Cinsiyet uyumlu kıyafetleri öner");
                }
            }

            promptBuilder.AppendLine("\nAşağıdaki ürünlerden uygun olanları öner:");
            foreach (var product in productList)
            {
                promptBuilder.AppendLine($"- {product.Name}");
            }


            promptBuilder.AppendLine("\nLütfen aşağıdaki JSON formatında yanıt ver ve önerilerini sadece yukarıdaki ürünlerden seç:");
            
            string exampleGender = gender == "erkek" ? "Erkek" : "Kadın";
            string exampleShoe = gender == "erkek" ? "Erkek Spor Ayakkabı" : "Kadın Topuklu Ayakkabı";
            
            promptBuilder.AppendLine($@"{{
                ""Style"": ""Casual"",
                ""UstGiyim"": ""{exampleGender} Kazak"",
                ""AltGiyim"": ""{exampleGender} Jean Pantolon"",
                ""Ayakkabi"": ""{exampleShoe}"",
                ""Aksesuar"": ""{exampleGender} Kolye"",
                ""ColorScheme"": ""Monokrom"",
                ""Occasion"": ""Günlük"",
                ""Description"": ""Modern ve şık günlük kombin""
            }}");

            return promptBuilder.ToString();
        }

        private async Task<string> CallGeminiAPI(string prompt)
        {
            if (string.IsNullOrEmpty(_apiKey))
            {
                throw new Exception("Gemini API key bulunamadı");
            }

            // Güncel Gemini modelleri:
            // gemini-1.5-flash (hızlı, önerilen)
            // gemini-1.5-pro (güçlü ama yavaş)
            // gemini-1.0-pro (eski versiyon)
            var url = $"https://generativelanguage.googleapis.com/v1beta/models/gemini-2.5-flash:generateContent?key={_apiKey}";
            
            var requestBody = new
            {
                contents = new[]
                {
                    new
                    {
                        parts = new[]
                        {
                            new { text = prompt }
                        }
                    }
                }
            };

            var json = JsonSerializer.Serialize(requestBody);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync(url, content);
            
            if (!response.IsSuccessStatusCode)
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                throw new Exception($"Gemini API hatası: {response.StatusCode} - {errorContent}");
            }

            return await response.Content.ReadAsStringAsync();
        }

        public async Task<(string description, string clothingType, string gender)> AnalyzeClothingImageAsync(IFormFile imageFile)
        {
            if (string.IsNullOrEmpty(_apiKey))
            {
                throw new Exception("Gemini API key ayarlanmamış.");
            }

            if (imageFile == null || imageFile.Length == 0)
            {
                return ("kıyafet", "bilinmiyor", "unisex");
            }

            try
            {
                // Görsel dosyayı base64'e çevir
                using var memoryStream = new MemoryStream();
                await imageFile.CopyToAsync(memoryStream);
                var imageBytes = memoryStream.ToArray();
                var base64Image = Convert.ToBase64String(imageBytes);

                // Gemini Vision API'yi çağır
                var prompt = @"Bu görseli analiz et ve şu bilgileri ver:
                1. Kıyafet açıklaması (renk, stil, tür)
                2. Kıyafet kategorisi: 'ust' (üst giyim: gömlek, tişört, kazak, ceket vb.), 'alt' (alt giyim: pantolon, etek, şort vb.), 'ayakkabi' (ayakkabı, bot vb.), 'aksesuar' (çanta, kemer, şapka vb.)
                3. Cinsiyet: 'kadin' (kadın kıyafeti), 'erkek' (erkek kıyafeti), 'unisex' (hem kadın hem erkek için uygun)

                Lütfen aşağıdaki JSON formatında yanıt ver:
                {
                    ""description"": ""mavi denim gömlek"",
                    ""clothingType"": ""ust"",
                    ""gender"": ""kadin""
                }";

                var url = $"https://generativelanguage.googleapis.com/v1beta/models/gemini-1.5-flash:generateContent?key={_apiKey}";
                
                var requestBody = new
                {
                    contents = new[]
                    {
                        new
                        {
                            parts = new object[]
                            {
                                new { text = prompt },
                                new 
                                { 
                                    inline_data = new 
                                    { 
                                        mime_type = imageFile.ContentType,
                                        data = base64Image 
                                    } 
                                }
                            }
                        }
                    }
                };

                var json = JsonSerializer.Serialize(requestBody);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await _httpClient.PostAsync(url, content);
                
                if (!response.IsSuccessStatusCode)
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    Console.WriteLine($"Gemini Vision API hatası: {response.StatusCode} - {errorContent}");
                    // Fallback: Dosya adından tahmin
                    return GetFallbackAnalysis(imageFile.FileName);
                }

                var responseContent = await response.Content.ReadAsStringAsync();
                return ParseImageAnalysisResponse(responseContent);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Görsel analiz hatası: {ex.Message}");
                // Fallback: Dosya adından tahmin
                return GetFallbackAnalysis(imageFile.FileName);
            }
        }

        private (string description, string clothingType, string gender) ParseImageAnalysisResponse(string response)
        {
            try
            {
                using var doc = JsonDocument.Parse(response);
                
                var candidates = doc.RootElement.GetProperty("candidates");
                if (candidates.GetArrayLength() > 0)
                {
                    var content = candidates[0].GetProperty("content");
                    var parts = content.GetProperty("parts");
                    if (parts.GetArrayLength() > 0)
                    {
                        var text = parts[0].GetProperty("text").GetString() ?? "";
                        
                        // JSON'ı parse et
                        var startIndex = text.IndexOf('{');
                        var endIndex = text.LastIndexOf('}');
                        
                        if (startIndex >= 0 && endIndex > startIndex)
                        {
                            var jsonPart = text.Substring(startIndex, endIndex - startIndex + 1);
                            using var analysisDoc = JsonDocument.Parse(jsonPart);
                            
                            var description = analysisDoc.RootElement.GetProperty("description").GetString() ?? "kıyafet";
                            var clothingType = analysisDoc.RootElement.GetProperty("clothingType").GetString() ?? "bilinmiyor";
                            var gender = analysisDoc.RootElement.GetProperty("gender").GetString() ?? "unisex";
                            
                            return (description, clothingType, gender);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Görsel analiz yanıtı parse edilemedi: {ex.Message}");
            }

            return ("kıyafet", "bilinmiyor", "unisex");
        }

        private (string description, string clothingType, string gender) GetFallbackAnalysis(string? fileName)
        {
            var name = fileName?.ToLower() ?? "";
            
            if (name.Contains("shirt") || name.Contains("gomlek") || name.Contains("tisort") || name.Contains("kazak") || name.Contains("ceket"))
            {
                return ("üst giyim", "ust", "unisex");
            }
            else if (name.Contains("jean") || name.Contains("pantolon") || name.Contains("etek") || name.Contains("sort"))
            {
                return ("alt giyim", "alt", "unisex");
            }
            else if (name.Contains("ayakkabi") || name.Contains("bot") || name.Contains("sandalet"))
            {
                return ("ayakkabı", "ayakkabi", "unisex");
            }
            else if (name.Contains("canta") || name.Contains("kemer") || name.Contains("sapka"))
            {
                return ("aksesuar", "aksesuar", "unisex");
            }
            
            return ("kıyafet", "bilinmiyor", "unisex");
        }

        private OutfitSuggestion ParseResponse(string response)
        {
            try
            {
                using var doc = JsonDocument.Parse(response);
                
                var candidates = doc.RootElement.GetProperty("candidates");
                if (candidates.GetArrayLength() > 0)
                {
                    var content = candidates[0].GetProperty("content");
                    var parts = content.GetProperty("parts");
                    if (parts.GetArrayLength() > 0)
                    {
                        var text = parts[0].GetProperty("text").GetString() ?? "";
                        
                        // JSON'ı parse et
                        var startIndex = text.IndexOf('{');
                        var endIndex = text.LastIndexOf('}');
                        
                        if (startIndex >= 0 && endIndex > startIndex)
                        {
                            var jsonPart = text.Substring(startIndex, endIndex - startIndex + 1);
                            var suggestion = JsonSerializer.Deserialize<OutfitSuggestion>(jsonPart, new JsonSerializerOptions
                            {
                                PropertyNameCaseInsensitive = true
                            });
                            
                            if (suggestion != null)
                            {
                                return suggestion;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Gemini yanıtı parse edilemedi: {ex.Message}");
            }

            throw new Exception("Gemini'den geçerli bir yanıt alınamadı.");
        }


    }
} 