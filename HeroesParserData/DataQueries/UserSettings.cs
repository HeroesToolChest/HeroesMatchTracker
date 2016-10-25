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
            ParsedDateTimeCheckBox = true;
            IsAutoUpdates = true;
            IsMinimizeToTray = false;
            IsBattleTagHidden = true;
            IsIncludeSubDirectories = true;

            UserPlayerId = 0;

            ReplaysLocation = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), @"Heroes of the Storm\Accounts");
            UserBattleTagName = string.Empty;
            SelectedSeason = "Season 2";

            ReplaysLatestSaved = new DateTime();
            ReplaysLastSaved = new DateTime();
        }

        public bool ReplayAutoScanCheckBox
        {
            get { return GetBooleanValue(nameof(ReplayAutoScanCheckBox)); }
            set { SetBooleanValue(nameof(ReplayAutoScanCheckBox), value); }
        }

        public bool ParsedDateTimeCheckBox
        {
            get { return GetBooleanValue(nameof(ParsedDateTimeCheckBox)); }
            set { SetBooleanValue(nameof(ParsedDateTimeCheckBox), value); }
        }

        public bool IsAutoUpdates
        {
            get { return GetBooleanValue(nameof(IsAutoUpdates)); }
            set { SetBooleanValue(nameof(IsAutoUpdates), value); }
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

        public string SelectedSeason
        {
            get { return GetStringValue(nameof(SelectedSeason)); }
            set { SetStringValue(nameof(SelectedSeason), value); }
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

        private bool GetBooleanValue(string name)
        {
            using (var db = new HeroesParserDataContext())
            {
                return bool.Parse(db.UserSettings.Where(x => x.Name == name).FirstOrDefault().Value);
            }
        }

        private void CreateNewSetting(UserSetting userSetting)
        {
            using (var db = new HeroesParserDataContext())
            {
                db.UserSettings.Add(userSetting);
                db.SaveChanges();
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
    }
}
