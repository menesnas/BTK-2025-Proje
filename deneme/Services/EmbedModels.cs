using System.Text.Json.Serialization;

namespace deneme.Services
{
    public class EmbedModels
    {
        public class EmbedRequest
        {
            [JsonPropertyName("model")]
            public string Model { get; set; } = string.Empty;

            // _singular_ content, not contents
            [JsonPropertyName("content")]
            public ContentChunk Content { get; set; } = new();
        }

        public class ContentChunk
        {
            [JsonPropertyName("parts")]
            public List<Part> Parts { get; set; } = new();
        }

        public class Part
        {
            [JsonPropertyName("text")]
            public string Text { get; set; } = string.Empty;
        }


        public class EmbedResponse
        {
            // API returns a single embedding object
            [JsonPropertyName("embedding")]
            public Embedding Embedding { get; set; } = null!;
        }
        public class Embedding
        {
            [JsonPropertyName("values")]
            public double[] Values { get; set; } = Array.Empty<double>();
        }
    }
}
