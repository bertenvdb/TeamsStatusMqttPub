using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using TeamsStatusMqttPub.Core.Models;
using TeamsStatusMqttPub.Core.Services;
using TeamsStatusMqttPub.Core.Services.MqttPublisher;
using TeamsStatusMqttPub.Core.Services.StatusPoller;

namespace TeamsStatusMqttPub.Core.Configuration;

/// <summary>
///     Extension methods for <see cref="IServiceCollection" /> that adds the application services.
/// </summary>
public static partial class ServiceConfiguration
{
    /// <summary>
    ///     Adds the application services into the service collection.
    /// </summary>
    /// <param name="services">The <see cref="IServiceCollection" /> instance.</param>
    public static void ConfigureAppServices(this IServiceCollection services)
    {
        services.AddAppLogging();

        services.AddSingleton<IStatusPoller, StatusPoller>();
        services.AddSingleton<IMqttPublisher, MqttPublisher>();

        services.AddSingleton<IAppInfo, AssemblyAppInfo>();

        IConfiguration configuration = AppConfiguration.Build();

        services.Configure<RuntimeSettings>(configuration.GetSection("Runtime"));
        services.Configure<StatusPollerSettings>(configuration.GetSection(StatusPollerSettings.SectionName));
        services.Configure<MqttSettings>(configuration.GetSection(MqttSettings.SectionName));

        using ServiceProvider sp = services.BuildServiceProvider();
        var runtimeSettings = sp.GetRequiredService<IOptions<RuntimeSettings>>();

        switch (runtimeSettings.Value.AvailabilityHandler)
        {
            case AvailabilitySystems.MicrosoftTeamsClassic:
                services.ConfigureMicrosoftTeamsClassicServices();
                break;

            case AvailabilitySystems.MicrosoftTeams:
                services.ConfigureMicrosoftTeamsServices();
                break;

            default:
                string message = string.Format("Unsupported 'AvailabilityHandler' value from {0}: {1}",
                    AppConfiguration.SettingsFileName, runtimeSettings.Value.AvailabilityHandler);
                throw new NotImplementedException(message);
        }
    }
}
