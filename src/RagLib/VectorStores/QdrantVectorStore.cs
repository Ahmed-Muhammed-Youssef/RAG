using Google.Protobuf.Collections;
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
    public async Task AddAsync(string collection, DocumentChunk chunk, float[] vector)
    {
        PointStruct point = new()
        {
            Id = Guid.NewGuid(),
            Vectors = vector.ToArray()
        };

        FillPayload(point.Payload, chunk);

        await _client.UpsertAsync(collection, [point]);
    }

    /// <inheritdoc/>
    public async Task AddRangeAsync(string collection, IEnumerable<(DocumentChunk chunk, float[] vector)> items)
    {
        IEnumerable<PointStruct> points = items.Select(item =>
        {
            PointStruct point = new()
            {
                Id = Guid.NewGuid(),
                Vectors = item.vector.ToArray()
            };

            FillPayload(point.Payload, item.chunk);
            return point;
        });

        await _client.UpsertAsync(collection, points.ToList());
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
    public async Task DeleteAsync(string collection, Guid id)
    {
        await _client.DeleteAsync(collection, id);
    }

    /// <inheritdoc/>
    public async Task DeleteCollectionAsync(string name)
    {
        await _client.DeleteCollectionAsync(name);
    }

    /// <inheritdoc/>
    public async Task<DocumentChunk?> GetById(string collection, Guid id)
    {
        List<RetrievedPoint> response = [.. (await _client.RetrieveAsync(collection, id))];
        RetrievedPoint? result = response.FirstOrDefault();

        return result is not null ? FromPayload(result.Payload) : null;
    }

    /// <inheritdoc/>
    public async Task<IEnumerable<string>> ListCollectionsAsync()
    {
        // Get all collections and return their names
        return await _client.ListCollectionsAsync();
    }

    /// <inheritdoc/>
    public async Task<List<(DocumentChunk Chunk, float Score)>> SearchAsync(string collection, float[] queryVector, int topK, IDictionary<string, string>? metadataFilters = null)
    {
        Filter? filter = metadataFilters != null && metadataFilters.Any()
             ? new Filter
             {
                 Must = {
                    metadataFilters.Select(f => new Condition
                    {
                        Field = new FieldCondition
                        {
                            Key = f.Key,
                            Match = new Match { Text = f.Value }
                        }
                    })
                 }
             }
             : null;

        var results = await _client.SearchAsync(collection, queryVector, filter, limit:(ulong)topK);

        return results.Select(res =>
            (Chunk: FromPayload(res.Payload), res.Score)
        ).ToList();
    }

    /// <inheritdoc/>
    public async Task UpsertAsync(string collection, Guid id, DocumentChunk chunk, float[] vector)
    {
        PointStruct point = new()
        {
            Id = id,
            Vectors = vector
        };

        FillPayload(point.Payload, chunk);

        await _client.UpsertAsync(collection, [point]);
    }

    // ----------- Private Helpers -----------

    private static void FillPayload(MapField<string, Value> payload, DocumentChunk chunk)
    {
        payload["index"] = new Value(chunk.Index);
        payload["content"] = new Value(chunk.Content);
        payload["document_id"] = new Value(chunk.Metadata.DocumentId.ToString() ?? string.Empty);

        if (chunk.Metadata.PageNumber is int pageNum)
            payload["page_number"] = new Value(pageNum);

        if(chunk.Metadata.Custom is not null)
        {
            foreach (var tag in chunk.Metadata.Custom)
            {
                payload[tag.Key] = new Value(tag.Value);
            }
        }
    }

    private static DocumentChunk FromPayload(MapField<string, Value> payload)
    {
        var chunk = new DocumentChunk
        {
            Index = (int)payload["index"].IntegerValue,
            Content = payload["content"].StringValue,
            Metadata = new DocumentMetadata
            {
                DocumentId = Guid.Parse(payload.TryGetValue("document_id", out var docIdVal) ? docIdVal.StringValue : ""),
                PageNumber = payload.TryGetValue("page_number", out var pageVal) ? (int?)pageVal.IntegerValue : null,
                Custom = payload
                    .Where(kvp => kvp.Key != "index" && kvp.Key != "content" && kvp.Key != "document_id" && kvp.Key != "page_number")
                    .ToDictionary(kvp => kvp.Key, kvp => kvp.Value.StringValue)
            }
        };

        return chunk;
    }
}
