using System.Collections.Generic;

namespace HeroesMatchTracker.Data.Migrations.Replays
{
    internal class Migration8_v2_4_0 : IMigrationCommand
    {
        private int Version = 8;

        public void Command(Dictionary<int, List<string>> migrations, Dictionary<int, List<IMigrationAddon>> migrationAddons)
        {
            List<string> steps = new List<string>
            {
                @"CREATE TABLE IF NOT EXISTS ReplayHotsApiUploads(
                ReplaysHotsApiUploadId INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL,
                ReplayId INTEGER UNIQUE,
                ReplayFileTimeStamp DATETIME,
                Status INTEGER,
                FOREIGN KEY (ReplayId) REFERENCES Replays (ReplayId))",
            };
            migrations.Add(Version, steps);

            List<IMigrationAddon> addonSteps = new List<IMigrationAddon>
            {
                new MigrationAddon8_v2_4_0_1(),
            };
            migrationAddons.Add(Version, addonSteps);
        }
    }
}
