using System;
using System.IO;

namespace HeroesStatTracker.Data.Settings.Queries
{
    public partial class UserSettings
    {
        internal UserSettings() { }

        internal void SetDefaultSettings()
        {
            ReplayWatchCheckBox = false;
            ReplayAutoScanCheckBox = false;
            IsAutoUpdates = true;
            IsIncludePreRelease = false;
            IsMinimizeToTray = false;
            IsBattleTagHidden = true;
            IsIncludeSubDirectories = true;
            IsNewUpdateApplied = false;
            IsIncludeManualPreRelease = false;
            IsHotsLogsUploaderEnabled = false;
            IsTeamExperienceRowChartEnabled = true;

            SelectedScanDateTimeIndex = 0;

            UserPlayerId = 0;

            ReplaysLocation = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), @"Heroes of the Storm\Accounts");
            UserBattleTagName = string.Empty;

            ReplaysLatestSaved = new DateTime();
            ReplaysLastSaved = new DateTime();
            ReplaysLatestHotsLogs = new DateTime();
            ReplaysLastHotsLogs = new DateTime();
        }

        public bool ReplayAutoScanCheckBox
        {
            get { return GetBooleanValue(nameof(ReplayAutoScanCheckBox)); }
            set { SetBooleanValue(nameof(ReplayAutoScanCheckBox), value); }
        }

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

        public bool IsIncludeManualPreRelease
        {
            get { return GetBooleanValue(nameof(IsIncludeManualPreRelease)); }
            set { SetBooleanValue(nameof(IsIncludeManualPreRelease), value); }
        }

        public bool ReplayWatchCheckBox
        {
            get { return GetBooleanValue(nameof(ReplayWatchCheckBox)); }
            set { SetBooleanValue(nameof(ReplayWatchCheckBox), value); }
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

        public bool IsIncludeSubDirectories
        {
            get { return GetBooleanValue(nameof(IsIncludeSubDirectories)); }
            set { SetBooleanValue(nameof(IsIncludeSubDirectories), value); }
        }

        public bool IsNewUpdateApplied
        {
            get { return GetBooleanValue(nameof(IsNewUpdateApplied)); }
            set { SetBooleanValue(nameof(IsNewUpdateApplied), value); }
        }

        public bool IsHotsLogsUploaderEnabled
        {
            get { return GetBooleanValue(nameof(IsHotsLogsUploaderEnabled)); }
            set { SetBooleanValue(nameof(IsHotsLogsUploaderEnabled), value); }
        }

        public bool IsTeamExperienceRowChartEnabled
        {
            get { return GetBooleanValue(nameof(IsTeamExperienceRowChartEnabled)); }
            set { SetBooleanValue(nameof(IsTeamExperienceRowChartEnabled), value); }
        }

        public int SelectedScanDateTimeIndex
        {
            get { return GetIntValue(nameof(SelectedScanDateTimeIndex)); }
            set { SetIntValue(nameof(SelectedScanDateTimeIndex), value); }
        }

        public long UserPlayerId
        {
            get { return GetLongValue(nameof(UserPlayerId)); }
            set { SetLongValue(nameof(UserPlayerId), value); }
        }

        public string ReplaysLocation
        {
            get { return GetStringValue(nameof(ReplaysLocation)); }
            set { SetStringValue(nameof(ReplaysLocation), value); }
        }

        public string UserBattleTagName
        {
            get { return GetStringValue(nameof(UserBattleTagName)); }
            set { SetStringValue(nameof(UserBattleTagName), value); }
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
    }
}
