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
    /// <param name="document">Raw document content (text).</param>
    /// <param name="metadata">Optional metadata to associate with the document (e.g., source name or tags).</param>
    Task IngestAsync(string document, IDictionary<string, string>? metadata = null);

    /// <summary>
    /// Asks a natural language question and returns a generated response using retrieved context.
    /// </summary>
    /// <param name="query">User's input question.</param>
    /// <param name="options">Optional override settings (e.g., number of chunks to retrieve).</param>
    /// <returns>Generated answer based on relevant context.</returns>
    Task<string> AskAsync(string query, RagQueryOptions? options = null);
}
