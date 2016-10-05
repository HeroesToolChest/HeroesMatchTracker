using HeroesParserData.DataQueries;
using HeroesParserData.Properties;
using System.Windows.Input;

namespace HeroesParserData.ViewModels
{
    public class SettingsViewModel : ViewModelBase
    {
        private string _inputBattleTagError;

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

        public string UserBattleTag
        {
            get { return Settings.Default.UserBattleTagName; }
            set
            {
                Settings.Default.UserBattleTagName = value;
                RaisePropertyChangedEvent(nameof(UserBattleTag));
            }
        }

        public string InputBattleTagError
        {
            get { return _inputBattleTagError; }
            set
            {
                _inputBattleTagError = value;
                RaisePropertyChangedEvent(nameof(InputBattleTagError));
            }
        }

        public ICommand SetUserBattleTag
        {
            get { return new DelegateCommand(() => SetBattleTagName()); }
        }

        /// <summary>
        /// Contructor
        /// </summary>
        public SettingsViewModel()
            :base()
        {

        }

        private void SetBattleTagName()
        {
            if (string.IsNullOrEmpty(UserBattleTag))
                return;
            else if (ValidateBattleTagName(UserBattleTag))
            {
                Settings.Default.UserPlayerId = Query.HotsPlayer.ReadPlayerIdFromBattleNetTag(UserBattleTag);
                InputBattleTagError = string.Empty;
            }
            else
            {
                Settings.Default.UserBattleTagName = string.Empty;
                InputBattleTagError = "BattleTag not found";
            }
        }

        private bool ValidateBattleTagName(string battleTagName)
        {
            return Query.HotsPlayer.IsValidBattleNetTagName(battleTagName);
        }
    }
}
