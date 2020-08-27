using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using HeroesMatchTracker.ViewModels;
using HeroesMatchTracker.Views;
using Serilog;
using System.Linq;

namespace HeroesMatchTracker
{
    public class App : Application
    {
        public override void Initialize()
        {
            AvaloniaXamlLoader.Load(this);
        }

        public override void OnFrameworkInitializationCompleted()
        {
            if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            {
                desktop.Startup += Desktop_Startup;
                desktop.Exit += Desktop_Exit;
            }

            base.OnFrameworkInitializationCompleted();
        }

        private void Desktop_Startup(object? sender, ControlledApplicationLifetimeStartupEventArgs e)
        {
            bool debugLoggingEnabled = false;

            if (e.Args != null && e.Args.Length > 0)
            {
                debugLoggingEnabled = e.Args.Contains("--debug");
#if DEBUG
                debugLoggingEnabled = true;
#endif
            }

            AppBootstrapper.RegisterDependencies(debugLoggingEnabled);

            ((IClassicDesktopStyleApplicationLifetime)ApplicationLifetime).MainWindow = new StartupWindow
            {
                DataContext = new StartupWindowViewModel(),
            };
        }

        private void Desktop_Exit(object? sender, ControlledApplicationLifetimeExitEventArgs e)
        {
            Log.CloseAndFlush();
        }
    }
}
