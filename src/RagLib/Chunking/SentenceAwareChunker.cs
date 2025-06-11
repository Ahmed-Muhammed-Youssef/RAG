using Microsoft.Extensions.Options;
using RagLib.Core.Interfaces;
using RagLib.Core.Models;
using System.Text;
using System.Text.RegularExpressions;

namespace RagLib.Chunking;

/// <inheritdoc/>
public partial class SentenceAwareChunker : IDocumentChunker
{
    private readonly ChunkerOptions _options;

    public SentenceAwareChunker(IOptions<ChunkerOptions> options)
    {
        _options = options.Value;
    }

    /// <inheritdoc/>
    public List<DocumentChunk> Chunk(string text, DocumentMetadata? metadata = null)
    {
        if (string.IsNullOrWhiteSpace(text))
            return [];

        metadata ??= new DocumentMetadata()
        {
            DocumentId = Guid.NewGuid()
        };

        List<string> sentences = SplitIntoSentences(text);
        List<DocumentChunk> chunks = [];

        List<string> currentChunk = [];
        int currentLength = 0;
        int chunkIndex = 0;

        for (int i = 0; i < sentences.Count; i++)
        {
            string sentence = sentences[i];
            currentChunk.Add(sentence);
            currentLength += sentence.Length;

            bool isChunkReady = currentLength >= _options.ChunkSize;

            bool isLastSentence = (i == sentences.Count - 1);
            if (isChunkReady || isLastSentence)
            {
                var chunkText = string.Join(" ", currentChunk);
                if (chunkText.Length >= _options.MinChunkSize)
                {
                    chunks.Add(new DocumentChunk
                    {
                        Index = chunkIndex++,
                        Content = chunkText,
                        Metadata = metadata
                    });
                }

                // Overlap: Reuse last X characters as start of next chunk
                string overlap = GetOverlapText(currentChunk, _options.ChunkOverlap);
                currentChunk = [overlap];
                currentLength = overlap.Length;
            }
        }

        return chunks;
    }

    /// <summary>
    /// Splits text into sentences using a regex-based heuristic.
    /// </summary>
    private static List<string> SplitIntoSentences(string text)
    {
        Regex sentenceEndRegex = SplitIntoSenetencesRegx();
        return sentenceEndRegex.Split(text).Where(s => !string.IsNullOrWhiteSpace(s)).ToList();
    }

    /// <summary>
    /// Gets the trailing overlapping text from the end of the chunk.
    /// </summary>
    private static string GetOverlapText(List<string> sentences, int targetLength)
    {
        StringBuilder sb = new();
        int total = 0;

        for (int i = sentences.Count - 1; i >= 0; i--)
        {
            string sentence = sentences[i];

            // Prepend sentence to the front of the StringBuilder
            sb.Insert(0, sentence + " ");
            total += sentence.Length;

            if (total >= targetLength)
                break;
        }

        return sb.ToString().TrimEnd();
    }

    [GeneratedRegex(@"(?<=[\.!?])\s+(?=[A-Z])", RegexOptions.Compiled)]
    private static partial Regex SplitIntoSenetencesRegx();
}
