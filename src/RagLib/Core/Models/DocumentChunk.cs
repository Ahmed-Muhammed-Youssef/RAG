namespace RagLib.Core.Models;

/// <summary>
/// Represents a chunk of a document with optional metadata.
/// </summary>
public class DocumentChunk
{
    public int Id { get; set; }
    public string Content { get; set; } = string.Empty;
    public IDictionary<string, string>? Metadata { get; set; }
}
