using GalaSoft.MvvmLight;
using HeroesStatTracker.Data;

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
        }

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
    }
}
