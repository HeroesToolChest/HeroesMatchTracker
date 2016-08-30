using System.Collections.Generic;

namespace HeroesParserData.Database
{
    public class HeroesParserDataContextMigrator
    {
        public Dictionary<int, List<string>> Migrations { get; set; } = new Dictionary<int, List<string>>();

        public HeroesParserDataContextMigrator()
        {
            Migrations = new Dictionary<int, List<string>>();

            // add call to MigrationVersionX() here
            MigrationVersion1();
        }

        // Add new migration versions here
        // Each one will be a new method, MigrationVersion1, MigrationVersion2, MigrationVersion3, etc....

        private void MigrationVersion1()
        {
            List<string> steps = new List<string>();

            steps.Add(@"CREATE TABLE ReplayAllHotsPlayerHero(
                        PlayerHeroesId INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL,
                        ReplayId INTEGER,
                        PlayerId INTEGER,
                        HeroName NVARCHAR (50),
                        IsUsable BOOLEAN,
                        FOREIGN KEY (ReplayId) REFERENCES Replays(ReplayId),
                        FOREIGN KEY (PlayerId) REFERENCES ReplayAllHotsPlayers (PlayerId))");

            Migrations.Add(1, steps);
        }
    }
}
