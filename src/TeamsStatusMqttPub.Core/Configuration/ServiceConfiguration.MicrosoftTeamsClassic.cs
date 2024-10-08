﻿using Microsoft.Extensions.DependencyInjection;
using TeamsStatusMqttPub.Core.Services.AvailabilityHandlers;
using TeamsStatusMqttPub.Core.Services.AvailabilityHandlers.MicrosoftTeamsClassic;

namespace TeamsStatusMqttPub.Core.Configuration;

public static partial class ServiceConfiguration
{
    /// <summary>
    ///     Adds the services required for Microsoft Teams Classic into the service collection.
    /// </summary>
    /// <param name="services">The <see cref="IServiceCollection" /> instance.</param>
    /// <returns>The <see cref="IServiceCollection" />.</returns>
    private static IServiceCollection ConfigureMicrosoftTeamsClassicServices(this IServiceCollection services)
    {
        services.AddSingleton<IMicrosoftTeamsClassicFactory, MicrosoftTeamsClassicFactory>(sp => new(sp));
        services.AddTransient<IAvailabilityHandler, MicrosoftTeamsClassicHandler>();

        return services;
    }
}
