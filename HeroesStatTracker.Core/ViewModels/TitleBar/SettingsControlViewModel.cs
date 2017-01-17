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
        /// <summary>
        /// Constructor
        /// </summary>
        public SettingsControlViewModel()
        {
            Messenger.Default.Register<NotificationMessage>(this, (message) => ToggleIsNightMode(message));
        }

        public RelayCommand OpenPaletteSelectorWindowCommand => new RelayCommand(OpenPaletteSelectorWindow);
        public RelayCommand<bool> ToggleBaseCommand => new RelayCommand<bool>(x => StylePalette.ApplyBase(x));

        public bool IsMinimizeToTray
        {
            get { return QueryDb.SettingsDb.UserSettings.IsMinimizeToTray; }
            set
            {
                QueryDb.SettingsDb.UserSettings.IsMinimizeToTray = value;
                RaisePropertyChanged();
            }
        }

        public bool IsAutoUpdates
        {
            get { return QueryDb.SettingsDb.UserSettings.IsAutoUpdates; }
            set
            {
                QueryDb.SettingsDb.UserSettings.IsAutoUpdates = value;
                if (!value)
                    IsIncludePreReleaseBuilds = false;

                RaisePropertyChanged();
            }
        }

        public bool IsIncludePreReleaseBuilds
        {
            get { return QueryDb.SettingsDb.UserSettings.IsIncludePreRelease; }
            set
            {
                QueryDb.SettingsDb.UserSettings.IsIncludePreRelease = value;
                RaisePropertyChanged();
            }
        }

        public bool IsBattleTagsHidden
        {
            get { return QueryDb.SettingsDb.UserSettings.IsBattleTagHidden; }
            set
            {
                QueryDb.SettingsDb.UserSettings.IsBattleTagHidden = value;
                RaisePropertyChanged();
            }
        }

        public bool IsNightMode
        {
            get { return QueryDb.SettingsDb.UserSettings.IsNightMode; }
            set
            {
                QueryDb.SettingsDb.UserSettings.IsNightMode = value;
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
                IsNightMode = QueryDb.SettingsDb.UserSettings.IsNightMode;
        }
    }
}
