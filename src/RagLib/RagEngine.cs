using RagLib.Core.Interfaces;
using RagLib.Core.Models;

namespace RagLib
{
    public class RagEngine : IRagEngine
    {
        /// <summary>
        /// Ingests and stores document chunks for future retrieval.
        /// </summary>
        /// <param name="document">Raw document content (text).</param>
        /// <param name="metadata">Optional metadata to associate with the document (e.g., source name or tags).</param>
        public Task<string> AskAsync(string query, RagQueryOptions? options = null)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Asks a natural language question and returns a generated response using retrieved context.
        /// </summary>
        /// <param name="query">User's input question.</param>
        /// <param name="options">Optional override settings (e.g., number of chunks to retrieve).</param>
        /// <returns>Generated answer based on relevant context.</returns>
        public Task IngestAsync(string document, IDictionary<string, string>? metadata = null)
        {
            throw new NotImplementedException();
        }
    }
}
