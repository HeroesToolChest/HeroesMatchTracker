using System.Collections.Generic;

namespace HeroesMatchTracker.Data.Migrations.Replays
{
    internal class Migration12_v2_11_0 : IMigrationCommand
    {
        private int Version = 12;

        public void Command(Dictionary<int, List<string>> migrations, Dictionary<int, List<IMigrationAddon>> migrationAddons)
        {
            List<string> steps = new List<string>
            {
                @"CREATE TABLE IF NOT EXISTS ReplayMatchDraftPicks(
                DraftPickId INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL,
                ReplayId INTEGER,
                PlayerSlotId INTEGER,
                HeroSelected TEXT,
                PickType INTEGER,
                FOREIGN KEY (ReplayId) REFERENCES Replays (ReplayId))",
            };

            migrations.Add(Version, steps);

            List<IMigrationAddon> addonSteps = new List<IMigrationAddon>
            {
                new MigrationAddon12_v2_11_0_1(),
            };
            migrationAddons.Add(Version, addonSteps);
        }
    }
}
