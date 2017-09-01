using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using HeroesMatchTracker.Core.ViewServices;
using HeroesMatchTracker.Data;
using Microsoft.Practices.ServiceLocation;
using Microsoft.Win32;
using System.IO;

namespace HeroesMatchTracker.Core.ViewModels.TitleBar.Settings
{
    public class SettingsControlViewModel : ViewModelBase
    {
        private bool _isMinimizeToTrayEnabled;

        private IDatabaseService Database;

        private RegistryKey RegistryKey = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true);

        public SettingsControlViewModel(IDatabaseService database)
        {
            Database = database;

            IsWindowsStartup = Database.SettingsDb().UserSettings.IsWindowsStartup;
        }

        public ICreateWindowService CreateWindow => ServiceLocator.Current.GetInstance<ICreateWindowService>();

        public RelayCommand UpdateDataFolderLocationCommand => new RelayCommand(UpdateDataFolderLocation);

        public IMainWindowDialogService MainWindowDialog => ServiceLocator.Current.GetInstance<IMainWindowDialogService>();

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

#if !DEBUG
                SetRegistryStartup(value);
#endif
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

        public bool ShowToasterUpdateNotification
        {
            get => Database.SettingsDb().UserSettings.ShowToasterUpdateNotification;
            set
            {
                Database.SettingsDb().UserSettings.ShowToasterUpdateNotification = value;
                RaisePropertyChanged();
            }
        }

        public string DataFolderLocation
        {
            get => Database.SettingsDb().UserSettings.DataFolderLocation;
            set
            {
                Database.SettingsDb().UserSettings.DataFolderLocation = value;
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

        private void UpdateDataFolderLocation()
        {
            CreateWindow.ShowDataFolderWindow();
        }
    }
}
