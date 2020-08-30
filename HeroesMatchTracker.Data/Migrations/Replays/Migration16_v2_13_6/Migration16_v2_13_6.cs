using System.Collections.Generic;

namespace HeroesMatchTracker.Data.Migrations.Replays
{
    internal class Migration16_v2_13_6 : IMigrationCommand
    {
        private int Version = 16;

        public void Command(Dictionary<int, List<string>> migrations, Dictionary<int, List<IMigrationAddon>> migrationAddons)
        {
            List<string> steps = new List<string>
            {
            };

            migrations.Add(Version, steps);

            List<IMigrationAddon> addonSteps = new List<IMigrationAddon>
            {
                new MigrationAddon16_v2_13_6_1(),
            };
            migrationAddons.Add(Version, addonSteps);
        }
    }
}
