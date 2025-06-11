using RagLib.Core.Models;

namespace RagLib.Core.Interfaces;

/// <summary>
/// Defines a contract for splitting a plain text document into meaningful chunks,
/// typically sentence- or paragraph-aligned, for use in retrieval-augmented generation (RAG) systems.
/// </summary>
public interface IDocumentChunker
{
    /// <summary>
    /// Splits the input text into a list of sentence-aware chunks.
    /// </summary>
    /// <param name="text">The input document as plain text.</param>
    /// <param name="documentMetadata">Optional document metadata.</param>
    /// <returns>A list of document chunks.</returns>
    List<DocumentChunk> Chunk(string text, DocumentMetadata? documentMetadata = null);
}
