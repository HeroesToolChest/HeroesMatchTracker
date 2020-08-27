using HeroesMatchTracker.Core.Services.ReplayParser;
using HeroesMatchTracker.Core.Startup;
using HeroesMatchTracker.Infrastructure.Database;
using HeroesMatchTracker.Infrastructure.Database.Contexts;
using HeroesMatchTracker.Infrastructure.Database.HMT2Contexts;
using HeroesMatchTracker.Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using Serilog;
using Serilog.Events;
using Splat;
using Splat.Serilog;
using System.IO;

namespace HeroesMatchTracker
{
    public static class AppBootstrapper
    {
        public static void RegisterDependencies(bool debugLoggingEnabled)
        {
            RegisterLogger(debugLoggingEnabled);

            Locator.CurrentMutable.Register(() => new HeroesReplaysDbContext(new DbContextOptions<HeroesReplaysDbContext>()));
            Locator.CurrentMutable.Register(() => new HMT2ReplaysDbContext(new DbContextOptions<HMT2ReplaysDbContext>()));

            Locator.CurrentMutable.Register<ILoadStartup>(() => new LoadStartup());

            Locator.CurrentMutable.Register<IDatabaseInit>(() => new DatabaseInit(Locator.Current.GetService<HMT2ReplaysDbContext>(), Locator.Current.GetService<HeroesReplaysDbContext>()));
            //Locator.CurrentMutable.Register<IReplayDataRepository>(() => new ReplayDataRepository(Locator.Current.GetService<HeroesReplaysDbContext>()));

            Locator.CurrentMutable.Register<IReplayCollector>(() => new ReplayCollector());
        }

        private static void RegisterLogger(bool debugLoggingEnabled)
        {
            LogEventLevel loggingLevel = LogEventLevel.Information;

            if (debugLoggingEnabled)
                loggingLevel = LogEventLevel.Debug;

            Locator.CurrentMutable.UseSerilogFullLogger();

            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .WriteTo.Async(x => x.File(Path.Join("logs", "log-.txt"), rollingInterval: RollingInterval.Day, restrictedToMinimumLevel: loggingLevel), bufferSize: 500)
                .CreateLogger();
        }
    }
}
