using Microsoft.Extensions.Options;
using RagLib.Chunking;
using RagLib.Core.Models;
using RagLib.Embedders;
using RagLib.VectorStores;

namespace Samples.Console.RagEngine;

internal class RagEngineSample
{
    public static async Task RunSample()
    {
        // Create a fixed-size chunker with a chunk size of 512 characters
        ChunkerOptions chunkerOptions = new()
        {
            ChunkSize = 512
        };

        IOptions<ChunkerOptions> options = Options.Create(chunkerOptions);
        FixedSizeChunker chunker = new(options);

        // Initialize the embedder and vector store
        GeminiEmbedder embedder = new("Add your API key");
        QdrantVectorStore vectorStore = new("localhost");

        // Create the RAG engine
        RagLib.RagEngine ragEngine = new(chunker, embedder, vectorStore);

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
