namespace TeamsStatusMqttPub.Core.Services.MqttPublisher;

public interface IMqttPublisher
{
    Task PublishMessage(string message);
}
