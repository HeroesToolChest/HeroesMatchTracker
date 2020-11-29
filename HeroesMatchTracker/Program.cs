using Avalonia;
using Avalonia.ReactiveUI;
using System.Linq;

namespace HeroesMatchTracker
{
    internal class Program
    {
        // Initialization code. Don't use any Avalonia, third-party APIs or any
        // SynchronizationContext-reliant code before AppMain is called: things aren't initialized
        // yet and stuff might break.
        public static void Main(string[] args)
        {
            bool debugLoggingEnabled = false;

            if (args != null && args.Length > 0)
            {
                debugLoggingEnabled = args.Contains("--debug");
#if DEBUG
                debugLoggingEnabled = true;
#endif
            }

            BuildAvaloniaApp(debugLoggingEnabled)
               .StartWithClassicDesktopLifetime(args);
        }

        // Avalonia configuration, don't remove; also used by visual designer.
        public static AppBuilder BuildAvaloniaApp(bool debugLoggingEnabled)
        {
            AppBootstrapper.Initialize(debugLoggingEnabled);

            AppBuilder appBuilder = AppBuilder.Configure<App>()
                .UsePlatformDetect()
                .LogToDebug()
                .UseReactiveUI();

            return appBuilder;
        }
    }
}
