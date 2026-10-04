using Microsoft.Extensions.DependencyInjection;

using SignalVisualizer.Services;
using SignalVisualizer.ViewModels;

namespace SignalVisualizer.DependencyInjection;

/// <summary>
/// Registers the application's services and view models with the dependency injection container.
/// </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Adds the application's services and view models to the service collection.
    /// Graph generators are registered in the order they should appear in the UI.
    /// </summary>
    /// <param name="services">The service collection to configure.</param>
    /// <returns>The same service collection instance, to allow method chaining.</returns>
    public static IServiceCollection AddAppServices(this IServiceCollection services)
    {
        services.AddSingleton<ITextToBinaryConverter, TextToBinaryConverter>();
        services.AddSingleton<IGraphGenerator, FrequencyShiftKeyGenerator>();
        services.AddSingleton<IGraphGenerator, NonReturnToZeroGenerator>();
        services.AddSingleton<IGraphGenerator, AmplitudeShiftKeyGenerator>();

        services.AddTransient<MainViewModel>();

        return services;
    }
}
