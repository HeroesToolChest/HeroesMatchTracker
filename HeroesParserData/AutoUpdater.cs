using HeroesParserData.Properties;
using NLog;
using Squirrel;
using System;
using System.Threading.Tasks;

namespace HeroesParserData
{
    public class AutoUpdater
    {
        private UpdateManager UpdateManager;
        private UpdateInfo UpdateInfo;
        private static Logger UpdaterLog = LogManager.GetLogger("UpdateLogFile");
        public Version CurrentVersion { get; private set; }
        public Version LatestVersion { get; private set; }

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

                    UpdaterLog.Log(LogLevel.Info, $"Current Version: {CurrentVersion}");
                    UpdaterLog.Log(LogLevel.Info, $"Latest Version: {LatestVersion}");

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

                    App.UpdateInProgress = true;
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
