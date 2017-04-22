using HeroesMatchData.Data.Migration.Replays;
using HeroesMatchData.Data.Migrations;
using HeroesMatchData.Data.Migrations.ReleaseNotes;
using HeroesMatchData.Data.Properties;
using HeroesMatchData.Data.Queries.Settings;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace HeroesMatchData.Data
{
    public class Database
    {
        private bool ReplaysDbFileCreated;
        private bool SettingsDbFileCreated;
        private bool ReleaseNotesDbFileCreated;

        private List<IMigrator> MigratorsList = new List<IMigrator>();

        private Database() { }

        public static Database Initialize()
        {
            return new Database();
        }

        /// <summary>
        /// Verifies database all database files and performs migrations if needed
        /// </summary>
        public Task ExecuteDatabaseMigrations()
        {
            // check for the database folder
            string applicationPath = AppDomain.CurrentDomain.BaseDirectory;
            string databasePath = Path.Combine(Directory.GetParent(applicationPath.TrimEnd('\\')).FullName, Settings.Default.DatabaseFolderName);

            if (!Directory.Exists(databasePath))
                Directory.CreateDirectory(databasePath);

            VerifyDatabaseFiles(databasePath);
            LegacyDatabaseCheck(databasePath, applicationPath);
            SetMigrators();

            // set domain
            AppDomain.CurrentDomain.SetData("DataDirectory", databasePath);

            // perform migrations
            foreach (var migrator in MigratorsList)
            {
                migrator.Initialize(true);
            }

            InitialDatabaseExecutions();

            return Task.CompletedTask;
        }

        private void SetMigrators()
        {
            MigratorsList.Add(new ReplaysMigrator(Settings.Default.ReplaysDbFileName, ReplaysDbFileCreated, Settings.Default.ReplaysDatabaseMigrationVersion));
            MigratorsList.Add(new SettingsMigrator(Settings.Default.SettingsDbFileName, SettingsDbFileCreated, Settings.Default.SettingsDatabaseMigrationVersion));
            MigratorsList.Add(new ReleaseNotesMigrator(Settings.Default.ReleaseNotesDbFileName, ReleaseNotesDbFileCreated, Settings.Default.ReleaseNotesDatabaseMigrationVersion));
        }

        private void VerifyDatabaseFiles(string databasePath)
        {
            if (!File.Exists(Path.Combine(databasePath, Settings.Default.ReplaysDbFileName)))
                ReplaysDbFileCreated = true;
            else
                ReplaysDbFileCreated = false;

            if (!File.Exists(Path.Combine(databasePath, Settings.Default.SettingsDbFileName)))
                SettingsDbFileCreated = true;
            else
                SettingsDbFileCreated = false;

            if (!File.Exists(Path.Combine(databasePath, Settings.Default.ReleaseNotesDbFileName)))
                ReleaseNotesDbFileCreated = true;
            else
                ReleaseNotesDbFileCreated = false;
        }

        private void LegacyDatabaseCheck(string databasePath, string applicationPath)
        {
            // checking for v1.x.x of database
            if (File.Exists(Path.Combine(applicationPath, Settings.Default.OldDatabaseFolderName, Settings.Default.Version1DatabaseName)))
            {
                // check if all three databases were created, if so perform the upgrade migration
                if (ReleaseNotesDbFileCreated && SettingsDbFileCreated && ReleaseNotesDbFileCreated)
                {
                    DatabaseUpgradeMigration.UpgradeDatabaseVersion2(databasePath, applicationPath);
                    ReplaysDbFileCreated = false; // don't let it set the current version
                }
            }
        }

        private void InitialDatabaseExecutions()
        {
            if (SettingsDbFileCreated)
            {
                new SettingsDb().UserSettings.SetDefaultSettings();
            }
        }
    }
}
