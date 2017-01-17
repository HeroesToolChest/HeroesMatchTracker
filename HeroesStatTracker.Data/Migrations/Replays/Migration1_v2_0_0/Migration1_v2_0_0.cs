using System.Collections.Generic;

namespace HeroesStatTracker.Data.Migrations.Replays
{
    internal class Migration1_v2_0_0 : IMigrationCommand
    {
        private Migration1_v2_0_0() { }

        public void Command(Dictionary<int, List<string>> migrations, Dictionary<int, List<IMigrationAddon>> migrationAddons)
        {
            /* List<string> steps = new List<string>();
            steps.Add(@"CREATE TABLE IF NOT EXISTS ReplayRenamedPlayers(
                        RenamedPlayerId INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL,
                        PlayerId INTEGER,
                        BattleTagName NVARCHAR (50),
                        BattleNetId INTEGER,
                       BattleNetRegionId INTEGER,
                        BattleNetSubId INTEGER,
                       DateAdded DATETIME,
                        FOREIGN KEY (PlayerId) REFERENCES ReplayAllHotsPlayers (PlayerId))");

            migrations.Add(1, steps);

            List<IMigrationAddon> addonSteps = new List<IMigrationAddon>();
            addonSteps.Add(new MigrationAddon1_v2_0_0_1());

            migrationAddons.Add(1, addonSteps); */
        }
    }
}
