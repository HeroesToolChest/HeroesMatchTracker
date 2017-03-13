using System.Collections.Generic;

namespace HeroesMatchData.Data.Migrations.Replays
{
    internal class Migration4_v2_0_0 : IMigrationCommand
    {
        private int Version = 4;

        internal Migration4_v2_0_0() { }

        public void Command(Dictionary<int, List<string>> migrations, Dictionary<int, List<IMigrationAddon>> migrationAddons)
        {
            // intentionally left emtpy, increasing the version to 4 to match HeroesParserData.db version
            List<string> steps = new List<string>();
            migrations.Add(Version, steps);

            List<IMigrationAddon> addonSteps = new List<IMigrationAddon>();
            migrationAddons.Add(Version, addonSteps);
        }
    }
}
