using HeroesParserData.Properties;
using NLog;
using Squirrel;
using System;
using System.IO;
using System.Threading.Tasks;

namespace HeroesParserData
{
    public class AutoUpdater
    {
        private UpdateManager UpdateManager;
        private UpdateInfo UpdateInfo;
        private static Logger UpdaterLog = LogManager.GetLogger("UpdateLogFile");

        public Version CurrentVersion { get; private set; }
        public string CurrentVersionString => CurrentVersion != null? $"{CurrentVersion.Major}.{CurrentVersion.Minor}.{CurrentVersion.Build}" : string.Empty; 
        public Version LatestVersion { get; private set; }
        public string LatestVersionString => LatestVersion != null? $"{LatestVersion.Major}.{LatestVersion.Minor}.{LatestVersion.Build}" : string.Empty;

        /// <summary>
        /// Checks for updates, sets property UpdateInfo to null if no updates found. Returns true is update is available.
        /// </summary>
        /// <returns></returns>
        public async Task<bool> CheckForUpdates()
        {
            try
            {
                using (UpdateManager = await UpdateManager.GitHubUpdateManager(Settings.Default.UpdateUrl))
                {
                    UpdaterLog.Log(LogLevel.Info, "Update Check");

                    UpdateInfo update = await UpdateManager.CheckForUpdate();

                    CurrentVersion = update.CurrentlyInstalledVersion != null ? update.CurrentlyInstalledVersion.Version.Version : null;
                    LatestVersion = update.FutureReleaseEntry != null ? update.FutureReleaseEntry.Version.Version : null;

                    UpdaterLog.Log(LogLevel.Info, $"Current Version: {CurrentVersionString}");
                    UpdaterLog.Log(LogLevel.Info, $"Latest Version: {LatestVersionString}");

                    if (CurrentVersion != null && CurrentVersion < LatestVersion)
                    {
                        // prevent updating to version 2
                        if (LatestVersion.Major >= 2 || (LatestVersion.Major == 1 && LatestVersion.Minor >= 100))
                        {
                            UpdaterLog.Log(LogLevel.Info, "The current version cannot auto update to version 2");
                            UpdaterLog.Log(LogLevel.Info, "Please download version 2 at https://github.com/koliva8245/HeroesMatchTracker");
                            UpdateInfo = null;
                            return false;
                        }

                        UpdateInfo = update;
                        return true;
                    }
                    else
                    {
                        UpdateInfo = null;
                        return false;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new AutoUpdaterException("Error checking for updates", ex);
            }

            // these two lines of code are for debugging/testing the update process
            //using (UpdateManager = new UpdateManager(@"C:\Users\koliva\Documents\Visual Studio 2015\Projects\GitHub\UpdateTest\UpdateTestRelease"))
            //CurrentVersion = new Version("1.3.0");
        }

        /// <summary>
        /// Performs the update by downloading and applying the releases. Returns true if releases were applied.
        /// </summary>
        /// <param name="update">The update object returned by CheckUpdates()</param>
        /// <returns></returns>
        public async Task<bool> ApplyReleases()
        {
            try
            {
                if (UpdateInfo == null)
                    return false;

                using (UpdateManager)
                {
                    UpdaterLog.Log(LogLevel.Info, "Downloading...");
                    await UpdateManager.DownloadReleases(UpdateInfo.ReleasesToApply);
                    UpdaterLog.Log(LogLevel.Info, "Downloading...Completed");

                    // apply the releases
                    UpdaterLog.Log(LogLevel.Info, "Applying releases");
                    string directoryPath = await UpdateManager.ApplyReleases(UpdateInfo);

                    App.NewLatestDirectory = directoryPath;

                    UpdaterLog.Log(LogLevel.Info, $"New directory path: {directoryPath}");

                    UserSettings.Default.IsNewUpdateApplied = true;

                    return true;
                }
            }
            catch (Exception ex)
            {
                throw new AutoUpdaterException("Error applying releases", ex);
            }
        }

        public static async Task RetrieveReleaseNotes()
        {
            try
            {
                ReleaseNoteHandler releaseNoteHandler = new ReleaseNoteHandler();
                await releaseNoteHandler.InitializeClient();

                // save the needed release notes
                releaseNoteHandler.AddApplyReleasesReleaseNotes();
            }
            catch (Exception ex)
            {
                UpdaterLog.Log(LogLevel.Info, ex);
            }
        }

        public void RestartApp()
        {
            UpdateManager.RestartApp();
        }

        public static void CopyDatabaseToLatestRelease()
        {
            string dbFile = Settings.Default.DatabaseFile;
            string dbFilePath = $@"Database\{dbFile}";
            string newAppDirectory = Path.Combine(App.NewLatestDirectory, "Database");
            string rootDirectory = Directory.GetParent(App.NewLatestDirectory).FullName;
            string backupDirectory = Path.Combine(rootDirectory, "BackupDatabases");

            try
            {
                if (!File.Exists(dbFilePath))
                {
                    UpdaterLog.Log(LogLevel.Info, $"Database file not found: {dbFilePath}");
                    UpdaterLog.Log(LogLevel.Info, "Nothing to copy");

                    return;
                }

                Directory.CreateDirectory(newAppDirectory);
                UpdaterLog.Log(LogLevel.Info, $"Directory created: {newAppDirectory}");

                File.Copy(dbFilePath, Path.Combine(newAppDirectory, dbFile));

                UpdaterLog.Log(LogLevel.Info, $"Database file copied to: {Path.Combine(newAppDirectory, dbFile)}");

                if (!Directory.Exists(backupDirectory))
                    Directory.CreateDirectory(backupDirectory);

                File.Copy(dbFilePath, Path.Combine(backupDirectory, dbFile), true);
                UpdaterLog.Log(LogLevel.Info, $"Database file backup copied to: {Path.Combine(backupDirectory, dbFile)}");
            }
            catch (Exception ex)
            {
                UpdaterLog.Log(LogLevel.Info, ex);
                throw;
            }
        }
    }

    
    [Serializable]
    public class AutoUpdaterException : Exception
    {
        public AutoUpdaterException(string message, Exception ex)
            :base(message, ex)
        {

        }

        public AutoUpdaterException(string message)
            : base(message)
        {

        }
    }
}
