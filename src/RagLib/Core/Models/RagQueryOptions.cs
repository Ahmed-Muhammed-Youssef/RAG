namespace RagLib.Core.Models;

/// <summary>
/// Options to control RAG behavior at query time.
/// </summary>
public class RagQueryOptions
{
    /// <summary>
    /// Number of top matching chunks to retrieve.
    /// </summary>
    public int TopK { get; set; } = 3;

    /// <summary>
    /// Optional filter on chunk metadata (e.g., tags or source).
    /// </summary>
    public IDictionary<string, string>? MetadataFilters { get; set; }
}
