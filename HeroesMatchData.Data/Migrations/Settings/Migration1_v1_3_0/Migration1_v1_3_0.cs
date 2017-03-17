using System.Collections.Generic;

namespace HeroesMatchData.Data.Migrations.Settings
{
    internal class Migration1_v1_3_0 : IMigrationCommand
    {
        private int Version = 1;

        public void Command(Dictionary<int, List<string>> migrations, Dictionary<int, List<IMigrationAddon>> migrationAddons)
        {
            List<string> steps = new List<string>
            {
                @"CREATE TABLE IF NOT EXISTS UserSettings(
                SettingId INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL,
                Name NVARCHAR,
                Value NVARCHAR)",
            };
            migrations.Add(Version, steps);

            List<IMigrationAddon> addonSteps = new List<IMigrationAddon>
            {
                new MigrationAddon1_v1_3_0_1(),
            };
            migrationAddons.Add(Version, addonSteps);
        }
    }
}
