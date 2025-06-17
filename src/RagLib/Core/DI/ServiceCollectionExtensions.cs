using Microsoft.Extensions.DependencyInjection;
using RagLib.Core.Builders;
using RagLib.Core.Interfaces;

namespace RagLib.Core.DI;

public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Registers a configured <see cref="IRagEngine"/> instance in the service container using the provided builder configuration.
    /// </summary>
    /// <param name="services">The <see cref="IServiceCollection"/> to register services into.</param>
    /// <param name="builderConfig">
    /// A delegate that configures the <see cref="RagEngineBuilder"/> used to construct the <see cref="IRagEngine"/>.
    /// </param>
    /// <returns>The same <see cref="IServiceCollection"/> instance so that additional calls can be chained.</returns>
    public static IServiceCollection AddRagEngine(this IServiceCollection services, Action<RagEngineBuilder> builderConfig)
    {
        RagEngineBuilder ragBuilder = RagEngineBuilder.Create();

        builderConfig(ragBuilder);

        var engine = ragBuilder.Build();

        services.AddSingleton<IRagEngine>(engine);

        return services;
    }
}
