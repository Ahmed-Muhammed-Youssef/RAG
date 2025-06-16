using RagLib.Embedders;

namespace Samples.Console;

public class Program
{
    static async Task Main(string[] args)
    {
        await GeminiEmbedderSamples();
    }

    private static async Task GeminiEmbedderSamples()
    {
        string apiKey = "Add your API key";
        var embedder = new GeminiEmbedder(apiKey);

        var embedding = await embedder.EmbedAsync("What is RAG?");
        System.Console.WriteLine(string.Join(", ", embedding));
    }
}
