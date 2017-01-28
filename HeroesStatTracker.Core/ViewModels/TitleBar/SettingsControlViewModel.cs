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
        private IDatabaseService IDatabaseService;

        /// <summary>
        /// Constructor
        /// </summary>
        public SettingsControlViewModel(IDatabaseService iDatabaseService)
        {
            IDatabaseService = iDatabaseService;
            Messenger.Default.Register<NotificationMessage>(this, (message) => ToggleIsNightMode(message));
        }

        public RelayCommand OpenPaletteSelectorWindowCommand => new RelayCommand(OpenPaletteSelectorWindow);
        public RelayCommand<bool> ToggleBaseCommand => new RelayCommand<bool>(x => StylePalette.ApplyBase(x));

        public bool IsMinimizeToTray
        {
            get { return IDatabaseService.SettingsDb().UserSettings.IsMinimizeToTray; }
            set
            {
                IDatabaseService.SettingsDb().UserSettings.IsMinimizeToTray = value;
                RaisePropertyChanged();
            }
        }

        public bool IsAutoUpdates
        {
            get { return IDatabaseService.SettingsDb().UserSettings.IsAutoUpdates; }
            set
            {
                IDatabaseService.SettingsDb().UserSettings.IsAutoUpdates = value;
                if (!value)
                    IsIncludePreReleaseBuilds = false;

                RaisePropertyChanged();
            }
        }

        public bool IsIncludePreReleaseBuilds
        {
            get { return IDatabaseService.SettingsDb().UserSettings.IsIncludePreRelease; }
            set
            {
                IDatabaseService.SettingsDb().UserSettings.IsIncludePreRelease = value;
                RaisePropertyChanged();
            }
        }

        public bool IsBattleTagsHidden
        {
            get { return IDatabaseService.SettingsDb().UserSettings.IsBattleTagHidden; }
            set
            {
                IDatabaseService.SettingsDb().UserSettings.IsBattleTagHidden = value;
                RaisePropertyChanged();
            }
        }

        public bool IsNightMode
        {
            get { return IDatabaseService.SettingsDb().UserSettings.IsNightMode; }
            set
            {
                IDatabaseService.SettingsDb().UserSettings.IsNightMode = value;
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
                IsNightMode = IDatabaseService.SettingsDb().UserSettings.IsNightMode;
        }
    }
}
