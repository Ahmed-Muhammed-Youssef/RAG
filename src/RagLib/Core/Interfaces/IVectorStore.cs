using RagLib.Core.Models;

namespace RagLib.Core.Interfaces;

public interface IVectorStore
{
    /// <summary>
    /// Adds a new document chunk and its embedding to the store.
    /// </summary>
    /// <param name="chunk">The document chunk.</param>
    /// <param name="vector">The corresponding embedding vector.</param>
    void Add(DocumentChunk chunk, List<float> vector);

    /// <summary>
    /// Retrieves the most similar document chunks based on vector similarity.
    /// </summary>
    /// <param name="queryVector">The embedding of the user's query.</param>
    /// <param name="topK">The maximum number of chunks to return.</param>
    /// <param name="metadataFilters">Optional metadata filters (e.g., by source or tags).</param>
    /// <returns>A list of relevant document chunks.</returns>
    List<DocumentChunk> Search(List<float> queryVector, int topK, IDictionary<string, string>? metadataFilters = null);

}
