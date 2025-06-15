using RagLib.Core.Models;

namespace RagLib.Core.Interfaces;

/// <summary>
/// Represents an abstraction over a vector store capable of managing collections of embedded document chunks.
/// Supports creation, deletion, and search operations across named collections.
/// </summary>
public interface IVectorStore
{
    // ----------- Collection Management -----------

    /// <summary>
    /// Creates a new named collection in the vector store.
    /// A collection is a logical grouping of related vectorized document chunks (e.g., per domain or dataset).
    /// </summary>
    /// <param name="name">The unique name of the collection to create.</param>
    Task CreateCollectionAsync(string name);

    /// <summary>
    /// Permanently deletes a named collection and all of its contents from the vector store.
    /// </summary>
    /// <param name="name">The name of the collection to remove.</param>
    Task DeleteCollectionAsync(string name);

    /// <summary>
    /// Retrieves the names of all existing collections currently stored in the vector store.
    /// </summary>
    /// <returns>A collection of collection names.</returns>
    Task<IEnumerable<string>> ListCollectionsAsync();


    // ----------- CRUD Operations -----------

    /// <summary>
    /// Adds a single embedded document chunk to a specified collection.
    /// </summary>
    /// <param name="collection">The name of the target collection.</param>
    /// <param name="chunk">The document chunk to store, including optional metadata.</param>
    /// <param name="vector">The embedding vector associated with the document chunk.</param>
    Task AddAsync(string collection, DocumentChunk chunk, float[] vector);

    /// <summary>
    /// Adds multiple embedded document chunks to the specified collection in a single batch.
    /// </summary>
    /// <param name="collection">The name of the target collection.</param>
    /// <param name="items">A list of document chunks paired with their embedding vectors.</param>
    Task AddRangeAsync(string collection, IEnumerable<(DocumentChunk chunk, float[] vector)> items);

    /// <summary>
    /// Inserts or replaces a document chunk identified by its ID in the given collection.
    /// If the ID already exists, the chunk and vector will be updated.
    /// </summary>
    /// <param name="collection">The name of the target collection.</param>
    /// <param name="id">The unique identifier for the document chunk.</param>
    /// <param name="chunk">The document chunk to store or update.</param>
    /// <param name="vector">The embedding vector associated with the chunk.</param>
    Task UpsertAsync(string collection, Guid id, DocumentChunk chunk, float[] vector);

    /// <summary>
    /// Deletes a document chunk from a collection by its unique ID.
    /// </summary>
    /// <param name="collection">The name of the collection to delete from.</param>
    /// <param name="id">The unique identifier of the chunk to delete.</param>
    Task DeleteAsync(string collection, Guid id);

    /// <summary>
    /// Retrieves a document chunk by its ID from the specified collection.
    /// </summary>
    /// <param name="collection">The name of the collection containing the chunk.</param>
    /// <param name="id">The unique identifier of the chunk.</param>
    /// <returns>The corresponding document chunk, or null if not found.</returns>
    Task<DocumentChunk?> GetById(string collection, Guid id);


    // ----------- Vector Search -----------

    /// <summary>
    /// Searches for the most similar document chunks to a given query vector within a specified collection.
    /// </summary>
    /// <param name="collection">The name of the collection to search in.</param>
    /// <param name="queryVector">The embedding vector representing the search query.</param>
    /// <param name="topK">The number of top results to return.</param>
    /// <param name="metadataFilters">Optional key-value filters to narrow the search results (e.g., by source, type, or tags).</param>
    /// <returns>A list of matched document chunks with their associated similarity scores.</returns>
    Task<List<(DocumentChunk Chunk, float Score)>> SearchAsync(
        string collection,
        float[] queryVector,
        int topK,
        IDictionary<string, string>? metadataFilters = null
    );
}
