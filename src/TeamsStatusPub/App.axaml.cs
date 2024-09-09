using System.Reactive.Concurrency;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Logging;
using Avalonia.Markup.Xaml;
using Microsoft.Extensions.DependencyInjection;
using ReactiveUI;
using TeamsStatusPub.ViewModels;
using TeamsStatusPub.Core.Configuration;
using TeamsStatusPub.Core.Services.AvailabilityHandlers;
using TeamsStatusPub.Core.Services.StatusPoller;

namespace TeamsStatusPub;

public class App : Application
{
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    public static ServiceProvider ServiceProvider { get; private set; }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

    public override void Initialize()
    {
        Logger.Sink = new AvaloniaSerilogSink();
        AvaloniaXamlLoader.Load(this);
    }

    public override void OnFrameworkInitializationCompleted()
    {
        var collection = new ServiceCollection();
        collection.ConfigureAppServices();
        collection.AddTransient<AppViewModel>();
        collection.AddTransient<AboutViewModel>();

        ServiceProvider = collection.BuildServiceProvider();

        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            desktop.ShutdownMode = ShutdownMode.OnExplicitShutdown;
            DataContext = ServiceProvider.GetRequiredService<AppViewModel>();
            var listener = RxApp.MainThreadScheduler.Schedule(StartStatusPoller);

            desktop.Exit += (_, _) =>
            {
                listener.Dispose();
            };
        }

        base.OnFrameworkInitializationCompleted();
    }

    private static async void StartStatusPoller()
    {
        var availabilityHandler = ServiceProvider.GetRequiredService<IAvailabilityHandler>();
        var statusPoller = ServiceProvider.GetRequiredService<IStatusPoller>();
        await statusPoller.Start(availabilityHandler.IsAvailable);
    }
}
