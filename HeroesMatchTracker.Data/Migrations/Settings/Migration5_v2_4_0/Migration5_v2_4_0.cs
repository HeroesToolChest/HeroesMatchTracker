using System.Collections.Generic;

namespace HeroesMatchTracker.Data.Migrations.Settings
{
    internal class Migration5_v2_4_0 : IMigrationCommand
    {
        private int Version = 5;

        public void Command(Dictionary<int, List<string>> migrations, Dictionary<int, List<IMigrationAddon>> migrationAddons)
        {
            List<string> steps = new List<string>
            {
                @"INSERT INTO UserSettings(Name, Value) VALUES ('DataFolderLocation', '');",
            };
            migrations.Add(Version, steps);

            List<IMigrationAddon> addonSteps = new List<IMigrationAddon>();
            migrationAddons.Add(Version, addonSteps);
        }
    }
}
