using Microsoft.Extensions.Configuration;
using System;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using static deneme.Services.EmbedModels;

namespace deneme.Services    
{
    public class EmbeddingService
    {
        private readonly HttpClient _http;
        private readonly string _apiKey;
        private readonly string _modelName;
        private readonly string _endpointUrl;

        public EmbeddingService(HttpClient http, IConfiguration config)
        {
            _http = http;
            _apiKey = config["GEMINI_API_KEY"]
                         ?? throw new ArgumentNullException("GEMINI_API_KEY missing");
            _modelName = config["EmbeddingModel"]
                         ?? "gemini-embedding-001";  // without 'models/' prefix
            _endpointUrl =
                $"https://generativelanguage.googleapis.com/v1beta/models/{_modelName}:embedContent";
        }

        public async Task<double[]> GetEmbeddingAsync(string text)
        {
            var req = new EmbedRequest
            {
                // API requires 'models/' prefix in the JSON model field
                Model = $"models/{_modelName}",
                Content = new ContentChunk
                {
                    Parts = { new Part { Text = text } }
                }
            };

            var json = JsonSerializer.Serialize(req);
            using var httpReq = new HttpRequestMessage(HttpMethod.Post, _endpointUrl);
            httpReq.Headers.Add("x-goog-api-key", _apiKey);
            httpReq.Content = new StringContent(json, Encoding.UTF8, "application/json");

            using var res = await _http.SendAsync(httpReq);
            res.EnsureSuccessStatusCode();

            await using var stream = await res.Content.ReadAsStreamAsync();
            var embedRes = await JsonSerializer.DeserializeAsync<EmbedResponse>(stream)
                           ?? throw new Exception("Empty embedding response");

            // Return the array under 'embedding.values'
            return embedRes.Embedding.Values;
        }
    }
}
