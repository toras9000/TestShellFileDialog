using System;
using System.Threading;
using System.Threading.Tasks;
using TestShellFileDialogCore.ViewModels;
using TestShellFileDialogCore.Views;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace TestShellFileDialogCore;

public class AppHost(IServiceProvider services) : IHostedService
{
    public Task StartAsync(CancellationToken cancellationToken)
    {
        var mainWindow = services.GetRequiredService<MainWindow>();
        mainWindow.Show();

        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
}

public static class AppHostServices
{
    extension(IServiceCollection services)
    {
        public IServiceCollection AddHostServices()
        {
            services.AddTransient<MainWindow>();
            services.AddTransient<MainWindowViewModel>();

            return services;
        }
    }
}