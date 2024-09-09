using Microsoft.Extensions.DependencyInjection;
using TeamsStatusMqttPub.Core.Services.AvailabilityHandlers;
using TeamsStatusMqttPub.Core.Services.AvailabilityHandlers.MicrosoftTeams;
using TeamsStatusMqttPub.Core.Services.AvailabilityHandlers.MicrosoftTeams.FileSystemProviders;

namespace TeamsStatusMqttPub.Core.Configuration;

public static partial class ServiceConfiguration
{
    /// <summary>
    ///     Adds the services required for Microsoft Teams into the service collection.
    /// </summary>
    /// <param name="services">The <see cref="IServiceCollection" /> instance.</param>
    /// <returns>The <see cref="IServiceCollection" />.</returns>
    private static IServiceCollection ConfigureMicrosoftTeamsServices(this IServiceCollection services)
    {
        services.AddTransient<IAvailabilityHandler, MicrosoftTeamsHandler>();
        services.AddTransient<IFileSystemProvider, FileSystemWrapper>();
        services.AddTransient<IDirectoryProvider, DirectoryWrapper>();
        services.AddTransient<ILogDiscovery, LogDiscovery>();

        return services;
    }
}
