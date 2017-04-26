using GalaSoft.MvvmLight;
using HeroesMatchTracker.Data;
using Microsoft.Win32;
using System.IO;
using System.Reflection;

namespace HeroesMatchTracker.Core.ViewModels.TitleBar
{
    public class SettingsControlViewModel : ViewModelBase
    {
        private bool _isMinimizeToTrayEnabled;
        private IDatabaseService Database;
        private RegistryKey RegistryKey = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true);

        /// <summary>
        /// Constructor
        /// </summary>
        public SettingsControlViewModel(IDatabaseService database)
        {
            Database = database;

            IsWindowsStartup = Database.SettingsDb().UserSettings.IsWindowsStartup;
        }

        public bool IsWindowsStartup
        {
            get => Database.SettingsDb().UserSettings.IsWindowsStartup;
            set
            {
                Database.SettingsDb().UserSettings.IsWindowsStartup = value;

                if (value)
                {
                    IsMinimizeToTray = true;
                    IsMinimizeToTrayEnabled = false;
                }
                else
                {
                    IsMinimizeToTrayEnabled = true;
                }

                SetRegistryStartup(value);
                RaisePropertyChanged();
            }
        }

        public bool IsMinimizeToTray
        {
            get => Database.SettingsDb().UserSettings.IsMinimizeToTray;
            set
            {
                Database.SettingsDb().UserSettings.IsMinimizeToTray = value;
                RaisePropertyChanged();
            }
        }

        public bool IsAutoUpdates
        {
            get => Database.SettingsDb().UserSettings.IsAutoUpdates;
            set
            {
                Database.SettingsDb().UserSettings.IsAutoUpdates = value;
                RaisePropertyChanged();
            }
        }

        public bool IsBattleTagsHidden
        {
            get => Database.SettingsDb().UserSettings.IsBattleTagHidden;
            set
            {
                Database.SettingsDb().UserSettings.IsBattleTagHidden = value;
                RaisePropertyChanged();
            }
        }

        public bool IsMinimizeToTrayEnabled
        {
            get => _isMinimizeToTrayEnabled;
            set
            {
                _isMinimizeToTrayEnabled = value;
                RaisePropertyChanged();
            }
        }

        private void SetRegistryStartup(bool set)
        {
            if (set)
            {
                RegistryKey.SetValue("Heroes Match Tracker", $"{Path.Combine(Directory.GetParent(Data.Database.DatabasePath).FullName, "Update.exe")} --processStart \"HeroesMatchTracker.exe\" --process-start-args /noshow");
            }
            else
            {
                if (RegistryKey.GetValue("Heroes Match Tracker") != null)
                    RegistryKey.DeleteValue("Heroes Match Tracker");
            }
        }
    }
}
