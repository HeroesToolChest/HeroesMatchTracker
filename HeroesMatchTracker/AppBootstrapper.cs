using HeroesMatchTracker.Core.Database;
using HeroesMatchTracker.Core.Services.ReplayParser;
using HeroesMatchTracker.Core.Startup;
using HeroesMatchTracker.Infrastructure.Database;
using HeroesMatchTracker.Infrastructure.Database.Contexts;
using HeroesMatchTracker.Infrastructure.Database.HMT2Contexts;
using HeroesMatchTracker.Infrastructure.Database.Repository;
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

            RegisterRepositories();

            Locator.CurrentMutable.Register<ILoadStartup>(() => new LoadStartup());

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

        private static void RegisterRepositories()
        {
            // contexts
            Locator.CurrentMutable.Register(() => new HeroesReplaysDbContext());
            Locator.CurrentMutable.Register(() => new HMT2ReplaysDbContext());

            // factories
            Locator.CurrentMutable.Register(() => new DbContextFactory<HMT2ReplaysDbContext>(), typeof(IDbContextFactory<HMT2ReplaysDbContext>));
            Locator.CurrentMutable.Register(() => new DbContextFactory<HeroesReplaysDbContext>(), typeof(IDbContextFactory<HeroesReplaysDbContext>));
            Locator.CurrentMutable.Register(() => new HeroesReplaysRepositoryFactory(), typeof(IRepositoryFactory));

            // db init
            Locator.CurrentMutable.Register<IDatabaseInit>(() => new DatabaseInit(
                Locator.Current.GetService<IDbContextFactory<HMT2ReplaysDbContext>>(), 
                Locator.Current.GetService<IDbContextFactory<HeroesReplaysDbContext>>()));

            // repositories
            Locator.CurrentMutable.Register(() => new ReplayMatchRepository(Locator.Current.GetService<IDbContextFactory<HeroesReplaysDbContext>>()), typeof(IReplayMatchRepository));
        }
    }
}
