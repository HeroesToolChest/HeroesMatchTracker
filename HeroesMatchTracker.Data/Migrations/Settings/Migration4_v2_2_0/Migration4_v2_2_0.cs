using System.Collections.Generic;

namespace HeroesMatchTracker.Data.Migrations.Settings
{
    internal class Migration4_v2_2_0 : IMigrationCommand
    {
        private int Version = 4;

        public void Command(Dictionary<int, List<string>> migrations, Dictionary<int, List<IMigrationAddon>> migrationAddons)
        {
            List<string> steps = new List<string>
            {
                @"CREATE TABLE IF NOT EXISTS FailedReplays(                
                FailedReplayId INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL,
                TimeStamp DATETIME,
                Build INTEGER,
                Status NVARCHAR(20),
                FilePath TEXT);",

                @"CREATE TABLE IF NOT EXISTS UserProfiles(
                UserProfileId INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL,
                UserBattleTagName NVARCHAR(40),
                UserRegion INTEGER,
                UserPlayerId INTEGER);",

                @"INSERT INTO UserSettings(Name, Value) VALUES ('ShowToasterUpdateNotification', 'True');",
                @"INSERT INTO UserSettings(Name, Value) VALUES ('IsUpdateAvailableKnown', 'False');",
                @"INSERT INTO UserSettings(Name, Value) VALUES ('IsAutoRequeueOnUpdate', 'True');",
                @"INSERT INTO UserSettings(Name, Value) VALUES ('ShowWhatsNewWindow', 'False');",
                @"INSERT INTO UserSettings(Name, Value) VALUES ('RequeueAllFailedReplays', 'False');",
            };
            migrations.Add(Version, steps);

            List<IMigrationAddon> addonSteps = new List<IMigrationAddon>();
            migrationAddons.Add(Version, addonSteps);
        }
    }
}
