using Mscc.GenerativeAI;
using RagLib.Core.Interfaces;

namespace RagLib.Embedders;

public class GeminiEmbedder : IEmbedder
{
    private readonly GenerativeModel _model;

    /// <summary>
    /// Initializes a new instance of the <see cref="GeminiEmbedder"/> class using the specified Gemini API key.
    /// </summary>
    /// <param name="apiKey">Your API key from Google AI Studio or Google Cloud Platform.</param>
    public GeminiEmbedder(string apiKey)
    {
        GoogleAI ai = new(apiKey: apiKey);
        _model = ai.GenerativeModel(model: Model.Embedding001);
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
        EmbedContentResponse response = await _model.EmbedContent(new EmbedContentRequest
        {
            Content = new ContentResponse(text),
            TaskType = TaskType.RetrievalDocument
        });

        return ExtractEmbeddings(response);
    }

    /// <summary>
    /// Extracts and flattens the list of embedding vectors from the Gemini API response.
    /// </summary>
    /// <param name="response">The response returned from the Gemini embedding request.</param>
    /// <returns>A single flattened embedding vector.</returns>
    /// <exception cref="InvalidOperationException">Thrown if the response contains no embeddings.</exception>
    private static List<float> ExtractEmbeddings(EmbedContentResponse response)
    {
        if (response.Embeddings == null || response.Embeddings.Count == 0)
        {
            throw new InvalidOperationException("No embeddings found in the response.");
        }

        return response.Embeddings.SelectMany(e => e.Values).ToList();
    }
}
