using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MQTTnet;
using MQTTnet.Client;
using TeamsStatusMqttPub.Core.Models;

namespace TeamsStatusMqttPub.Core.Services.MqttPublisher;

public class MqttPublisher(ILogger<MqttPublisher> logger, IOptions<MqttSettings> mqttSettings) : IMqttPublisher
{
    private readonly MqttSettings _settings = mqttSettings.Value;

    public async Task PublishMessage(string message)
    {
        var mqttFactory = new MqttFactory();

        using IMqttClient? mqttClient = mqttFactory.CreateMqttClient();
        MqttClientOptions? mqttClientOptions = new MqttClientOptionsBuilder()
            .WithTcpServer(_settings.Host)
            .WithClientId(_settings.ClientId)
            .WithCredentials(_settings.Username, _settings.Password)
            .Build();

        try
        {
            await mqttClient.ConnectAsync(mqttClientOptions, CancellationToken.None);

            MqttApplicationMessage? applicationMessage = new MqttApplicationMessageBuilder()
                .WithTopic(_settings.Topic)
                .WithPayload(message)
                .Build();

            await mqttClient.PublishAsync(applicationMessage, CancellationToken.None);

            logger.LogInformation("Published status: {message}", message);
        }
        catch (Exception e)
        {
            logger.LogError(e, "Failed to publish to MQTT broker");
            throw;
        }
        finally
        {
            await mqttClient.DisconnectAsync();
        }
    }
}
