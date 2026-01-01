using System.Windows;
using CommunityToolkit.Mvvm.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace TestShellFileDialogCore;

/// <summary>
/// Interaction logic for App.xaml
/// </summary>
public partial class App
{
    public App()
    {
        var builder = Host.CreateApplicationBuilder();
        builder.Services.AddHostedService<AppHost>();
        builder.Services.AddHostServices();

        this.AppHost = builder.Build();

        Ioc.Default.ConfigureServices(this.AppHost.Services);
    }

    public IHost AppHost { get; }

    protected override void OnStartup(StartupEventArgs e)
    {
        base.OnStartup(e);

        _ = this.AppHost.StartAsync();
    }
}
