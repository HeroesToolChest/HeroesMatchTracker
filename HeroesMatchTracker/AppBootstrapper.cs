using HeroesMatchTracker.Core.Database.HeroesReplays;
using HeroesMatchTracker.Core.Services;
using HeroesMatchTracker.Core.Services.ReplayParser;
using HeroesMatchTracker.Core.Startup;
using HeroesMatchTracker.Infrastructure;
using HeroesMatchTracker.Infrastructure.Database;
using HeroesMatchTracker.Infrastructure.Database.Contexts;
using HeroesMatchTracker.Infrastructure.Database.Contexts.HMT2Contexts;
using HeroesMatchTracker.Infrastructure.Database.Repository.HeroesReplays;
using HeroesMatchTracker.Views;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ReactiveUI;
using Serilog;
using Serilog.Events;
using Splat;
using Splat.Microsoft.Extensions.DependencyInjection;
using Splat.Serilog;
using System;
using System.IO;

namespace HeroesMatchTracker
{
    public static class AppBootstrapper
    {
        public static IServiceProvider Container { get; private set; } = null!;

        public static void Initialize(bool debugLoggingEnabled)
        {
            RegisterLogger(debugLoggingEnabled);

            var host = new HostBuilder()
                .ConfigureServices(services =>
                {
                    services.UseMicrosoftDependencyResolver();

                    IMutableDependencyResolver resolver = Locator.CurrentMutable;
                    resolver.InitializeSplat();
                    resolver.InitializeReactiveUI();

                    // Configure our local services and access the host configuration
                    ConfigureServices(services);
                })
                //.ConfigureLogging(loggingBuilder =>
                //{
                //    /*
                //    //remove loggers incompatible with UWP
                //    {
                //        var eventLoggers = loggingBuilder.Services
                //        .Where(l => l.ImplementationType == typeof(EventLogLoggerProvider))
                //        .ToList();

                //        foreach (var el in eventLoggers)
                //        loggingBuilder.Services.Remove(el);
                //    }
                //    */

                //    loggingBuilder.AddSplat();
                //})
                .UseEnvironment(Environments.Development)
                .Build();

            // Since MS DI container is a different type,
            // we need to re-register the built container with Splat again
            Container = host.Services;
        }

        //public static void RegisterDependencies(bool debugLoggingEnabled)
        //{
        //    RegisterLogger(debugLoggingEnabled);

        //    //RegisterRepositories();

        //    Locator.CurrentMutable.Register<ILoadStartup>(() => new LoadStartup());

        //    Locator.CurrentMutable.Register<IReplayCollector>(() => new ReplayCollector());
        //}

        private static void ConfigureServices(IServiceCollection services)
        {
            // db contexts
            services.AddDbContextFactory<HMT2ReplaysDbContext>(options =>
            {
                options
                    .UseLazyLoadingProxies()
                    .UseSqlite(HMT2ConnectionString.HMT2Replays);
            });
            services.AddDbContextFactory<HeroesReplaysDbContext>(options =>
            {
                options
                    .UseLazyLoadingProxies()
                    .UseSqlite(DbConnectionString.HeroesReplays);
            });

            services.AddScoped(x => x.GetRequiredService<IDbContextFactory<HMT2ReplaysDbContext>>().CreateDbContext());
            services.AddScoped(x => x.GetRequiredService<IDbContextFactory<HeroesReplaysDbContext>>().CreateDbContext());

            // startup
            services.AddTransient<ILoadStartup, LoadStartup>();
            services.AddTransient<IDatabaseInit, DatabaseInit>();
            services.AddTransient<IStartupWindow, StartupWindow>();

            // other services
            services.AddTransient<IReplayCollector, ReplayCollector>();
            services.AddTransient<IReplayParseData, ReplayParseData>();

            // repos
            services.AddTransient<IReplayMatchRepository, ReplayMatchRepository>();
            services.AddTransient<IReplayPlayerRepository, ReplayPlayerRepository>();
            services.AddTransient<IReplayPlayerToonRepository, ReplayPlayerToonRepository>();

            //services.AddSingleton<IStartupWindow, StartupWindow>();

            //services.AddSingleton<IViewFor<StartupWindowViewModel>, StartupWindow>();
            //services.AddSingleton<IStartupWindow, StartupWindow>();

            //services.AddSingleton<StartupWindowViewModel>();
            //services.AddSingleton(x => x.GetRequiredService<StartupWindowViewModel>());
            //services.AddSingleton<IViewFor<MainViewModel>, MainPage>();

            //Locator.CurrentMutable.RegisterViewsForViewModels(Assembly.GetCallingAssembly());

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

        //private static void RegisterRepositories()
        //{
        //    // contexts
        //    Locator.CurrentMutable.Register(() => new HeroesReplaysDbContext());
        //    Locator.CurrentMutable.Register(() => new HMT2ReplaysDbContext());

        //    // db init
        //    Locator.CurrentMutable.Register<IDatabaseInit>(() => new DatabaseInit(
        //        Locator.Current.GetService<IDbContextFactory<HMT2ReplaysDbContext>>(),
        //        Locator.Current.GetService<IDbContextFactory<HeroesReplaysDbContext>>()));

        //    // factories
        //    Locator.CurrentMutable.Register(() => new DbContextFactory<HMT2ReplaysDbContext>(), typeof(IDbContextFactory<HMT2ReplaysDbContext>));
        //    Locator.CurrentMutable.Register(() => new DbContextFactory<HeroesReplaysDbContext>(), typeof(IDbContextFactory<HeroesReplaysDbContext>));

        //    Locator.CurrentMutable.Register(() => new HeroesReplaysRepositoryFactory(), typeof(IHeroesReplaysRepositoryFactory));

        //    // repositories
        //    Locator.CurrentMutable.Register(() => new ReplayMatchRepository(Locator.Current.GetService<IDbContextFactory<HeroesReplaysDbContext>>()), typeof(IReplayMatchRepository));
        //    Locator.CurrentMutable.Register(() => new ReplayPlayerRepository(Locator.Current.GetService<IDbContextFactory<HeroesReplaysDbContext>>()), typeof(IReplayPlayerRepository));
        //    Locator.CurrentMutable.Register(() => new ReplayPlayerToonRepository(Locator.Current.GetService<IDbContextFactory<HeroesReplaysDbContext>>()), typeof(IReplayPlayerToonRepository));

        //    // services
        //    Locator.CurrentMutable.Register(() => new ParsedReplayService(Locator.Current.GetService<IHeroesReplaysRepositoryFactory>()), typeof(IParsedReplayService));
        //}
    }
}
