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
                @"INSERT INTO UserSettings(Name, Value) VALUES ('IsHotsApiUploaderEnabled', 'False');",
                @"INSERT INTO UserSettings(Name, Value) VALUES ('ReplaysLatestHotsApi', '2010-01-01 12:00:00');",
                @"INSERT INTO UserSettings(Name, Value) VALUES ('ReplaysLastHotsApi', '2010-01-01 12:00:00');",
                @"INSERT INTO UserSettings(Name, Value) VALUES ('PreReleaseCheck', 'False');",
            };
            migrations.Add(Version, steps);

            List<IMigrationAddon> addonSteps = new List<IMigrationAddon>();
            migrationAddons.Add(Version, addonSteps);
        }
    }
}
