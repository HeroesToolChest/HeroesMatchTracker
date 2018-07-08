using System.Collections.Generic;

namespace HeroesMatchTracker.Data.Migrations.Replays
{
    internal class Migration10_v2_9_0 : IMigrationCommand
    {
        private int Version = 10;

        public void Command(Dictionary<int, List<string>> migrations, Dictionary<int, List<IMigrationAddon>> migrationAddons)
        {
            List<string> steps = new List<string>
            {
            };
            migrations.Add(Version, steps);

            List<IMigrationAddon> addonSteps = new List<IMigrationAddon>
            {
                new MigrationAddon9_v2_7_0_1(),
                new MigrationAddon9_v2_7_0_2(),
                new MigrationAddon9_v2_7_0_3(),
            };
            migrationAddons.Add(Version, addonSteps);
        }
    }
}
