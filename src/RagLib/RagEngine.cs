using RagLib.Core.Interfaces;
using RagLib.Core.Models;

namespace RagLib;

/// <inheritdoc/>
public class RagEngine : IRagEngine
{
    /// <inheritdoc/>
    public Task<string> AskAsync(string query, RagQueryOptions? options = null)
    {
        throw new NotImplementedException();
    }

    /// <inheritdoc/>
    public Task IngestAsync(string document, IDictionary<string, string>? metadata = null)
    {
        throw new NotImplementedException();
    }
}
