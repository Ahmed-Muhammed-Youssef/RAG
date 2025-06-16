using Samples.Console.Embedders;
using Samples.Console.VectorStores;

namespace Samples.Console;

public class Program
{
    static async Task Main()
    {
        await GeminiEmbedderSample.RunSample();
        await QdrantVectorStoreSample.RunSample();
    }  
}
