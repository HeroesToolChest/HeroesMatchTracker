using System.Collections.Generic;

namespace HeroesParserData.Database
{
    public class HeroesParserDataContextMigrator
    {
        public Dictionary<int, List<string>> Migrations { get; set; } = new Dictionary<int, List<string>>();
        public Dictionary<int, List<IMigrationAddon>> MigrationAddons { get; set; } = new Dictionary<int, List<IMigrationAddon>>();

        public HeroesParserDataContextMigrator()
        {
            // add call to MigrationVersionX() here
            MigrationVersion1();
        }

        // Add new migration versions here
        // Each one will be a new method, MigrationVersion1, MigrationVersion2, MigrationVersion3, etc....

        private void MigrationVersion1()
        {
            List<string> steps = new List<string>();
            steps.Add(@"CREATE TABLE IF NOT EXISTS ReplayRenamedPlayers(
                        RenamedPlayerId INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL,
                        PlayerId INTEGER,
                        BattleTagName NVARCHAR (50),
                        BattleNetId INTEGER,
                        BattleNetRegionId INTEGER,
                        BattleNetSubId INTEGER,
                        DateAdded DATETIME,
                        FOREIGN KEY (PlayerId) REFERENCES ReplayAllHotsPlayers (PlayerId))");

            Migrations.Add(1, steps);

            List<IMigrationAddon> addonSteps = new List<IMigrationAddon>();
            addonSteps.Add(new MigrationAddon1_1_0_1());

            MigrationAddons.Add(1, addonSteps);
        }
    }
}
