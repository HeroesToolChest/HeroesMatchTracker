using HeroesParserData.Properties;

namespace HeroesParserData.ViewModels
{
    public class SettingsViewModel : ViewModelBase
    {
        public bool IsMinimizeToTray
        {
            get { return Settings.Default.IsMinimizeToTray; }
            set
            {
                Settings.Default.IsMinimizeToTray = value;
                RaisePropertyChangedEvent(nameof(IsMinimizeToTray));
            }
        }

        public bool IsAutoUpdates
        {
            get { return Settings.Default.IsAutoUpdates; }
            set
            {
                Settings.Default.IsAutoUpdates = value;
                RaisePropertyChangedEvent(nameof(IsAutoUpdates));
            }
        }

        public bool IsBattleTagsHidden
        {
            get { return Settings.Default.IsBattleTagHidden; }
            set
            {
                Settings.Default.IsBattleTagHidden = value;
                RaisePropertyChangedEvent(nameof(IsBattleTagsHidden));
            }
        }
        public SettingsViewModel()
            :base()
        {

        }
    }
}
