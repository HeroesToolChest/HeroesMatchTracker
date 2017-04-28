using HeroesMatchTracker.Data.Databases;
using HeroesMatchTracker.Data.Migrations;
using HeroesMatchTracker.Data.Migrations.Replays;

namespace HeroesMatchTracker.Data.Migration.Replays
{
    internal class ReplaysMigrator : MigratorBase<ReplaysContext, ReplaysContextMigrator>, IMigrator
    {
        public ReplaysMigrator(string dbName, bool databaseFileCreated, int requiredDatabaseVersion)
            : base(dbName, databaseFileCreated, requiredDatabaseVersion)
        { }
    }
}
