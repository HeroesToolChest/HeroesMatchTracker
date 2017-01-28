using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using GalaSoft.MvvmLight.Messaging;
using HeroesStatTracker.Core.Messaging;
using HeroesStatTracker.Core.ViewServices;
using HeroesStatTracker.Data;
using Microsoft.Practices.ServiceLocation;

namespace HeroesStatTracker.Core.ViewModels.TitleBar
{
    public class SettingsControlViewModel : ViewModelBase
    {
        private IDatabaseService Database;

        /// <summary>
        /// Constructor
        /// </summary>
        public SettingsControlViewModel(IDatabaseService database)
        {
            Database = database;
            Messenger.Default.Register<NotificationMessage>(this, (message) => ToggleIsNightMode(message));
        }

        public RelayCommand OpenPaletteSelectorWindowCommand => new RelayCommand(OpenPaletteSelectorWindow);
        public RelayCommand<bool> ToggleBaseCommand => new RelayCommand<bool>(x => StylePalette.ApplyBase(x));

        public bool IsMinimizeToTray
        {
            get { return Database.SettingsDb().UserSettings.IsMinimizeToTray; }
            set
            {
                Database.SettingsDb().UserSettings.IsMinimizeToTray = value;
                RaisePropertyChanged();
            }
        }

        public bool IsAutoUpdates
        {
            get { return Database.SettingsDb().UserSettings.IsAutoUpdates; }
            set
            {
                Database.SettingsDb().UserSettings.IsAutoUpdates = value;
                if (!value)
                    IsIncludePreReleaseBuilds = false;

                RaisePropertyChanged();
            }
        }

        public bool IsIncludePreReleaseBuilds
        {
            get { return Database.SettingsDb().UserSettings.IsIncludePreRelease; }
            set
            {
                Database.SettingsDb().UserSettings.IsIncludePreRelease = value;
                RaisePropertyChanged();
            }
        }

        public bool IsBattleTagsHidden
        {
            get { return Database.SettingsDb().UserSettings.IsBattleTagHidden; }
            set
            {
                Database.SettingsDb().UserSettings.IsBattleTagHidden = value;
                RaisePropertyChanged();
            }
        }

        public bool IsNightMode
        {
            get { return Database.SettingsDb().UserSettings.IsNightMode; }
            set
            {
                Database.SettingsDb().UserSettings.IsNightMode = value;
                RaisePropertyChanged();
            }
        }

        public IPaletteSelectorWindowService PaletteSelectorWindowService
        {
            get { return ServiceLocator.Current.GetInstance<IPaletteSelectorWindowService>(); }
        }

        private void OpenPaletteSelectorWindow()
        {
            PaletteSelectorWindowService.CreatePaletteWindow();
        }

        private void ToggleIsNightMode(NotificationMessage message)
        {
            if (message.Notification == StaticMessage.IsNightModeToggle)
                IsNightMode = Database.SettingsDb().UserSettings.IsNightMode;
        }
    }
}
