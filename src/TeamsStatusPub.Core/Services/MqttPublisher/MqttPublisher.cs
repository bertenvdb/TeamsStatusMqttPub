using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using TeamsStatusPub.Core.Models;
using MQTTnet;
using MQTTnet.Client;

namespace TeamsStatusPub.Core.Services.MqttPublisher;

public class MqttPublisher(ILogger<MqttPublisher> logger, IOptions<MqttSettings> mqttSettings) : IMqttPublisher
{
    private readonly MqttSettings _settings = mqttSettings.Value;

    public async Task PublishMessage(string message)
    {
        var mqttFactory = new MqttFactory();

        using var mqttClient = mqttFactory.CreateMqttClient();
        var mqttClientOptions = new MqttClientOptionsBuilder()
            .WithTcpServer(_settings.Host)
            .WithCredentials(_settings.Username, _settings.Password)
            .Build();

        try
        {
            await mqttClient.ConnectAsync(mqttClientOptions, CancellationToken.None);
        }
        catch (Exception e)
        {
            logger.LogError(e, "Failed to connect to MQTT broker");
            throw;
        }

        var applicationMessage = new MqttApplicationMessageBuilder()
            .WithTopic(_settings.Topic)
            .WithPayload(message)
            .Build();

        await mqttClient.PublishAsync(applicationMessage, CancellationToken.None);

        logger.LogInformation("Published status: {message}", message);

        await mqttClient.DisconnectAsync();
    }
}
