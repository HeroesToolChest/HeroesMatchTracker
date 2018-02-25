using System.Collections.Generic;

namespace HeroesMatchTracker.Data.Migrations.Replays
{
    internal class Migration9_v2_6_7 : IMigrationCommand
    {
        private int Version = 9;

        public void Command(Dictionary<int, List<string>> migrations, Dictionary<int, List<IMigrationAddon>> migrationAddons)
        {
            List<string> steps = new List<string>
            {
            };
            migrations.Add(Version, steps);

            List<IMigrationAddon> addonSteps = new List<IMigrationAddon>
            {
                new MigrationAddon8_v2_6_7_1(),
                new MigrationAddon8_v2_6_7_2(),
            };
            migrationAddons.Add(Version, addonSteps);
        }
    }
}
