using System.Collections.Generic;

namespace HeroesMatchTracker.Data.Migrations.Replays
{
    internal class Migration5_v2_0_0 : IMigrationCommand
    {
        private int Version = 5;

        public void Command(Dictionary<int, List<string>> migrations, Dictionary<int, List<IMigrationAddon>> migrationAddons)
        {
            List<string> steps = new List<string>
            {
                @"UPDATE Replays
                SET GameMode = 512
                WHERE GameMode = 7;

                UPDATE Replays
                SET GameMode = 256
                WHERE GameMode = 6;

                UPDATE Replays
                SET GameMode = 128
                WHERE GameMode = 5;

                UPDATE Replays
                SET GameMode = 64
                WHERE GameMode = 4;

                UPDATE Replays
                SET GameMode = 32
                WHERE GameMode = 3;

                UPDATE Replays
                SET GameMode = 16
                WHERE GameMode = 2;

                UPDATE Replays
                SET GameMode = 2
                WHERE GameMode = -1;",
            };
            migrations.Add(Version, steps);

            List<IMigrationAddon> addonSteps = new List<IMigrationAddon>();
            migrationAddons.Add(Version, addonSteps);
        }
    }
}
