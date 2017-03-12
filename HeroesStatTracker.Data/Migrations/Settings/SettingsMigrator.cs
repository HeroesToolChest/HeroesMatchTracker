using HeroesMatchData.Data.Databases;
using HeroesMatchData.Data.Migrations;
using HeroesMatchData.Data.Migrations.Settings;

namespace HeroesMatchData.Data.Migration.Replays
{
    internal class SettingsMigrator : MigratorBase<SettingsContext, SettingsContextMigrator>, IMigrator
    {
        public SettingsMigrator(string dbName, bool databaseExists, int requiredDatabaseVersion)
            : base(dbName, databaseExists, requiredDatabaseVersion)
        {
        }
    }
}
