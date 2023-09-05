using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using PathFinder.Factory;
using PathFinder.Factory.Implementation;
using PathFinder.Service.Abstract;
using PathFinder.Service.Implementation;
using System.Windows;

namespace PathFinder
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private readonly IHost _host;

        public App()
        {
            _host = Host.CreateDefaultBuilder()
                .ConfigureServices((context, services) =>
                {
                    ConfigureServices(services);
                })
                .Build();

        }

        private void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<MainWindow>();
            services.AddSingleton<IMapFactory, MapFactory>();
            services.AddSingleton<IPathFindingService, PathFindingService>();
        }

        protected override async void OnStartup(StartupEventArgs e)
        {
            await _host.StartAsync();

            var mainWindow = _host.Services.GetRequiredService<MainWindow>();
            MainWindow.Show();

            base.OnStartup(e);
        }

        protected override async void OnExit(ExitEventArgs e)
        {
            using (_host)
            {
                await _host.StopAsync();
            }

            base.OnExit(e);
        }

    }
}
