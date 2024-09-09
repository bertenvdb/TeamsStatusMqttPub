using TeamsStatusMqttPub.Core.Configuration;

namespace TeamsStatusMqttPub.Core.Models;

/// <summary>
/// Application runtime settings from the
/// <see cref="AppConfiguration.SettingsFileName"/> file.
/// </summary>
public record RuntimeSettings
{
    /// <summary>
    /// The system to handle determining availability.
    /// </summary>
    public AvailabilitySystems AvailabilityHandler { get; init; }
}
