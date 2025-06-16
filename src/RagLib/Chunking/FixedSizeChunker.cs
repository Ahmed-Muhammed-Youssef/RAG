using Microsoft.Extensions.Options;
using RagLib.Core.Interfaces;
using RagLib.Core.Models;

namespace RagLib.Chunking;

public class FixedSizeChunker : IDocumentChunker
{
    private readonly ChunkerOptions _options;
    public FixedSizeChunker(IOptions<ChunkerOptions> options)
    {
        _options = options.Value;
    }

    /// <inheritdoc />
    public List<DocumentChunk> Chunk(string text, DocumentMetadata? documentMetadata = null)
    {
        List<DocumentChunk> result = [];
        int position = 0;
        documentMetadata ??= new DocumentMetadata
        {
            DocumentId = Guid.NewGuid()
        };

        while (position < text.Length)
        {
            int remaining = text.Length - position;
            int size = Math.Min(_options.ChunkSize, remaining);

            string chunkText = text.Substring(position, size);
            
            DocumentChunk chunk = new()
            {
                Content = chunkText,
                Metadata = documentMetadata
            };
            
            result.Add(chunk);
            position += size;
        }

        return result;
    }
}
