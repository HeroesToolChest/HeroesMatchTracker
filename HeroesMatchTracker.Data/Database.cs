using HeroesMatchTracker.Data.Migration.Replays;
using HeroesMatchTracker.Data.Migrations;
using HeroesMatchTracker.Data.Migrations.ReleaseNotes;
using HeroesMatchTracker.Data.Properties;
using HeroesMatchTracker.Data.Queries.Settings;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace HeroesMatchTracker.Data
{
    public class Database
    {
        private bool ReplaysDbFileCreated;
        private bool SettingsDbFileCreated;
        private bool ReleaseNotesDbFileCreated;

        private List<IMigrator> MigratorsList = new List<IMigrator>();

        private Database() { }

        public static string ApplicationPath => AppDomain.CurrentDomain.BaseDirectory;
        public static string DatabasePath => Path.Combine(Directory.GetParent(ApplicationPath.TrimEnd('\\')).FullName, Settings.Default.DatabaseFolderName);
        public static string ReleaseNotesDbFileName => Settings.Default.ReleaseNotesDbFileName;

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
            if (!Directory.Exists(DatabasePath))
                Directory.CreateDirectory(DatabasePath);

            VerifyDatabaseFiles();
            LegacyDatabaseCheck();
            SetMigrators();

            // set domain
            AppDomain.CurrentDomain.SetData("DataDirectory", DatabasePath);

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

        private void VerifyDatabaseFiles()
        {
            ReplaysDbFileCreated = !File.Exists(Path.Combine(DatabasePath, Settings.Default.ReplaysDbFileName));
            SettingsDbFileCreated = !File.Exists(Path.Combine(DatabasePath, Settings.Default.SettingsDbFileName));
            ReleaseNotesDbFileCreated = !File.Exists(Path.Combine(DatabasePath, Settings.Default.ReleaseNotesDbFileName));
        }

        private void LegacyDatabaseCheck()
        {
            // checking for v1.x.x of database
            if (File.Exists(Path.Combine(ApplicationPath, Settings.Default.OldDatabaseFolderName, Settings.Default.Version1DatabaseName)))
            {
                // check if all three databases were created, if so perform the upgrade migration
                if (ReleaseNotesDbFileCreated && SettingsDbFileCreated && ReleaseNotesDbFileCreated)
                {
                    DatabaseUpgradeMigration.UpgradeDatabaseVersion2(DatabasePath, ApplicationPath);
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
