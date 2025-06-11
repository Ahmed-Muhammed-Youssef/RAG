namespace RagLib.Core.Models;

public class ChunkerOptions
{
    /// <summary>
    /// Target number of characters per chunk.
    /// </summary>
    public int ChunkSize { get; set; } = 500;

    /// <summary>
    /// Number of characters of overlap between adjacent chunks.
    /// </summary>
    public int ChunkOverlap { get; set; } = 100;

    /// <summary>
    /// Minimum number of characters to include in a chunk. Prevents too-small fragments.
    /// </summary>
    public int MinChunkSize { get; set; } = 200;
}
