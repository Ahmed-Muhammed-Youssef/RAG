using Microsoft.Extensions.Options;
using RagLib.Chunking;
using RagLib.Core.Models;

namespace RagLib.Test.Chunking;

public class FixedSizeChunkerTests
{
    private static FixedSizeChunker CreateChunker(int chunkSize = 50, int minChunkSize = 10, int chunkOverlap = 20)
    {
        IOptions<ChunkerOptions> options = Options.Create(new ChunkerOptions
        {
            ChunkSize = chunkSize,
            MinChunkSize = minChunkSize,
            ChunkOverlap = chunkOverlap
        });
        return new FixedSizeChunker(options);
    }

    [Fact]
    public void Chunk_EmptyText_ReturnsEmptyList()
    {
        var chunker = CreateChunker();
        var chunks = chunker.Chunk("");
        Assert.Empty(chunks);
    }

    [Fact]
    public void Chunk_NullText_ThrowsArgumentNullException()
    {
        var chunker = CreateChunker();
        Assert.Throws<NullReferenceException>(() => chunker.Chunk(null!));
    }

    [Fact]
    public void Chunk_SingleChunkEqualToChunkSize_ReturnsOneChunk()
    {
        var chunker = CreateChunker(chunkSize: 10, minChunkSize: 5);
        var text = "abcdefghij"; // 10 characters
        var chunks = chunker.Chunk(text);
        Assert.Single(chunks);
        Assert.Equal("abcdefghij", chunks[0].Content);
    }

    [Fact]
    public void Chunk_TextExactlyMultipleOfChunkSize_ReturnsExpectedChunks()
    {
        var chunker = CreateChunker(chunkSize: 4, minChunkSize: 2);
        var text = "abcdefgh"; // 8 characters, expect 2 chunks
        var chunks = chunker.Chunk(text);
        Assert.Equal(2, chunks.Count);
        Assert.Equal("abcd", chunks[0].Content);
        Assert.Equal("efgh", chunks[1].Content);
    }


    [Fact]
    public void Chunk_LastChunkIsKeptEvenIfTooSmall()
    {
        var chunker = CreateChunker(chunkSize: 5);
        var text = "abcdefghijxyz"; // 13 chars -> 5,5,3
        var chunks = chunker.Chunk(text);
        Assert.Equal(3, chunks.Count);
        Assert.Equal("abcde", chunks[0].Content);
        Assert.Equal("fghij", chunks[1].Content);
        Assert.Equal("xyz", chunks[2].Content); // smaller than min, but allowed as last
    }

    [Fact]
    public void Chunk_AssignsProvidedMetadata()
    {
        var chunker = CreateChunker(chunkSize: 5);
        var metadata = new DocumentMetadata
        {
            DocumentId = Guid.NewGuid(),
            FileName = "test.txt"
        };

        var chunks = chunker.Chunk("abcdefghij", metadata);
        Assert.All(chunks, c =>
        {
            Assert.Equal(metadata.DocumentId, c.Metadata?.DocumentId);
            Assert.Equal("test.txt", c.Metadata?.FileName);
        });
    }

    [Fact]
    public void Chunk_LongText_GeneratesExpectedNumberOfChunks()
    {
        var chunker = CreateChunker(chunkSize: 10, minChunkSize: 5);
        var text = new string('x', 95); // Should produce 9 full chunks + 1 short one
        var chunks = chunker.Chunk(text);
        Assert.Equal(10, chunks.Count);
        Assert.All(chunks.Take(9), c => Assert.Equal(10, c.Content.Length));
        Assert.Equal(5, chunks.Last().Content.Length); // 95 % 10 == 5
    }
}
