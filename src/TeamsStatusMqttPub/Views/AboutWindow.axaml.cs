using Avalonia.Controls;
using Microsoft.Extensions.DependencyInjection;
using TeamsStatusMqttPub.ViewModels;

namespace TeamsStatusMqttPub.Views;

public partial class AboutWindow : Window
{
    public AboutWindow()
    {
        InitializeComponent();
        DataContext = App.ServiceProvider.GetRequiredService<AboutViewModel>();
    }
}
