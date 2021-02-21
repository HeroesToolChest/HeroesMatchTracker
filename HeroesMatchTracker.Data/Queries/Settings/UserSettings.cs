﻿using System;
using System.IO;

namespace HeroesMatchTracker.Data.Queries.Settings
{
    public partial class UserSettings
    {
        internal UserSettings() { }

        #region Settings
        public bool IsAutoUpdates
        {
            get { return GetBooleanValue(); }
            set { SetBooleanValue(value); }
        }

        public bool IsWindowsStartup
        {
            get { return GetBooleanValue(); }
            set { SetBooleanValue(value); }
        }

        public bool IsMinimizeToTray
        {
            get { return GetBooleanValue(); }
            set { SetBooleanValue(value); }
        }

        public bool IsBattleTagHidden
        {
            get { return GetBooleanValue(); }
            set { SetBooleanValue(value); }
        }

        public bool ShowToasterUpdateNotification
        {
            get { return GetBooleanValue(nameof(ShowToasterUpdateNotification)); }
            set { SetBooleanValue(value); }
        }
        #endregion Settings

        #region Replays
        public bool ReplayAutoScanCheckBox
        {
            get { return GetBooleanValue(); }
            set { SetBooleanValue(value); }
        }

        public bool ReplayWatchCheckBox
        {
            get { return GetBooleanValue(); }
            set { SetBooleanValue(value); }
        }

        public bool IsIncludeSubDirectories
        {
            get { return GetBooleanValue(); }
            set { SetBooleanValue(value); }
        }

        public bool ReplayAutoStartStartUp
        {
            get { return GetBooleanValue(); }
            set { SetBooleanValue(value); }
        }

        public DateTime ReplaysLatestSaved
        {
            get { return GetDateTimeValue(); }
            set { SetDateTimeValue(value); }
        }

        public DateTime ReplaysLastSaved
        {
            get { return GetDateTimeValue(); }
            set { SetDateTimeValue(value); }
        }

        public DateTime ReplaysLatestHotsLogs
        {
            get { return GetDateTimeValue(); }
            set { SetDateTimeValue(value); }
        }

        public DateTime ReplaysLastHotsLogs
        {
            get { return GetDateTimeValue(); }
            set { SetDateTimeValue(value); }
        }

        public DateTime ReplaysLatestHotsApi
        {
            get { return GetDateTimeValue(); }
            set { SetDateTimeValue(value); }
        }

        public DateTime ReplaysLastHotsApi
        {
            get { return GetDateTimeValue(); }
            set { SetDateTimeValue(value); }
        }

        public DateTime ReplaysLatestHeroesProfile
        {
            get { return GetDateTimeValue(); }
            set { SetDateTimeValue(value); }
        }

        public DateTime ReplaysLastHeroesProfile
        {
            get { return GetDateTimeValue(); }
            set { SetDateTimeValue(value); }
        }

        public bool IsHotsLogsUploaderEnabled
        {
            get { return GetBooleanValue(); }
            set { SetBooleanValue(value); }
        }

        public bool IsHotsApiUploaderEnabled
        {
            get { return GetBooleanValue(); }
            set { SetBooleanValue(value); }
        }

        public bool IsHeroesProfileUploaderEnabled
        {
            get { return GetBooleanValue(); }
            set { SetBooleanValue(value); }
        }

        public int SelectedScanDateTimeIndex
        {
            get { return GetIntValue(); }
            set { SetIntValue(value); }
        }

        public string ReplaysLocation
        {
            get { return GetStringValue(); }
            set { SetStringValue(value); }
        }

        public bool IsAutoRequeueOnUpdate
        {
            get { return GetBooleanValue(); }
            set { SetBooleanValue(value); }
        }
        #endregion Replays

        #region Selected User Profile
        public string UserBattleTagName
        {
            get { return GetStringValue(); }
            set { SetStringValue(value); }
        }

