using HeroesStatTracker.Data.Migration.Replays;
using HeroesStatTracker.Data.Migrations;
using HeroesStatTracker.Data.Queries.Settings;
using System;
using System.Collections.Generic;
using System.IO;

namespace HeroesStatTracker.Data
{
    public class Database
    {
        private bool ReplaysDbFileCreated;
        private bool SettingsDbFileCreated;

        private List<IMigrator> MigratorsList = new List<IMigrator>();

        private Database() { }

        public static Database Initialize()
        {
            return new Database();
        }

        /// <summary>
        /// Verifies database all database files and performs migrations if needed
        /// </summary>
        public void ExecuteDatabaseMigrations()
        {
            // check for the database folder
            string applicationPath = AppDomain.CurrentDomain.BaseDirectory;
            string databasePath = Path.Combine(applicationPath, Properties.Settings.Default.DatabaseFolderName);

            if (!Directory.Exists(databasePath))
                Directory.CreateDirectory(databasePath);

            // set the domain
            AppDomain.CurrentDomain.SetData("DataDirectory", Directory.GetCurrentDirectory());

            VerifyDatabaseFiles();
            SetMigrators();

            foreach (var migrator in MigratorsList)
            {
                migrator.Initialize(true);
            }

            InitialDatabaseExecutions();
        }

        private void SetMigrators()
        {
            MigratorsList.Add(new ReplaysMigrator(Properties.Settings.Default.ReplaysDbFileName, ReplaysDbFileCreated, Properties.Settings.Default.ReplaysDatabaseMigrationVersion));
            MigratorsList.Add(new SettingsMigrator(Properties.Settings.Default.SettingsDbFileName, SettingsDbFileCreated, Properties.Settings.Default.SettingsDatabaseMigrationVersion));
        }

        private void VerifyDatabaseFiles()
        {
            if (!File.Exists(Path.Combine(Properties.Settings.Default.DatabaseFolderName, Properties.Settings.Default.ReplaysDbFileName)))
                ReplaysDbFileCreated = true;
            else
                ReplaysDbFileCreated = false;

            if (!File.Exists(Path.Combine(Properties.Settings.Default.DatabaseFolderName, Properties.Settings.Default.SettingsDbFileName)))
                SettingsDbFileCreated = true;
            else
                SettingsDbFileCreated = false;
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
