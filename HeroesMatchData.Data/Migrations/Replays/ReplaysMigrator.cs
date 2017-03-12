using HeroesMatchData.Data.Databases;
using HeroesMatchData.Data.Migrations;
using HeroesMatchData.Data.Migrations.Replays;

namespace HeroesMatchData.Data.Migration.Replays
{
    internal class ReplaysMigrator : MigratorBase<ReplaysContext, ReplaysContextMigrator>, IMigrator
    {
        public ReplaysMigrator(string dbName, bool databaseFileCreated, int requiredDatabaseVersion)
            : base(dbName, databaseFileCreated, requiredDatabaseVersion)
        {
        }
    }
}
