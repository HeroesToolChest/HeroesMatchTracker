using HeroesStatTracker.Data.Databases;
using HeroesStatTracker.Data.Migrations;
using HeroesStatTracker.Data.Migrations.Replays;

namespace HeroesStatTracker.Data.Migration.Replays
{
    internal class ReplaysMigrator : MigratorBase<ReplaysContext, ReplaysContextMigrator>, IMigrator
    {
        public ReplaysMigrator(string dbName, bool databaseFileCreated, int requiredDatabaseVersion)
            : base(dbName, databaseFileCreated, requiredDatabaseVersion)
        {
        }
    }
}
