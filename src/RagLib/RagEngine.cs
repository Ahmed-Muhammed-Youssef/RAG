using RagLib.Core.Interfaces;
using RagLib.Core.Models;

namespace RagLib;

/// <inheritdoc/>
public class RagEngine : IRagEngine
{
    private readonly IDocumentChunker chunker;
    private readonly IEmbedder embedder;
    private readonly IVectorStore vectorStore;

    internal RagEngine(IDocumentChunker chunker, IEmbedder embedder, IVectorStore vectorStore)
    {
        this.chunker = chunker;
        this.embedder = embedder;
        this.vectorStore = vectorStore;
    }

    /// <inheritdoc/>
    public async Task<List<DocumentChunk>> GetRelevantChunksAsync(string collection, string query, RagQueryOptions? options = null)
    {
        var queryEmbedding = await embedder.EmbedAsync(query);
        var topK = options?.TopK ?? 3;

        List<(DocumentChunk Chunk, float Score)> retrievedChunks = await vectorStore.SearchAsync(collection, [.. queryEmbedding], topK);
        return [.. retrievedChunks.Select(c => c.Chunk)];
    }

    /// <inheritdoc/>
    public async Task IngestAsync(string collection, string document, DocumentMetadata? metadata = null)
    {
        // check if collection exists, if not create it
        if (!(await vectorStore.ListCollectionsAsync()).Contains(collection))
        {
            await vectorStore.CreateCollectionAsync(collection);
        }

        List<DocumentChunk> chunks = chunker.Chunk(document, metadata);

        foreach (var chunk in chunks)
        {
            var vector = await embedder.EmbedAsync(chunk.Content);
            await vectorStore.AddAsync(collection, chunk, [.. vector]);
        }
    }
}
