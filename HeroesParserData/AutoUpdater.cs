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
        public string CurrentVersionString => CurrentVersion != null? $"{CurrentVersion.Major}.{CurrentVersion.Minor}.{CurrentVersion.Revision}" : string.Empty; 
        public Version LatestVersion { get; private set; }
        public string LatestVersionString => LatestVersion != null? $"{LatestVersion.Major}.{LatestVersion.Minor}.{LatestVersion.Revision}" : string.Empty;

        /// <summary>
        /// Checks for updates and applies the update if IsAutoUpdates in settings is true. 
        /// Returns true if update is applied.
        /// </summary>
        /// <returns></returns>
        public async Task<bool> AutoUpdate()
        {
            try
            {
                await CheckForUpdates();

                if (UpdateInfo != null)
                {
                    if (Settings.Default.IsAutoUpdates)
                    {
                        await ApplyReleases();
                        return true;
                    }
                    else
                    {
                        UpdaterLog.Log(LogLevel.Info, "Auto updates off. Not updating.");
                    }
                }
                else
                    UpdaterLog.Log(LogLevel.Info, "Have latest already. No update needed. ");

                return false;
            }
            catch (Exception ex)
            {
                UpdaterLog.Log(LogLevel.Info, ex.Message);
                UpdaterLog.Log(LogLevel.Info, ex.StackTrace);
                throw;
            }
        }

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

                    UpdaterLog.Log(LogLevel.Info, "Applying releases");
                    string directoryPath = await UpdateManager.ApplyReleases(UpdateInfo);

                    App.NewLatestDirectory = directoryPath;

                    UpdaterLog.Log(LogLevel.Info, $"New directory path: {directoryPath}");

                    return true;
                }
            }
            catch (Exception ex)
            {
                throw new AutoUpdaterException("Error applying releases", ex);
            }
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
