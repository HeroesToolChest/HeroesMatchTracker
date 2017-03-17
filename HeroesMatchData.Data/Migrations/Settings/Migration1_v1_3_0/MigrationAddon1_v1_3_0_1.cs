using HeroesMatchData.Data.Databases;
using HeroesMatchData.Data.Queries.Settings;

namespace HeroesMatchData.Data.Migrations.Settings
{
    internal class MigrationAddon1_v1_3_0_1 : MigrationMethods<SettingsContext>, IMigrationAddon
    {
        public void Execute()
        {
            new SettingsDb().UserSettings.SetDefaultSettings();
        }
    }
}
