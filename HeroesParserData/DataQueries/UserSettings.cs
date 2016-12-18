using HeroesParserData.Models.DbModels;
using System;
using System.IO;
using System.Linq;

namespace HeroesParserData
{
    public sealed class UserSettings
    {
        public static UserSettings Default
        {
            get { return new UserSettings(); }
        }

        public void SetDefaultSettings()
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

        private void CreateNewSetting(UserSetting userSetting)
        {
            using (var db = new HeroesParserDataContext())
            {
                db.UserSettings.Add(userSetting);
                db.SaveChanges();
            }
        }

        private bool GetBooleanValue(string name)
        {
            using (var db = new HeroesParserDataContext())
            {
                return bool.Parse(db.UserSettings.Where(x => x.Name == name).FirstOrDefault().Value);
            }
        }

        private void SetBooleanValue(string name, bool value)
        {
            using (var db = new HeroesParserDataContext())
            {
                var record = db.UserSettings.Where(x => x.Name == name).FirstOrDefault();
                if (record != null)
                {
                    record.Value = value.ToString();
                    db.SaveChanges();
                }
                else
                {
                    CreateNewSetting(new UserSetting { Name = name, Value = value.ToString() });
                }
            }
        }

        private string GetStringValue(string name)
        {
            using (var db = new HeroesParserDataContext())
            {
                return db.UserSettings.Where(x => x.Name == name).FirstOrDefault().Value;
            }
        }

        private void SetStringValue(string name, string value)
        {
            using (var db = new HeroesParserDataContext())
            {
                var record = db.UserSettings.Where(x => x.Name == name).FirstOrDefault();
                if (record != null)
                {
                    record.Value = value;
                    db.SaveChanges();
                }
                else
                {
                    CreateNewSetting(new UserSetting { Name = name, Value = value });
                }
            }
        }

        private DateTime GetDateTimeValue(string name)
        {
            using (var db = new HeroesParserDataContext())
            {
                return DateTime.Parse(db.UserSettings.Where(x => x.Name == name).FirstOrDefault().Value);
            }
        }

        private void SetDateTimeValue(string name, DateTime value)
        {
            using (var db = new HeroesParserDataContext())
            {
                var record = db.UserSettings.Where(x => x.Name == name).FirstOrDefault();
                if (record != null)
                {
                    record.Value = value.ToString();
                    db.SaveChanges();
                }
                else
                {
                    CreateNewSetting(new UserSetting { Name = name, Value = value.ToString() });
                }
            }
        }

        private long GetLongValue(string name)
        {
            using (var db = new HeroesParserDataContext())
            {
                return long.Parse(db.UserSettings.Where(x => x.Name == name).FirstOrDefault().Value);
            }
        }

        private void SetLongValue(string name, long value)
        {
            using (var db = new HeroesParserDataContext())
            {
                var record = db.UserSettings.Where(x => x.Name == name).FirstOrDefault();
                if (record != null)
                {
                    record.Value = value.ToString();
                    db.SaveChanges();
                }
                else
                {
                    CreateNewSetting(new UserSetting { Name = name, Value = value.ToString() });
                }
            }
        }

        private int GetIntValue(string name)
        {
            using (var db = new HeroesParserDataContext())
            {
                return int.Parse(db.UserSettings.Where(x => x.Name == name).FirstOrDefault().Value);
            }
        }

        private void SetIntValue(string name, int value)
        {
            using (var db = new HeroesParserDataContext())
            {
                var record = db.UserSettings.Where(x => x.Name == name).FirstOrDefault();
                if (record != null)
                {
                    record.Value = value.ToString();
                    db.SaveChanges();
                }
                else
                {
                    CreateNewSetting(new UserSetting { Name = name, Value = value.ToString() });
                }
            }
        }
    }
}
