using System.Collections.Generic;

namespace HeroesMatchTracker.Data.Migrations.Replays
{
    internal class Migration6_v2_1_0 : IMigrationCommand
    {
        private int Version = 6;

        public void Command(Dictionary<int, List<string>> migrations, Dictionary<int, List<IMigrationAddon>> migrationAddons)
        {
            List<string> steps = new List<string>
            {
                @"UPDATE Replays
                SET GameMode = 2
                WHERE GameMode = 16;",
            };
            migrations.Add(Version, steps);

            List<IMigrationAddon> addonSteps = new List<IMigrationAddon>();
            migrationAddons.Add(Version, addonSteps);
        }
    }
}
