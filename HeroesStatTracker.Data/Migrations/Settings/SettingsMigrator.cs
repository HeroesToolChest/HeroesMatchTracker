using HeroesStatTracker.Data.Databases;
using HeroesStatTracker.Data.Migrations;
using HeroesStatTracker.Data.Migrations.Settings;

namespace HeroesStatTracker.Data.Migration.Replays
{
    internal class SettingsMigrator : MigratorBase<SettingsContext, SettingsContextMigrator>, IMigrator
    {
        public SettingsMigrator(string dbName, bool databaseExists, int requiredDatabaseVersion)
            : base(dbName, databaseExists, requiredDatabaseVersion)
        {
        }
    }
}
