using HeroesStatTracker.Data.Databases;
using HeroesStatTracker.Data.Settings.Models;
using System;
using System.Linq;

namespace HeroesStatTracker.Data.Settings.Queries
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

        private bool GetBooleanValue(string name)
        {
            using (var db = new SettingsContext())
            {
                return bool.Parse(db.UserSettings.Where(x => x.Name == name).FirstOrDefault().Value);
            }
        }

        private void SetBooleanValue(string name, bool value)
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

        private string GetStringValue(string name)
        {
            using (var db = new SettingsContext())
            {
                return db.UserSettings.Where(x => x.Name == name).FirstOrDefault().Value;
            }
        }

        private void SetStringValue(string name, string value)
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

        private DateTime GetDateTimeValue(string name)
        {
            using (var db = new SettingsContext())
            {
                return DateTime.Parse(db.UserSettings.Where(x => x.Name == name).FirstOrDefault().Value);
            }
        }

        private void SetDateTimeValue(string name, DateTime value)
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

        private long GetLongValue(string name)
        {
            using (var db = new SettingsContext())
            {
                return long.Parse(db.UserSettings.Where(x => x.Name == name).FirstOrDefault().Value);
            }
        }

        private void SetLongValue(string name, long value)
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

        private int GetIntValue(string name)
        {
            using (var db = new SettingsContext())
            {
                return int.Parse(db.UserSettings.Where(x => x.Name == name).FirstOrDefault().Value);
            }
        }

        private void SetIntValue(string name, int value)
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
