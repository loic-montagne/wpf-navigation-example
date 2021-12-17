using Microsoft.Extensions.DependencyInjection;
using System.Windows;

namespace NavigationExample
{
    /// <summary>
    /// Logique d'interaction pour App.xaml
    /// </summary>
    public partial class App : Application
    {
        public ServiceProvider ServiceProvider { get; }

        public App()
        {
            ServiceCollection services = new ServiceCollection();
            ConfigureServices(services);
            ServiceProvider = services.BuildServiceProvider();
        }

        private void ConfigureServices(ServiceCollection services)
        {
            services.AddSingleton(s => new Services.NavigationService(s));
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            ServiceProvider.GetService<Services.NavigationService>()
                           .Show<Views.MainWindow, ViewModels.MainViewModel>();
        }

    }
}
