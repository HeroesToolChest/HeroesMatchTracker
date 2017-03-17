using System.Collections.Generic;

namespace HeroesMatchData.Data.Migrations.Replays
{
    internal class Migration3_v1_4_0 : IMigrationCommand
    {
        private int Version = 3;
        public void Command(Dictionary<int, List<string>> migrations, Dictionary<int, List<IMigrationAddon>> migrationAddons)
        {
            List<string> steps = new List<string>
            {
                @"CREATE TABLE IF NOT EXISTS ReplayHotsLogsUploads(
                ReplaysHotsLogsUploadId INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL,
                ReplayId INTEGER,
                ReplayFileTimeStamp DATETIME,
                Status INTEGER,
                FOREIGN KEY (ReplayId) REFERENCES Replays (ReplayId))",
            };
            migrations.Add(Version, steps);

            List<IMigrationAddon> addonSteps = new List<IMigrationAddon>();
            migrationAddons.Add(Version, addonSteps);
        }
    }
}
