using RagLib.Embedders;

namespace Samples.Console.Embedders;

/// <summary>
/// Demonstrates how to use GeminiEmbedder to generate text embeddings.
/// </summary>
internal static class GeminiEmbedderSample
{
    /// <summary>
    /// Runs a Gemini embedding example using a sample query.
    /// </summary>
    public static async Task RunSample()
    {
        string apiKey = "Add your API key";
        var embedder = new GeminiEmbedder(apiKey);

        var embedding = await embedder.EmbedAsync("What is RAG?");
        System.Console.WriteLine(string.Join(", ", embedding));
    }
}
