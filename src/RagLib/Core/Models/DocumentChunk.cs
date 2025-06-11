namespace RagLib.Core.Models;

/// <summary>
/// Represents a semantically meaningful chunk of a document,
/// enriched with metadata to enable retrieval, filtering, and reconstruction.
/// </summary>
public class DocumentChunk
{
    /// <summary>
    /// The sequential index of the chunk across the entire document.
    /// Useful for reconstructing the original order or referencing a chunk in isolation.
    /// This value always starts at 0 and increments for each chunk, regardless of page boundaries.
    /// </summary>
    public int Index { get; set; }

    /// <summary>
    /// The textual content of the chunk.
    /// This should be a coherent section of the original document, often sentence- or paragraph-aligned.
    /// </summary>
    public string Content { get; set; } = string.Empty;

    /// <summary>
    /// The metadata associated with this chunk.
    /// Includes source document ID, page number, file info, and optional custom tags.
    /// </summary>
    public DocumentMetadata Metadata { get; set; } = new();
}
