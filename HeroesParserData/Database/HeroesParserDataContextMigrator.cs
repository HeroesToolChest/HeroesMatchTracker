using HeroesParserData.Database.Migrations;
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
            MigrationVersion2();
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
            addonSteps.Add(new MigrationAddon1_1_0_2());

            MigrationAddons.Add(1, addonSteps);
        }

        private void MigrationVersion2()
        {
            List<string> steps = new List<string>();
            steps.Add(@"CREATE TABLE IF NOT EXISTS UserSettings(
                        SettingId INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL,
                        Name NVARCHAR,
                        Value NVARCHAR)");

            Migrations.Add(2, steps);

            List<IMigrationAddon> addonSteps = new List<IMigrationAddon>();
            addonSteps.Add(new MigrationAddon1_2_0_1());
            addonSteps.Add(new MigrationAddon1_2_0_2());
            addonSteps.Add(new MigrationAddon1_2_0_3());

            MigrationAddons.Add(2, addonSteps);
        }
    }
}
