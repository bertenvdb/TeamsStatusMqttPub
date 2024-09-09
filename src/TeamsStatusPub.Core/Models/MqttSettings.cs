namespace TeamsStatusPub.Core.Models;

/// <summary>
/// MQTT settings from the
/// <see cref="AppConfiguration.SettingsFileName"/> file.
/// </summary>
public record MqttSettings
{
    /// <summary>
    /// The name of the section in the configuration file.
    /// </summary>
    public const string SectionName = "Mqtt";

    /// <summary>
    /// The host of the MQTT broker to connect to.
    /// </summary>
    public string Host { get; init; } = default!;

    /// <summary>
    /// The port to connect to the MQTT broker on.
    /// </summary>
    public string Port { get; init; } = default!;

    /// <summary>
    /// The topic to publish the message to.
    /// </summary>
    public string Topic { get; init; } = default!;

    /// <summary>
    /// The user to connect to the MQTT broker with.
    /// </summary>
    public string Username { get; init; } = default!;

    /// <summary>
    /// The password to connect to the MQTT broker with.
    /// </summary>
    public string Password { get; init; } = default!;
}
