using RagLib.Core.Interfaces;
using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace RagLib.Embedders;

/// <inheritdoc/>
public class GeminiEmbedder : IEmbedder
{
    private readonly string _apiKey;
    private readonly HttpClient _httpClient;


    /// <summary>
    /// Initializes a new instance of the <see cref="GeminiEmbedder"/> class using the specified Gemini API key.
    /// </summary>
    /// <param name="apiKey">Your API key from Google AI Studio or Google Cloud Platform.</param>
    /// <param name="handler"></param>
    /// <exception cref="ArgumentNullException"></exception>
    public GeminiEmbedder(string apiKey, HttpMessageHandler? handler = null!)
    {
        _apiKey = apiKey ?? throw new ArgumentNullException(nameof(apiKey));
        _httpClient = new HttpClient(handler ?? new HttpClientHandler())
        {
            BaseAddress = new Uri("https://generativelanguage.googleapis.com")
        };
    }

    /// <summary>
    /// Generates an embedding vector for the provided input text using Gemini's <c>embedding-001</c> model.
    /// </summary>
    /// <param name="text">The input text to embed.</param>
    /// <returns>
    /// A task representing the asynchronous operation. The result contains a list of floating-point values representing the embedding vector.
    /// </returns>
    /// <exception cref="InvalidOperationException">
    /// Thrown when the Gemini API response does not contain any embeddings.
    /// </exception>
    public async Task<List<float>> EmbedAsync(string text)
    {
        if (string.IsNullOrWhiteSpace(text))
            throw new ArgumentException("Text cannot be null or whitespace.", nameof(text));

        EmbeddingRequest request = new()
        {
            Content = new EmbeddingParts
            { 
                Parts =
                [
                    new EmbeddingTextWrapper { Text = text }
                ]
            }
        };

        HttpResponseMessage response = await _httpClient.PostAsJsonAsync($"/v1beta/models/text-embedding-004:embedContent?key={_apiKey}", request);

        response.EnsureSuccessStatusCode();

        var result = await response.Content.ReadFromJsonAsync<EmbedResponse>();
        return result?.Embedding?.Values ?? throw new InvalidOperationException("Embedding not returned.");

    }

    private class EmbeddingRequest
    {
        [JsonPropertyName("model")]
        public string Model { get; set; } = "models/text-embedding-004";

        [JsonPropertyName("content")]
        public EmbeddingParts Content { get; set; } = new();

        [JsonPropertyName("title")]
        public string Title { get; set; } = "";

        [JsonPropertyName("taskType")]
        public string TaskType { get; set; } = "RETRIEVAL_DOCUMENT";
    }

    private class EmbeddingParts
    {
        [JsonPropertyName("parts")]
        public List<EmbeddingTextWrapper> Parts { get; set; } = [];
    }

    private class EmbeddingTextWrapper
    {
        [JsonPropertyName("text")]
        public string Text { get; set; } = string.Empty;
    }
    public class EmbedResponse
    {
        [JsonPropertyName("embedding")]
        public ContentEmbedding? Embedding { get; set; }
    }

    public class ContentEmbedding
    {
        [JsonPropertyName("values")]
        public List<float> Values { get; set; } = [];
    }
}
