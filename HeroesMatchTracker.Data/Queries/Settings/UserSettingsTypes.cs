using HeroesMatchTracker.Data.Databases;
using HeroesMatchTracker.Data.Models.Settings;
using System;
using System.Linq;
using System.Runtime.CompilerServices;

namespace HeroesMatchTracker.Data.Queries.Settings
{
    public partial class UserSettings
    {
        private void CreateNewSetting(UserSetting userSetting)
        {
            using (var db = new SettingsContext())
            {
                db.UserSettings.Add(userSetting);
                db.SaveChanges();
            }
        }

        private bool GetBooleanValue([CallerMemberName] string name = null)
        {
            using (var db = new SettingsContext())
            {
                return bool.Parse(db.UserSettings.Where(x => x.Name == name).FirstOrDefault().Value);
            }
        }

        private void SetBooleanValue(bool value, [CallerMemberName] string name = null)
        {
            using (var db = new SettingsContext())
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

        private string GetStringValue([CallerMemberName] string name = null)
        {
            using (var db = new SettingsContext())
            {
                return db.UserSettings.Where(x => x.Name == name).FirstOrDefault().Value;
            }
        }

        private void SetStringValue(string value, [CallerMemberName] string name = null)
        {
            using (var db = new SettingsContext())
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

        private DateTime GetDateTimeValue([CallerMemberName] string name = null)
        {
            using (var db = new SettingsContext())
            {
                return DateTime.Parse(db.UserSettings.Where(x => x.Name == name).FirstOrDefault().Value);
            }
        }

        private void SetDateTimeValue(DateTime value, [CallerMemberName] string name = null)
        {
            using (var db = new SettingsContext())
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

        private long GetLongValue([CallerMemberName] string name = null)
        {
            using (var db = new SettingsContext())
            {
                return long.Parse(db.UserSettings.Where(x => x.Name == name).FirstOrDefault().Value);
            }
        }

        private void SetLongValue(long value, [CallerMemberName] string name = null)
        {
            using (var db = new SettingsContext())
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

        private int GetIntValue([CallerMemberName] string name = null)
        {
            using (var db = new SettingsContext())
            {
                return int.Parse(db.UserSettings.Where(x => x.Name == name).FirstOrDefault().Value);
            }
        }

        private void SetIntValue(int value, [CallerMemberName] string name = null)
        {
            using (var db = new SettingsContext())
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
