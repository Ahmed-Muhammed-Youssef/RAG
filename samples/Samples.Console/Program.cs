using Samples.Console.Embedders;
using Samples.Console.VectorStores;

namespace Samples.Console;

internal static class Program
{
    static async Task Main()
    {
        await GeminiEmbedderSample.RunSample();
        await QdrantVectorStoreSample.RunSample();
    }  
}
