using HeroesMatchData.Data.Databases;
using System;
using System.Configuration;
using System.Data;
using System.Data.SQLite;
using System.IO;

namespace HeroesMatchData.Data.Migrations
{
    internal static class DatabaseUpgradeMigration
    {
        private static readonly string MigrationLogFile = $"Logs/{Properties.Settings.Default.MigrationLogFile}";

        internal static void UpgradeDatabaseVersion2()
        {
            var releaseNotesTable = new DataTable();
            try
            {
                MigrationLogger("Executing database upgrade migration...");

                // delete the Replays.sqlite file
                string replaysSqlite = Path.Combine(Properties.Settings.Default.DatabaseFolderName, Properties.Settings.Default.ReplaysDbFileName);
                if (File.Exists(replaysSqlite))
                    File.Delete(replaysSqlite);

                MigrationLogger($"Deleted {replaysSqlite}");

                using (var conn = new SQLiteConnection(ConfigurationManager.ConnectionStrings[Properties.Settings.Default.OldHeroesParserDatabaseConnName].ConnectionString))
                {
                    conn.Open();
                    using (var transaction = conn.BeginTransaction())
                    {
                        // get the release notes from HeroesParserData
                        using (var cmd = new SQLiteCommand("SELECT * FROM ReleaseNotes", conn))
                        {
                            using (SQLiteDataAdapter adapter = new SQLiteDataAdapter(cmd))
                            {
                                adapter.Fill(releaseNotesTable);
                            }
                        }

                        using (var cmd = new SQLiteCommand("DROP TABLE ReleaseNotes; DROP TABLE UserSettings;", conn))
                        {
                            cmd.ExecuteNonQuery();
                        }

                        MigrationLogger("Dropped table ReleaseNotes and UserSettings");
                        transaction.Commit();
                    }
                }

                using (var db = new ReleaseNotesContext())
                {
                    foreach (DataRow row in releaseNotesTable.Rows)
                    {
                        db.Database.ExecuteSqlCommand(
                            "INSERT INTO ReleaseNotes VALUES (@Version, @PreRelease, @DateReleased, @PatchNote);",
                            new SQLiteParameter("@Version", row["Version"]),
                            new SQLiteParameter("@PreRelease", row["PreRelease"]),
                            new SQLiteParameter("@DateReleased", row["DateReleased"]),
                            new SQLiteParameter("@PatchNote", row["PatchNote"]));
                    }

                    MigrationLogger("Added ReleaseNotes to ReleaseNotes.sqlite");
                }

                // rename HeroesParserData.db to Replays.sqlite
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
