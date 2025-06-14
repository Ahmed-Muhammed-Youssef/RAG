using Qdrant.Client;
using Qdrant.Client.Grpc;
using RagLib.Core.Interfaces;
using RagLib.Core.Models;

namespace RagLib.VectorStores;

/// <summary>
/// Implements IVectorStore using Qdrant vector database.
/// Requires a running Qdrant instance locally (default port 6334) or remotely.
/// </summary>
public class QdrantVectorStore : IVectorStore
{
    private readonly QdrantClient _client;

    /// <summary>
    /// Initializes the Qdrant client.
    /// </summary>
    /// <param name="host">Hostname or URL of Qdrant (include port if non-default).</param>
    public QdrantVectorStore(string host = "localhost")
    {
        // Basic constructor connects via REST + gRPC
        _client = new QdrantClient(host);
    }

    /// <inheritdoc/>
    public Task AddAsync(string collection, DocumentChunk chunk, List<float> vector)
    {
        throw new NotImplementedException();
    }

    /// <inheritdoc/>
    public Task AddRangeAsync(string collection, IEnumerable<(DocumentChunk chunk, List<float> vector)> items)
    {
        throw new NotImplementedException();
    }

    /// <inheritdoc/>
    public async Task CreateCollectionAsync(string name)
    {
        // Create a collection with default 1536 dimensions (e.g., OpenAI embeddings) and cosine flavor
        await _client.CreateCollectionAsync(
            name,
            new VectorParams { Size = 1536, Distance = Distance.Cosine }
        );
    }

    /// <inheritdoc/>
    public Task DeleteAsync(string collection, string id)
    {
        throw new NotImplementedException();
    }

    /// <inheritdoc/>
    public async Task DeleteCollectionAsync(string name)
    {
        await _client.DeleteCollectionAsync(name);
    }

    /// <inheritdoc/>
    public Task<DocumentChunk?> GetById(string collection, string id)
    {
        throw new NotImplementedException();
    }

    /// <inheritdoc/>
    public async Task<IEnumerable<string>> ListCollectionsAsync()
    {
        // Get all collections and return their names
        return await _client.ListCollectionsAsync();
    }

    /// <inheritdoc/>
    public Task<List<(DocumentChunk Chunk, float Score)>> SearchAsync(string collection, List<float> queryVector, int topK, IDictionary<string, string>? metadataFilters = null)
    {
        throw new NotImplementedException();
    }

    /// <inheritdoc/>
    public Task UpsertAsync(string collection, string id, DocumentChunk chunk, List<float> vector)
    {
        throw new NotImplementedException();
    }
}
