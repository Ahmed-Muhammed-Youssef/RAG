using Microsoft.Extensions.Options;
using RagLib.Chunking;
using RagLib.Core.Interfaces;
using RagLib.Core.Models;
using RagLib.Embedders;
using RagLib.VectorStores;

namespace RagLib.Core.Builders;

/// <summary>
/// Fluent builder for constructing a RagEngine with configurable components.
/// </summary>
public class RagEngineBuilder
{
    private IEmbedder? _embedder;
    private IVectorStore? _vectorStore;
    private IDocumentChunker? _chunker;
    private readonly HttpClientHandler _httpClientHandler;

    private RagEngineBuilder() 
    {
        _httpClientHandler = new();
    }

    /// <summary>
    /// Starts building a new RagEngine instance.
    /// </summary>
    public static RagEngineBuilder Create() => new();

    /// <summary>
    /// Configures a custom embedder implementation.
    /// </summary>
    public RagEngineBuilder UseEmbedder(IEmbedder embedder)
    {
        _embedder = embedder;
        return this;
    }

    /// <summary>
    /// Configures a Gemini-based embedder.
    /// </summary>
    public RagEngineBuilder UseGeminiEmbedder(string apiKey)
    {
        _embedder = new GeminiEmbedder(apiKey, new HttpClient(_httpClientHandler));
        return this;
    }

    /// <summary>
    /// Configures a custom vector store implementation.
    /// </summary>
    public RagEngineBuilder UseVectorStore(IVectorStore vectorStore)
    {
        _vectorStore = vectorStore;
        return this;
    }

    /// <summary>
    /// Configures a Qdrant-based vector store.
    /// </summary>
    public RagEngineBuilder UseQdrantStore(string endpoint)
    {
        _vectorStore = new QdrantVectorStore(endpoint);
        return this;
    }

    /// <summary>
    /// Configures a custom chunker implementation.
    /// </summary>
    public RagEngineBuilder UseChunker(IDocumentChunker chunker)
    {
        _chunker = chunker;
        return this;
    }

    /// <summary>
    /// Configures a fixed-size chunker with the given character size.
    /// </summary>
    public RagEngineBuilder UseFixedSizeChunker(int chunkSize)
    {
        var options = Options.Create(new ChunkerOptions { ChunkSize = chunkSize });
        _chunker = new FixedSizeChunker(options);
        return this;
    }

    /// <summary>
    /// Builds and returns the configured RagEngine instance.
    /// </summary>
    public RagEngine Build()
    {
        if (_chunker == null)
                throw new InvalidOperationException("Chunker is not configured.");
            if (_embedder == null)
                throw new InvalidOperationException("Embedder is not configured.");
            if (_vectorStore == null)
                throw new InvalidOperationException("Vector store is not configured.");

            return new RagEngine(_chunker, _embedder, _vectorStore);
    }
}
