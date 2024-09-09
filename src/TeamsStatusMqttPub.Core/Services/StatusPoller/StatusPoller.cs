using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using TeamsStatusMqttPub.Core.Models;
using TeamsStatusMqttPub.Core.Services.MqttPublisher;

namespace TeamsStatusMqttPub.Core.Services.StatusPoller;

public class StatusPoller(
    ILogger<StatusPoller> logger,
    IOptions<StatusPollerSettings> settings,
    IMqttPublisher mqttPublisher)
    : IStatusPoller
{
    private readonly ILogger<StatusPoller> _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    private readonly IMqttPublisher _mqttPublisher = mqttPublisher ?? throw new ArgumentNullException(nameof(mqttPublisher));
    private readonly StatusPollerSettings _settings = settings.Value ?? throw new ArgumentNullException(nameof(settings));
    private string? _status;

    // ReSharper disable once NotAccessedField.Local
    private Timer? _timer;

    public Task Start(Func<bool>? availabilityHandler)
    {
        ArgumentNullException.ThrowIfNull(availabilityHandler);

        _timer = new(_ =>
        {
            PublishStatusIfChanged(availabilityHandler);
        }, null, TimeSpan.Zero, TimeSpan.FromSeconds(_settings.PollingInterval));

        return Task.CompletedTask;
    }

    private void PublishStatusIfChanged(Func<bool> availabilityHandler)
    {
        string newStatus = availabilityHandler.Invoke().ToString();
        _logger.LogDebug("Found status: {newStatus}", newStatus);

        if (_status != newStatus)
        {
            _status = newStatus;
            _mqttPublisher.PublishMessage(_status);
        }
    }
}
