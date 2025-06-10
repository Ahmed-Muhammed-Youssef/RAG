using RagLib.Core.Models;

namespace RagLib.Core.Interfaces
{
    public interface IDocumentChunker
    {
        /// <summary>
        /// Splits the input text into a list of chunks suitable for embedding and storage.
        /// </summary>
        /// <param name="text">The full document text.</param>
        /// <returns>List of document chunks.</returns>
        List<DocumentChunk> Chunk(string text);
    }
}
