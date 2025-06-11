using Microsoft.Extensions.Options;
using RagLib.Chunking;
using RagLib.Core.Models;

namespace RagLib.Test.Chunking;

public class SentenceAwareChunkerTests
{
    private static SentenceAwareChunker CreateChunker(int chunkSize = 50, int minChunkSize = 10, int chunkOverlap = 20)
    {
        var options = Options.Create(new ChunkerOptions
        {
            ChunkSize = chunkSize,
            MinChunkSize = minChunkSize,
            ChunkOverlap = chunkOverlap
        });
        return new SentenceAwareChunker(options);
    }

    [Fact]
    public void Chunk_EmptyText_ReturnsEmptyList()
    {
        var chunker = CreateChunker();
        var chunks = chunker.Chunk("");
        Assert.Empty(chunks);
    }

    [Fact]
    public void Chunk_NullText_ReturnsEmptyList()
    {
        var chunker = CreateChunker();
        var chunks = chunker.Chunk(null!);
        Assert.Empty(chunks);
    }

    [Fact]
    public void Chunk_SingleShortSentence_ReturnsNoChunkIfUnderMin()
    {
        var chunker = CreateChunker(chunkSize: 100, minChunkSize: 20);
        var text = "Hi.";
        var chunks = chunker.Chunk(text);
        Assert.Empty(chunks);
    }

    [Fact]
    public void Chunk_SingleLongSentence_ReturnsOneChunk()
    {
        var chunker = CreateChunker(chunkSize: 100, minChunkSize: 10);
        var text = "This is a fairly long sentence that should be enough to form a chunk.";
        var chunks = chunker.Chunk(text);
        Assert.Single(chunks);
        Assert.Contains("This is a fairly long sentence", chunks[0].Content);
    }

    [Fact]
    public void Chunk_MultipleSentences_CreatesExpectedChunks()
    {
        var chunker = CreateChunker(chunkSize: 50, minChunkSize: 20, chunkOverlap: 15);
        var text = "This is sentence one. This is sentence two. This is sentence three. This is sentence four. This is sentence five.";
        var chunks = chunker.Chunk(text);

        Assert.True(chunks.Count >= 2, "Expected at least 2 chunks.");
        Assert.All(chunks, c => Assert.True(c.Content.Length >= 20));
        Assert.Equal(0, chunks[0].Index);
        Assert.Equal(1, chunks[1].Index);
    }

    [Fact]
    public void Chunk_OverlapIncludedBetweenChunks()
    {
        SentenceAwareChunker chunker = CreateChunker(chunkSize: 50, minChunkSize: 10, chunkOverlap: 25);
        string text = "Sentence A. Sentence B. Sentence C. Sentence D. Sentence E.";
        List<DocumentChunk> chunks = chunker.Chunk(text);

        Assert.True(chunks.Count >= 2);

        bool overlap = chunks[1].Content.StartsWith("Sentence C") || chunks[1].Content.StartsWith("Sentence B");
        Assert.True(overlap, "Second chunk should start with an overlapping sentence from the first.");
    }

    [Fact]
    public void Chunk_AssignsDefaultMetadataId_WhenMissing()
    {
        var chunker = CreateChunker();
        var chunks = chunker.Chunk("First sentence. Second sentence.");
        Assert.NotEqual(Guid.Empty, chunks[0].Metadata!.DocumentId);
    }

    [Fact]
    public void Chunk_UsesProvidedMetadata()
    {
        var chunker = CreateChunker();
        var metadata = new DocumentMetadata { DocumentId = Guid.NewGuid(), FileName = "TestDoc" };
        var chunks = chunker.Chunk("First sentence. Second sentence.", metadata);

        Assert.All(chunks, c => Assert.Equal(metadata.DocumentId, c.Metadata!.DocumentId));
        Assert.All(chunks, c => Assert.Equal("TestDoc", c.Metadata!.FileName));
    }

    [Fact]
    public void Chunk_RespectsChunkSizeAndMinSize()
    {
        var chunker = CreateChunker(chunkSize: 40, minChunkSize: 30);
        var text = "1234567890. abcdefghij. klmnopqrst. uvwxyzabcd.";
        var chunks = chunker.Chunk(text);

        Assert.All(chunks, chunk => Assert.True(chunk.Content.Length >= 30));
    }
}
