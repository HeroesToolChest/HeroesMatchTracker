using System.Collections.Generic;

namespace HeroesMatchData.Data.Migrations.Replays
{
    internal class Migration1_v1_2_0 : IMigrationCommand
    {
        private int Version = 1;

        public void Command(Dictionary<int, List<string>> migrations, Dictionary<int, List<IMigrationAddon>> migrationAddons)
        {
            List<string> steps = new List<string>
            {
                @"CREATE TABLE IF NOT EXISTS ReplayRenamedPlayers(                
                RenamedPlayerId INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL,
                PlayerId INTEGER,
                BattleTagName NVARCHAR (50),
                BattleNetId INTEGER,
                BattleNetRegionId INTEGER,
                BattleNetSubId INTEGER,
                DateAdded DATETIME,
                FOREIGN KEY (PlayerId) REFERENCES ReplayAllHotsPlayers (PlayerId))",
            };
            migrations.Add(Version, steps);

            List<IMigrationAddon> addonSteps = new List<IMigrationAddon>
            {
                new MigrationAddon1_v1_2_0_1(),
                new MigrationAddon1_v1_2_0_2(),
            };
            migrationAddons.Add(Version, addonSteps);
        }
    }
}
