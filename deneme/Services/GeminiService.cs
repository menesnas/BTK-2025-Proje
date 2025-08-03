using System.Net.Http;
using System.Text;
using System.Text.Json;

namespace deneme.Services
{
    public class GeminiService
    {
        private const string BaseUrl =
            "https://generativelanguage.googleapis.com/v1beta/models";

        private readonly string _apiKey;
        private readonly HttpClient _http;

        public GeminiService(string apiKey)
        {
            _apiKey = apiKey;
            _http = new HttpClient { Timeout = TimeSpan.FromSeconds(15) };
        }

        /// <summary>
        /// prompt  → Gemini-1.5 Flash text response
        /// </summary>
        public async Task<string> AskAsync(string prompt)
        {
            // 📌 Model adını burada değiştiriyoruz
            var url = $"{BaseUrl}/gemini-1.5-flash:generateContent?key={_apiKey}";

            var body = JsonSerializer.Serialize(new
            {
                contents = new[] {
                    new {
                        parts = new[] { new { text = prompt } }
                    }
                }
            });

            var resp = await _http.PostAsync(
                url,
                new StringContent(body, Encoding.UTF8, "application/json"));

            var raw = await resp.Content.ReadAsStringAsync();

            if (!resp.IsSuccessStatusCode)
                return $"API Hatası {(int)resp.StatusCode}: {raw}";

            using var doc = JsonDocument.Parse(raw);
            return doc.RootElement
                      .GetProperty("candidates")[0]
                      .GetProperty("content")
                      .GetProperty("parts")[0]
                      .GetProperty("text")
                      .GetString() ?? "(boş)";
        }
    }
}