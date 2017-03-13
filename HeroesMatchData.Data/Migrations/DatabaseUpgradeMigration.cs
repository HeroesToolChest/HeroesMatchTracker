using System;
using System.Configuration;
using System.Data.SQLite;
using System.IO;

namespace HeroesMatchData.Data.Migrations
{
    internal static class DatabaseUpgradeMigration
    {
        private static readonly string MigrationLogFile = $"Logs/{Properties.Settings.Default.MigrationLogFile}";

        internal static void UpgradeDatabaseVersion2()
        {
            try
            {
                MigrationLogger("Executing database upgrade migration...");

                // delete the Replays.sqlite file
                string replaysSqlite = Path.Combine(Properties.Settings.Default.DatabaseFolderName, Properties.Settings.Default.ReplaysDbFileName);
                if (File.Exists(replaysSqlite))
                {
                    File.Delete(replaysSqlite);
                }

                MigrationLogger($"Deleted {replaysSqlite}");

                using (var conn = new SQLiteConnection(ConfigurationManager.ConnectionStrings[Properties.Settings.Default.OldHeroesParserDatabaseConnName].ConnectionString))
                {
                    using (var cmd = new SQLiteCommand("DROP TABLE ReleaseNotes; DROP TABLE UserSettings;", conn))
                    {
                        conn.Open();
                        cmd.ExecuteNonQuery();
                    }

                    MigrationLogger("Dropped table ReleaseNotes and UserSettings");
                }

                File.Move(Path.Combine(Properties.Settings.Default.DatabaseFolderName, Properties.Settings.Default.Version1DatabaseName), Path.Combine(Properties.Settings.Default.DatabaseFolderName, Properties.Settings.Default.ReplaysDbFileName));
                MigrationLogger("HeroesParserData.db renamed to Replays.sqlite");
                MigrationLogger("Upgrade migration completed");
            }
            catch (Exception)
            {
                MigrationLogger($"[ERROR] An error has occured during the upgrade migration");
                throw;
            }
        }

        internal static void MigrationLogger(string message)
        {
            using (StreamWriter writer = new StreamWriter(MigrationLogFile, true))
            {
                writer.WriteLine($"[{DateTime.Now.ToLocalTime()}] [Upgrade Migration] {message}");
            }
        }
    }
}
