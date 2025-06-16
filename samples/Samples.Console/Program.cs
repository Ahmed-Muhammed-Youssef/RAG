using Samples.Console.Embedders;
using Samples.Console.RagEngine;
using Samples.Console.VectorStores;

namespace Samples.Console;

internal static class Program
{
    static async Task Main()
    {
        await GeminiEmbedderSample.RunSample();
        await QdrantVectorStoreSample.RunSample();
        await RagEngineSample.RunSample();
    }  
}
