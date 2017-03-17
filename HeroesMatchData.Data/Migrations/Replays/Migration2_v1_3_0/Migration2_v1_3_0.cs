using System.Collections.Generic;

namespace HeroesMatchData.Data.Migrations.Replays
{
    internal class Migration2_v1_3_0 : IMigrationCommand
    {
        private int Version = 2;

        public void Command(Dictionary<int, List<string>> migrations, Dictionary<int, List<IMigrationAddon>> migrationAddons)
        {
            List<string> steps = new List<string>();
            migrations.Add(Version, steps);

            List<IMigrationAddon> addonSteps = new List<IMigrationAddon>
            {
                new MigrationAddon2_v1_3_0_1(),
                new MigrationAddon2_v1_3_0_2(),
            };
            migrationAddons.Add(Version, addonSteps);
        }
    }
}
