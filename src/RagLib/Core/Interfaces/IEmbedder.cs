namespace RagLib.Core.Interfaces;

public interface IEmbedder
{
    /// <summary>
    /// Generates an embedding vector for the provided input text.
    /// </summary>
    /// <param name="text">Input string to embed.</param>
    /// <returns>Vector representation of the text.</returns>
    Task<List<float>> EmbedAsync(string text);
}
