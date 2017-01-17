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
            get { return GetBooleanValue(nameof(IsAutoUpdates)); }
            set { SetBooleanValue(nameof(IsAutoUpdates), value); }
        }

        public bool IsIncludePreRelease
        {
            get { return GetBooleanValue(nameof(IsIncludePreRelease)); }
            set { SetBooleanValue(nameof(IsIncludePreRelease), value); }
        }

        public bool IsMinimizeToTray
        {
            get { return GetBooleanValue(nameof(IsMinimizeToTray)); }
            set { SetBooleanValue(nameof(IsMinimizeToTray), value); }
        }

        public bool IsBattleTagHidden
        {
            get { return GetBooleanValue(nameof(IsBattleTagHidden)); }
            set { SetBooleanValue(nameof(IsBattleTagHidden), value); }
        }

        public bool IsNightMode
        {
            get { return GetBooleanValue(nameof(IsNightMode)); }
            set { SetBooleanValue(nameof(IsNightMode), value); }
        }

        public bool IsAlternateStyle
        {
            get { return GetBooleanValue(nameof(IsAlternateStyle)); }
            set { SetBooleanValue(nameof(IsAlternateStyle), value); }
        }

        public string MainStylePrimary
        {
            get { return GetStringValue(nameof(MainStylePrimary)); }
            set { SetStringValue(nameof(MainStylePrimary), value); }
        }

        public string MainStyleAccent
        {
            get { return GetStringValue(nameof(MainStyleAccent)); }
            set { SetStringValue(nameof(MainStyleAccent), value); }
        }
        #endregion Settings

        #region Replays
        public bool ReplayAutoScanCheckBox
        {
            get { return GetBooleanValue(nameof(ReplayAutoScanCheckBox)); }
            set { SetBooleanValue(nameof(ReplayAutoScanCheckBox), value); }
        }

        public bool ReplayWatchCheckBox
        {
            get { return GetBooleanValue(nameof(ReplayWatchCheckBox)); }
            set { SetBooleanValue(nameof(ReplayWatchCheckBox), value); }
        }

        public bool IsIncludeSubDirectories
        {
            get { return GetBooleanValue(nameof(IsIncludeSubDirectories)); }
            set { SetBooleanValue(nameof(IsIncludeSubDirectories), value); }
        }

        public bool ReplayAutoStartStartUp
        {
            get { return GetBooleanValue(nameof(ReplayAutoStartStartUp)); }
            set { SetBooleanValue(nameof(ReplayAutoStartStartUp), value); }
        }

        public DateTime ReplaysLatestSaved
        {
            get { return GetDateTimeValue(nameof(ReplaysLatestSaved)); }
            set { SetDateTimeValue(nameof(ReplaysLatestSaved), value); }
        }

        public DateTime ReplaysLastSaved
        {
            get { return GetDateTimeValue(nameof(ReplaysLastSaved)); }
            set { SetDateTimeValue(nameof(ReplaysLastSaved), value); }
        }

        public DateTime ReplaysLatestHotsLogs
        {
            get { return GetDateTimeValue(nameof(ReplaysLatestHotsLogs)); }
            set { SetDateTimeValue(nameof(ReplaysLatestHotsLogs), value); }
        }

        public DateTime ReplaysLastHotsLogs
        {
            get { return GetDateTimeValue(nameof(ReplaysLastHotsLogs)); }
            set { SetDateTimeValue(nameof(ReplaysLastHotsLogs), value); }
        }

        public bool IsHotsLogsUploaderEnabled
        {
            get { return GetBooleanValue(nameof(IsHotsLogsUploaderEnabled)); }
            set { SetBooleanValue(nameof(IsHotsLogsUploaderEnabled), value); }
        }

        public int SelectedScanDateTimeIndex
        {
            get { return GetIntValue(nameof(SelectedScanDateTimeIndex)); }
            set { SetIntValue(nameof(SelectedScanDateTimeIndex), value); }
        }

        public string ReplaysLocation
        {
            get { return GetStringValue(nameof(ReplaysLocation)); }
            set { SetStringValue(nameof(ReplaysLocation), value); }
        }
        #endregion Replays

        public bool IsIncludeManualPreRelease
        {
            get { return GetBooleanValue(nameof(IsIncludeManualPreRelease)); }
            set { SetBooleanValue(nameof(IsIncludeManualPreRelease), value); }
        }

        public bool IsNewUpdateApplied
        {
            get { return GetBooleanValue(nameof(IsNewUpdateApplied)); }
            set { SetBooleanValue(nameof(IsNewUpdateApplied), value); }
        }

        public bool IsTeamExperienceRowChartEnabled
        {
            get { return GetBooleanValue(nameof(IsTeamExperienceRowChartEnabled)); }
            set { SetBooleanValue(nameof(IsTeamExperienceRowChartEnabled), value); }
        }

        public long UserPlayerId
        {
            get { return GetLongValue(nameof(UserPlayerId)); }
            set { SetLongValue(nameof(UserPlayerId), value); }
        }

        public string UserBattleTagName
        {
            get { return GetStringValue(nameof(UserBattleTagName)); }
            set { SetStringValue(nameof(UserBattleTagName), value); }
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
            MainStylePrimary = "blue";
            MainStyleAccent = "lightblue";

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

            // needs to be organized
            IsNewUpdateApplied = false;
            IsIncludeManualPreRelease = false;

            IsTeamExperienceRowChartEnabled = true;

            UserPlayerId = 0;

            UserBattleTagName = string.Empty;
        }
    }
}
