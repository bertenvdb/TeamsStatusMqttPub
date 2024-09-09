namespace TeamsStatusMqttPub.Core.Models;

/// <summary>
/// Status Poller settings from the
/// <see cref="AppConfiguration.SettingsFileName"/> file.
/// </summary>
public record StatusPollerSettings
{
    /// <summary>
    /// The name of the section in the configuration file.
    /// </summary>
    public const string SectionName = "StatusPoller";

    /// <summary>
    /// The interval in seconds at which to poll for the current status.
    /// </summary>
    public int PollingInterval { get; init; }
}
