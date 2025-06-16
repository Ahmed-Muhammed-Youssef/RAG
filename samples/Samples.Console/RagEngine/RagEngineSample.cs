using RagLib.Core.Builders;
using RagLib.Core.Models;

namespace Samples.Console.RagEngine;

internal class RagEngineSample
{
    public static async Task RunSample()
    {
        // Create a RagEngine instance with a fixed-size chunker, Gemini embedder, and Qdrant vector store
        var ragEngine = RagEngineBuilder.Create()
            .UseFixedSizeChunker(512)
            .UseGeminiEmbedder("your-api-key")
            .UseQdrantStore("localhost")
            .Build();

        // Sample document to ingest
        string document = "RAG stands for Retrieval-Augmented Generation, a technique that combines retrieval of relevant documents with generative models.";
        
        // Ingest the document
        await ragEngine.IngestAsync("sample-collection", document);

        // search for relevant chunks
        string query = "What is RAG?";
        List<DocumentChunk> relevantChunks = await ragEngine.GetRelevantChunksAsync("sample-collection", query, new RagQueryOptions { TopK = 3 });

        // Output the relevant chunks
        System.Console.WriteLine($"Relevant chunks for query '{query}':");
        foreach (var chunk in relevantChunks)
        {
            System.Console.WriteLine($"- {chunk.Content} (Index: {chunk.Index}, Metadata: {chunk.Metadata?.Custom})");
        }
    }
}
