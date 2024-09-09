namespace TeamsStatusMqttPub.Core.Services.StatusPoller;

public interface IStatusPoller
{
    Task Start(Func<bool>? availabilityHandler);
}
