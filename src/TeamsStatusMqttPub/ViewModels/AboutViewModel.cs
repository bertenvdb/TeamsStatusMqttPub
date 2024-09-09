using System;
using System.ComponentModel;
using System.Reflection;
using Microsoft.Extensions.Options;
using TeamsStatusMqttPub.Core.Models;
using TeamsStatusMqttPub.Core.Services;
using TeamsStatusMqttPub.Core.Services.AvailabilityHandlers;

namespace TeamsStatusMqttPub.ViewModels;

public class AboutViewModel : ViewModelBase
{
    private readonly IAppInfo _appInfo;
    private readonly IAvailabilityHandler _availabilityHandler;
    private readonly RuntimeSettings _runtimeSettings;

    public AboutViewModel(IAppInfo appInfo, IOptions<RuntimeSettings> runtimeSettings,
        IAvailabilityHandler availabilityHandler)
    {
        ArgumentNullException.ThrowIfNull(runtimeSettings);
        _appInfo = appInfo ?? throw new ArgumentNullException(nameof(appInfo));
        _runtimeSettings = runtimeSettings.Value ?? throw new ArgumentNullException(nameof(runtimeSettings));
        _availabilityHandler = availabilityHandler ?? throw new ArgumentNullException(nameof(availabilityHandler));
    }

    public string ApplicationName => _appInfo.ApplicationName;
    public string Copyright => _appInfo.Copyright;
    public string WebsiteUrl => _appInfo.WebsiteUrl;
    public string Version => _appInfo.Version;

    public string LastAvailabilitySystemStatus
    {
        get
        {
            string status = _availabilityHandler.IsAvailable() ? "not busy" : "busy";
            var handlerName = _runtimeSettings.AvailabilityHandler
                .GetType()
                .GetMember(_runtimeSettings.AvailabilityHandler.ToString())[0]
                .GetCustomAttribute(typeof(DescriptionAttribute), false) as DescriptionAttribute;

            return handlerName is null
                ? throw new NotImplementedException("Missing expected Description attribute")
                : $"{handlerName.Description}: {status}";
        }
    }
}
