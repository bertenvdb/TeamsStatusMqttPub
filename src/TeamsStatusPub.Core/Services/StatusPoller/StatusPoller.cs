using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using TeamsStatusPub.Core.Models;
using TeamsStatusPub.Core.Services.MqttPublisher;

namespace TeamsStatusPub.Core.Services.StatusPoller;

public class StatusPoller : IStatusPoller
{
    private readonly ILogger<StatusPoller> _logger;
    private readonly StatusPollerSettings _settings;
    private readonly IMqttPublisher _mqttPublisher;
    private string? _status;

    private Timer _timer;

    public StatusPoller(
        ILogger<StatusPoller> logger,
        IOptions<StatusPollerSettings> settings,
        IMqttPublisher mqttPublisher)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _settings = settings.Value ?? throw new ArgumentNullException(nameof(settings));
        _mqttPublisher = mqttPublisher ?? throw new ArgumentNullException(nameof(mqttPublisher));
    }

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
        var newStatus = availabilityHandler.Invoke().ToString();
        _logger.LogDebug("Found status: {newStatus}", newStatus);

        if (_status != newStatus)
        {
            _status = newStatus;
            _mqttPublisher.PublishMessage(_status);
        }
    }
}
