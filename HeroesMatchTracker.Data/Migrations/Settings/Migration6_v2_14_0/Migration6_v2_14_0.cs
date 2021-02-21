using System.Collections.Generic;

namespace HeroesMatchTracker.Data.Migrations.Settings
{
    internal class Migration6_v2_14_0 : IMigrationCommand
    {
        private int Version = 6;

        public void Command(Dictionary<int, List<string>> migrations, Dictionary<int, List<IMigrationAddon>> migrationAddons)
        {
            List<string> steps = new List<string>
            {
                @"INSERT INTO UserSettings(Name, Value) VALUES ('IsHeroesProfileUploaderEnabled', 'False');",
                @"INSERT INTO UserSettings(Name, Value) VALUES ('ReplaysLatestHeroesProfile', '2010-01-01 12:00:00');",
                @"INSERT INTO UserSettings(Name, Value) VALUES ('ReplaysLastHeroesProfile', '2010-01-01 12:00:00');",
            };
            migrations.Add(Version, steps);

            List<IMigrationAddon> addonSteps = new List<IMigrationAddon>();
            migrationAddons.Add(Version, addonSteps);
        }
    }
}
