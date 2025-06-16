using RagLib.Core.Models;
using RagLib.VectorStores;

namespace Samples.Console.VectorStores;
/// <summary>
/// Demonstrates basic usage of QdrantVectorStore:
/// - Creating a collection
/// - Adding document chunks with dummy vectors
/// - Performing a similarity search
/// - Listing and deleting collections
/// </summary>
public static class QdrantVectorStoreSample
{
    /// <summary>
    /// Runs a complete vector store workflow using a dummy vector.
    /// Requires a local Qdrant instance running (default: http://localhost:6334).
    /// </summary>
    public static async Task RunSample()
    {
        // Initialize the Qdrant vector store (assumes local Qdrant instance running on port 6334)
        QdrantVectorStore store = new("localhost");

        string collectionName = "sample-documents";

        // 1. Create a collection (you can only create it once, then reuse)
        System.Console.WriteLine("Creating collection...");
        await store.CreateCollectionAsync(collectionName);

        // 2. Prepare a sample document chunk
        DocumentChunk chunk = new()
        {
            Index = 0,
            Content = "The capital of France is Paris.",
            Metadata = new DocumentMetadata
            {
                DocumentId = Guid.NewGuid(),
                PageNumber = 1,
                Custom = new Dictionary<string, string>
                {
                    ["source"] = "encyclopedia"
                }
            }
        };

        float[] vector = CreateDummyVector(1536); // replace with real embedding vector

        // 3. Add the chunk to the vector store
        System.Console.WriteLine("Adding document chunk...");
        await store.AddAsync(collectionName, chunk, vector);

        // 4. Search for similar documents (using the same vector as query)
        System.Console.WriteLine("Searching for similar documents...");
        List<(DocumentChunk Chunk, float Score)> results = await store.SearchAsync(collectionName, vector, topK: 5);

        foreach ((DocumentChunk Chunk, float Score) in results)
        {
            System.Console.WriteLine($"Found chunk with score {Score:F4}: {Chunk.Content}");
        }

        // 5. List all collections
        System.Console.WriteLine("Collections:");
        IEnumerable<string> collections = await store.ListCollectionsAsync();
        foreach (string name in collections)
        {
            System.Console.WriteLine($" - {name}");
        }

        // 6. Delete the collection 
        await store.DeleteCollectionAsync(collectionName);
    }

    /// <summary>
    /// Generates a dummy vector with fixed values for demo purposes.
    /// Replace with real embeddings in production.
    /// </summary>
    static float[] CreateDummyVector(int dimensions)
    {
        float[] vector = new float[dimensions];
        for (int i = 0; i < dimensions; i++)
        {
            vector[i] = (float)(Math.Sin(i) * 0.01); // example values
        }
        return vector;
    }
}
