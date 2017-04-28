using System.Collections.Generic;

namespace HeroesMatchTracker.Data.Migrations.Settings
{
    internal class Migration2_v2_0_0 : IMigrationCommand
    {
        private int Version = 2;

        public void Command(Dictionary<int, List<string>> migrations, Dictionary<int, List<IMigrationAddon>> migrationAddons)
        {
            List<string> steps = new List<string>
            {
                @"INSERT INTO UserSettings(Name, Value) VALUES ('IsWindowsStartup', 'False')",
                @"INSERT INTO UserSettings(Name, Value) VALUES ('IsStartedViaStartup', 'False')",
            };
            migrations.Add(Version, steps);

            List<IMigrationAddon> addonSteps = new List<IMigrationAddon>();
            migrationAddons.Add(Version, addonSteps);
        }
    }
}
