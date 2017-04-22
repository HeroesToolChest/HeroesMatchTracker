using HeroesMatchTracker.Data.Databases;
using HeroesMatchTracker.Data.Migrations;
using HeroesMatchTracker.Data.Migrations.Settings;

namespace HeroesMatchTracker.Data.Migration.Replays
{
    internal class SettingsMigrator : MigratorBase<SettingsContext, SettingsContextMigrator>, IMigrator
    {
        public SettingsMigrator(string dbName, bool databaseExists, int requiredDatabaseVersion)
            : base(dbName, databaseExists, requiredDatabaseVersion)
        {
        }
    }
}
