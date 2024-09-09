# Teams Status MQTT Pub

<img src="src/TeamsStatusMqttPub/Assets/logo.webp" width="100" alt="TeamsStatusMqttPub Logo" align="right" />

Publish your [Microsoft Teams](https://www.microsoft.com/en-us/microsoft-teams/group-chat-software/) Status to an MQTT broker. The MQTT message can then be picked up by home automation systems such as Home Assistant and can the in turn e.g.toggle power to a smart lightbulb when you're in a meeting.

Teams Status Pub runs on the system tray to publish your Teams status via MQTT. You're considered busy in Teams when on a call, regardless of video or mic status.

This is heavily based on Andrei Nicholson's [TeamsStatusPub](https://github.com/tetsuo13/TeamsStatusPub). I just made it publish the current status to an MQTT broker instead of replying with the status on http requests. 
I needed something low-effort which would make my status available for Home Assistant and this seemed like the way to go.

## Roadmap
- Publish the exact Teams status and not just a boolean representing your availability.

## Getting Started

- Your computer must be running Windows 10 or later.
- .NET 8 runtime.
- The desktop version of Teams must be installed, either Teams or Teams Classic.

Teasted against the following minimum versions and is known to work on the latest available version:

- Microsoft Teams 24033.811.2738.2546
- Microsoft Teams classic for Windows versions 1.5.00.21463

> [!NOTE]
> Microsoft Teams Classic is considered deprecated and will eventually be removed.

### Teams

No additional changes are required, the default settings already log sufficient data.

Note that there is sometimes a delay between the status shown in Teams and the logs used by Teams Status Pub. For Teams Classic this delay was rarely longer than a few seconds but in Teams it can be up to a few minutes. Resetting or otherwise changing the status in Teams doesn't seem to cause the log to be updated any faster.

Teams Status Pub will reflect the statuses in Teams that are deemed "not available" -- **Busy** and **Do not disturb**. This is slightly different than how availability was deemed with Teams Classic as it was strictly based on whether or not you were engaged in a call. This can be apparent after ending a meeting call early: in Teams you remain in **Busy** status until the end of the meeting so therefor Teams Status Pub reports as unavailable but in Teams Classic Teams Status Pub reports as available despite the status.

### Teams Status Pub

Download the installer from the [Releases](https://github.com/tetsuo13/TeamsStatusqttPub/releases) page. By default it will install to your `C:\Users\username\AppData\Local\Programs\TeamsStatusMqttPub` folder. As part of the installation there will be an `appsettings.json` file used to configure some basic settings.

| Setting | Description |
| ------- | ----------- |
| `Runtime:AvailabilityHandler` | Can be either "MicrosoftTeams" (default) or "MicrosoftTeamsClassic". |
| `StatusPoller:PollingInterval` | The interval in seconds at which to poll for the current status. |
| `Mqtt:Host` | The host of the MQTT broker to connect to. |
| `Mqtt:Port` | The port to connect to the MQTT broker on. |
| `Mqtt:Topic` | The topic to publish the message to. |
| `Mqtt:Username` | The user to connect to the MQTT broker with. |
| `Mqtt:Password` | The password to connect to the MQTT broker with. |

## Troubleshooting

To enable logging, change the `Serilog:MinimumLevel:Default` value in `appsettings.json` to "Information" (use "Debug" to increase the verbosity level). The default value that's there is set in order to disable logging. An application restart will be required. By changing the value, a log file will be written in the same directory as the application.

## Development

This was written as an Avalonia application using the MVVM design pattern.

There has been some consideration taken to make the code modular enough to handle integrating with other conferencing tools other than Microsoft Teams. In the future it could be possible to add additional availability handlers and use configuration to determine which handler(s) to use. A detailed breakdown of handlers and how a user is considered busy can be found in the [README](src/TeamsStatusMqttPub.Core/Services/AvailabilityHandlers/README.md) file in the availability handlers directory.

## License

See [LICENSE](LICENSE) 
