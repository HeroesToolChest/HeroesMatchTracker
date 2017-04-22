using HeroesMatchData.Core.Properties;
using HeroesMatchData.Core.ReleaseNotes;
using HeroesMatchData.Data;
using NLog;
using Squirrel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace HeroesMatchData.Core.Updater
{
    public class AutoUpdater
    {
        private static Logger UpdaterLog = LogManager.GetLogger(LogFileNames.UpdateFileName);

        private UpdateManager UpdateManager;
        private UpdateInfo UpdateInfo;

        private IDatabaseService Database;

        public AutoUpdater(IDatabaseService database)
        {
            Database = database;
        }

        public Version CurrentVersion { get; private set; }
        public string CurrentVersionString => CurrentVersion != null ? $"{CurrentVersion.Major}.{CurrentVersion.Minor}.{CurrentVersion.Build}" : string.Empty;
        public Version LatestVersion { get; private set; }
        public string LatestVersionString => LatestVersion != null ? $"{LatestVersion.Major}.{LatestVersion.Minor}.{LatestVersion.Build}" : string.Empty;

        public static async Task RetrieveReleaseNotes(IDatabaseService database)
        {
            ReleaseNoteHandler releaseNoteHandler = new ReleaseNoteHandler(database);
            await releaseNoteHandler.InitializeClient();

            // save the needed release notes
            releaseNoteHandler.AddApplyReleasesReleaseNotes();
        }

        /// <summary>
        /// Checks for updates, sets property UpdateInfo to null if no updates found. Returns true is update is available.
        /// </summary>
        /// <returns></returns>
        public async Task<bool> CheckForUpdates()
        {
            try
            {
                using (UpdateManager = await UpdateManager.GitHubUpdateManager(Settings.Default.UpdateUrl, prerelease: true))
                {
                    UpdaterLog.Log(LogLevel.Info, "Update Check");

                    UpdateInfo update = await UpdateManager.CheckForUpdate();

                    CurrentVersion = update.CurrentlyInstalledVersion?.Version.Version;
                    LatestVersion = update.FutureReleaseEntry?.Version.Version;

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

            // these two lines of code are for debugging/testing the update process
            // using (UpdateManager = new UpdateManager(@"C:\Users\koliva\Documents\Visual Studio 2015\Projects\GitHub\UpdateTest\UpdateTestRelease"))
            // CurrentVersion = new Version("1.3.0");
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

                    Settings.Default.NewLatestDirectory = directoryPath;

                    UpdaterLog.Log(LogLevel.Info, $"New directory path: {directoryPath}");

                    Database.SettingsDb().UserSettings.IsNewUpdateApplied = true;

                    return true;
                }
            }
            catch (Exception ex)
            {
                throw new AutoUpdaterException("Error applying releases", ex);
            }
        }

        public void RestartApp()
        {
            UpdateManager.RestartApp();
        }
    }
}
