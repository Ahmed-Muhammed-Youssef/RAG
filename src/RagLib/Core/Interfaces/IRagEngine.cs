using RagLib.Core.Models;

namespace RagLib.Core.Interfaces;

/// <summary>
/// Interface for a customizable Retrieval-Augmented Generation (RAG) engine.
/// </summary>
public interface IRagEngine
{
    /// <summary>
    /// Ingests and stores document chunks for future retrieval.
    /// </summary>
    /// <param name="collection">Logical grouping of documents (e.g., "legal", "faq").</param>
    /// <param name="document">Raw document content (text).</param>
    /// <param name="metadata">Optional metadata to associate with the document (e.g., source name or tags).</param>
    Task IngestAsync(string collection, string document, DocumentMetadata? metadata = null);

    /// <summary>
    /// Retrieves relevant document chunks based on a query.
    /// </summary>
    /// <param name="collection"></param>
    /// <param name="query"></param>
    /// <param name="options"></param>
    /// <returns></returns>
    Task<List<DocumentChunk>> GetRelevantChunksAsync(string collection, string query, RagQueryOptions? options = null);
}
