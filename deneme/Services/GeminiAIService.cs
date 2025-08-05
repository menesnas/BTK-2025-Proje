using System.Text;
using System.Text.Json;
using deneme.Models;
using deneme.Services;

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

        public async Task<OutfitSuggestion> GetOutfitSuggestionAsync(string imageDescription, OutfitPreferences preferences, List<Product> productList, string giyimTipi = "")
        {
            if (string.IsNullOrEmpty(_apiKey))
            {
                throw new Exception("Gemini API key ayarlanmamış. Lütfen appsettings.json dosyasında Gemini:ApiKey değerini güncelleyin.");
            }

            var prompt = BuildPrompt(imageDescription, preferences, productList, giyimTipi);
            var response = await CallGeminiAPI(prompt);
            return ParseResponse(response);
        }

        // Eski interface imzası için overload
        public async Task<OutfitSuggestion> GetOutfitSuggestionAsync(string imageDescription, OutfitPreferences preferences, List<Product> productList)
        {
            return await GetOutfitSuggestionAsync(imageDescription, preferences, productList, "");
        }

        private string BuildPrompt(string imageDescription, OutfitPreferences preferences, List<Product> productList, string giyimTipi = "")
        {
            var promptBuilder = new StringBuilder();

            promptBuilder.AppendLine("Sen bir moda uzmanısın. Aşağıdaki bilgilere göre kombin önerisi ver:");
            promptBuilder.AppendLine($"Kıyafet: {imageDescription}");
            promptBuilder.AppendLine($"Durum: {preferences.Occasion}");
            promptBuilder.AppendLine($"Bütçe: {preferences.BudgetRange}");

            if (!string.IsNullOrEmpty(preferences.PreferredColors))
            {
                promptBuilder.AppendLine($"Tercih edilen renkler: {preferences.PreferredColors}");
            }

            promptBuilder.AppendLine("\nÖnemli: Önerdiğin ürünler aşağıdaki kategorilerden olmalıdır:");


            promptBuilder.AppendLine("\nAşağıdaki ürünlerden uygun olanları öner:");
            foreach (var product in productList)
            {
                promptBuilder.AppendLine($"- {product.Name}");
            }


            promptBuilder.AppendLine("\nLütfen aşağıdaki JSON formatında yanıt ver ve önerilerini sadece yukarıdaki ürünlerden seç:");
            promptBuilder.AppendLine(@"{
                ""Style"": ""Casual"",
                ""UstGiyim"": ""Kadın Kazak"",
                ""AltGiyim"": ""Kadın Jean Pantolon"",
                ""Ayakkabi"": ""Erkek Spor Ayakkabı"",
                ""Aksesuar"": ""Kadın Kolye"",
                ""ColorScheme"": ""Monokrom"",
                ""Occasion"": ""Günlük"",
                ""Description"": ""Modern ve şık günlük kombin""
            }");

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