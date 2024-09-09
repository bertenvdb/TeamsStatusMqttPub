namespace TeamsStatusPub.Core.Services.MqttPublisher;

public interface IMqttPublisher
{
    Task PublishMessage(string message);
}
