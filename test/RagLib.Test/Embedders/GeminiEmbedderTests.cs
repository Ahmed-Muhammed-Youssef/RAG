using RagLib.Embedders;
using RichardSzalay.MockHttp;

namespace RagLib.Test.Embedders;

public class GeminiEmbedderTests
{
    [Fact]
    public async Task EmbedAsync_Returns_ValidEmbedding()
    {
        // Arrange
        MockHttpMessageHandler mockHandler = new MockHttpMessageHandler();
        mockHandler.When("*").Respond("application/json",
            @"{ ""embedding"": { ""values"": [1.1, 2.2, 3.3] } }");

        HttpClient mockHttpClient = new(mockHandler);

        GeminiEmbedder embedder = new("fake-api-key", mockHttpClient);

        // Act
        List<float> result = await embedder.EmbedAsync("test");

        // Assert
        Assert.Equal([1.1f, 2.2f, 3.3f], result);
    }
}