        public int UserRegion
        {
            get { return GetIntValue(); }
            set { SetIntValue(value); }
        }

        public long UserPlayerId
        {
            get { return GetLongValue(); }
            set { SetLongValue(value); }
        }
        #endregion User Profile

        #region Graphs
        public bool IsTeamExperienceRowChartEnabled
        {
            get { return GetBooleanValue(nameof(IsTeamExperienceRowChartEnabled)); }
            set { SetBooleanValue(value); }
        }

        public bool IsTeamExpOverTimeStackedAreaEnabled
        {
            get { return GetBooleanValue(nameof(IsTeamExpOverTimeStackedAreaEnabled)); }
            set { SetBooleanValue(value); }
        }
        #endregion

        #region Auto Updater

        /// <summary>
        /// Used for manually updating the application to determine if an update was applied
        /// </summary>
        public bool IsNewUpdateApplied
        {
            get { return GetBooleanValue(nameof(IsNewUpdateApplied)); }
            set { SetBooleanValue(value); }
        }

        /// <summary>
        /// User is already notified about the update
        /// </summary>
        public bool IsUpdateAvailableKnown
        {
            get { return GetBooleanValue(nameof(IsUpdateAvailableKnown)); }
            set { SetBooleanValue(value); }
        }
        #endregion Auto Updater

        #region Other

        /// <summary>
        /// Application was auto-started on windows startup
        /// </summary>
        public bool IsStartedViaStartup
        {
            get { return GetBooleanValue(); }
            set { SetBooleanValue(value); }
        }

        /// <summary>
        /// Has the What's New Window been shown yet
        /// </summary>
        public bool ShowWhatsNewWindow
        {
            get { return GetBooleanValue(); }
            set { SetBooleanValue(value); }
        }

        /// <summary>
        /// Has the failed replays been requeued yet
        /// </summary>
        public bool RequeueAllFailedReplays
        {
            get { return GetBooleanValue(); }
            set { SetBooleanValue(value); }
        }

        public bool PreReleaseCheck
        {
            get { return GetBooleanValue(); }
            set { SetBooleanValue(value); }
        }

        #endregion Other

        internal void SetDefaultSettings()
        {
            DateTime lastWeek = DateTime.Now.AddDays(-7);

            // Settings
            IsAutoUpdates = true;
            IsWindowsStartup = false;
            IsMinimizeToTray = true;
            IsBattleTagHidden = true;
            ShowToasterUpdateNotification = true;

            // Replays
            ReplayWatchCheckBox = false;
            ReplayAutoScanCheckBox = false;
            IsIncludeSubDirectories = true;
            ReplayAutoStartStartUp = false;
            IsHotsApiUploaderEnabled = false;
            IsHeroesProfileUploaderEnabled = false;
            ReplaysLatestSaved = lastWeek;
            ReplaysLastSaved = lastWeek;
            ReplaysLatestHotsLogs = lastWeek;
            ReplaysLastHotsLogs = lastWeek;
            ReplaysLatestHotsApi = lastWeek;
            ReplaysLastHotsApi = lastWeek;
            ReplaysLatestHeroesProfile = lastWeek;
            ReplaysLastHeroesProfile = lastWeek;
            SelectedScanDateTimeIndex = 0;
            ReplaysLocation = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "Heroes of the Storm", "Accounts");
            IsAutoRequeueOnUpdate = true;

            // User Profile
            UserBattleTagName = string.Empty;
            UserRegion = -1;
            UserPlayerId = 0;

            // Graphs
            IsTeamExperienceRowChartEnabled = true;
            IsTeamExpOverTimeStackedAreaEnabled = true;

            // AutoUpdater
            IsNewUpdateApplied = false;

            // Other
            IsStartedViaStartup = false;
            IsUpdateAvailableKnown = false;
            ShowWhatsNewWindow = false;
            RequeueAllFailedReplays = false;
            PreReleaseCheck = false;
        }
    }
}
