using Heroes.Helpers;
using System;
using System.IO;

namespace HeroesStatTracker.Data.Queries.Settings
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

        public bool IsIncludePreRelease
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

        public bool IsNightMode
        {
            get { return GetBooleanValue(); }
            set { SetBooleanValue(value); }
        }

        public bool IsAlternateStyle
        {
            get { return GetBooleanValue(); }
            set { SetBooleanValue(value); }
        }

        public string MainStylePrimary
        {
            get { return GetStringValue(); }
            set { SetStringValue(value); }
        }

        public string MainStyleAccent
        {
            get { return GetStringValue(); }
            set { SetStringValue(value); }
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

        public bool IsHotsLogsUploaderEnabled
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
        #endregion Replays

        #region User Profile
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

        public bool IsIncludeManualPreRelease
        {
            get { return GetBooleanValue(); }
            set { SetBooleanValue(value); }
        }

        public bool IsNewUpdateApplied
        {
            get { return GetBooleanValue(nameof(IsNewUpdateApplied)); }
            set { SetBooleanValue(value); }
        }

        public bool IsTeamExperienceRowChartEnabled
        {
            get { return GetBooleanValue(nameof(IsTeamExperienceRowChartEnabled)); }
            set { SetBooleanValue(value); }
        }

        internal void SetDefaultSettings()
        {
            // Settings
            IsAutoUpdates = true;
            IsIncludePreRelease = false;
            IsMinimizeToTray = false;
            IsBattleTagHidden = true;
            IsNightMode = false;
            IsAlternateStyle = false;
            MainStylePrimary = HeroesHelpers.DefaultColorPalette.DefaultPrimary;
            MainStyleAccent = HeroesHelpers.DefaultColorPalette.DefaultAccent;

            // Replays
            ReplayWatchCheckBox = false;
            ReplayAutoScanCheckBox = false;
            IsIncludeSubDirectories = true;
            ReplayAutoStartStartUp = false;
            IsHotsLogsUploaderEnabled = false;
            ReplaysLatestSaved = DateTime.Now;
            ReplaysLastSaved = DateTime.Now;
            ReplaysLatestHotsLogs = DateTime.Now;
            ReplaysLastHotsLogs = DateTime.Now;
            SelectedScanDateTimeIndex = 0;
            ReplaysLocation = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), @"Heroes of the Storm\Accounts");

            // User Profile
            UserBattleTagName = string.Empty;
            UserRegion = -1;
            UserPlayerId = 0;

            // needs to be organized
            IsNewUpdateApplied = false;
            IsIncludeManualPreRelease = false;

            IsTeamExperienceRowChartEnabled = true;
        }
    }
}
