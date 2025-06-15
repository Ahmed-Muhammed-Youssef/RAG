using RagLib.Core.Interfaces;
using System.Net.Http.Json;
using System.Text.Json.Serialization;

namespace RagLib.Embedding
{
    /// <inheritdoc/>
    internal class GeminiEmbedder : IEmbedder
    {
        private readonly string _apiKey;
        private readonly HttpClient _httpClient;

        public GeminiEmbedder(string apiKey)
        {
            _apiKey = apiKey ?? throw new ArgumentNullException(nameof(apiKey));
            _httpClient = new HttpClient
            {
                BaseAddress = new Uri("https://generativelanguage.googleapis.com/v1beta/models/embedding-001:")
            };
        }
        /// <inheritdoc/>
        public async Task<List<float>> EmbedAsync(string text)
        {
            if (string.IsNullOrWhiteSpace(text))
                throw new ArgumentException("Text cannot be null or whitespace.", nameof(text));

            var request = new { content = text };
            var response = await _httpClient.PostAsJsonAsync($"embedText?key={_apiKey}", request);

            response.EnsureSuccessStatusCode();

            var result = await response.Content.ReadFromJsonAsync<EmbedResponse>();
            return result?.Embedding?.Value ?? throw new InvalidOperationException("Embedding not returned.");

        }
        private class EmbedResponse
        {
            [JsonPropertyName("embedding")]
            public EmbeddingWrapper? Embedding { get; set; }
        }

        private class EmbeddingWrapper
        {
            [JsonPropertyName("value")]
            public List<float>? Value { get; set; }
        }
    }
}
