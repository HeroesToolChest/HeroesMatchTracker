using HeroesMatchTracker.Core.Startup;
using HeroesMatchTracker.Infrastructure.Database.Contexts;
using HeroesMatchTracker.Infrastructure.Database.HMT2Contexts;
using Microsoft.EntityFrameworkCore;
using Splat;
using System;
using System.IO;

namespace HeroesMatchTracker.Infrastructure.Database
{
    public class DatabaseInit : IDatabaseInit, IEnableLogger
    {
        private const string _backupDirectory = "_backup";
        private const string _replaySqlite = "Replays.sqlite";

        private readonly IDbContextFactory<HMT2ReplaysDbContext> _hmt2ReplaysDbContextFactory;
        private readonly IDbContextFactory<HeroesReplaysDbContext> _heroesReplaysDbContextFactory;

        public DatabaseInit(
            IDbContextFactory<HMT2ReplaysDbContext> hmt2ReplaysDbContextFactory,
            IDbContextFactory<HeroesReplaysDbContext> heroesReplaysDbContextFactory)
        {
            _hmt2ReplaysDbContextFactory = hmt2ReplaysDbContextFactory;
            _heroesReplaysDbContextFactory = heroesReplaysDbContextFactory;
        }

        public void HMT2ReplayDbCheck()
        {
            using HMT2ReplaysDbContext hmt2ReplaysDbContext = _hmt2ReplaysDbContextFactory.CreateDbContext();

            if (File.Exists(_replaySqlite))
            {
                this.Log().Info($"Found existing {_replaySqlite} (HMT2) file.");
                this.Log().Info($"Creating backup of {_replaySqlite} (HMT2) file at {_backupDirectory} directory");

                try
                {
                    Directory.CreateDirectory(_backupDirectory);
                    File.Copy(_replaySqlite, Path.Join(_backupDirectory, $"{_replaySqlite}"));
                }
                catch (Exception ex)
                {
                    this.Log().Error(ex, $"Failed to create back up file. Update to HMT3 failed.");
                    return;
                }

                this.Log().Info($"Performing HMT3 update...");
                hmt2ReplaysDbContext.Database.ExecuteSqlRaw(@"
                    BEGIN;

                    CREATE TABLE __EFMigrationsHistory(
                    MigrationId TEXT PRIMARY KEY NOT NULL,
                    ProductVersion TEXT NOT NULL);

                    INSERT INTO __EFMigrationsHistory VALUES ('20200711043321_InitialCreate', '3.1.5');

                    COMMIT");

                this.Log().Info("Executed migration history successfully.");

                string newFileName = DbConnectionString.HeroesReplays.Substring(DbConnectionString.HeroesReplays.IndexOf('=', StringComparison.OrdinalIgnoreCase) + 1);

                try
                {
                    File.Move(_replaySqlite, newFileName);
                    this.Log().Info($"File renamed from {_replaySqlite} file to {newFileName}");
                }
                catch (Exception ex)
                {
                    this.Log().Error(ex, $"Unable to rename {_replaySqlite} file to {newFileName}");
                }
            }
        }

        public void InitHeroesReplaysDb()
        {
            using HeroesReplaysDbContext heroesReplaysDbContext = _heroesReplaysDbContextFactory.CreateDbContext();

            this.Log().Info($"Migrating {DbConnectionString.HeroesReplays} database...");
            heroesReplaysDbContext.Database.Migrate();
            this.Log().Info($"{DbConnectionString.HeroesReplays} migration completed.");
        }
    }
}
