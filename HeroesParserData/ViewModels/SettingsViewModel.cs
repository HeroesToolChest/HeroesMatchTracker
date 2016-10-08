using HeroesParserData.DataQueries;
using HeroesParserData.Properties;
using NLog;
using System;
using System.Threading.Tasks;
using System.Windows.Input;

namespace HeroesParserData.ViewModels
{
    public class SettingsViewModel : ViewModelBase
    {
        private string _inputBattleTagError;
        private string _checkForUpdatesResponse;
        private bool _isApplyUpdateEnabled;
        private bool _isCheckForUpdatesEnabled;

        private AutoUpdater AutoUpdater;

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

        public string CheckForUpdatesResponse
        {
            get { return _checkForUpdatesResponse; }
            set
            {
                _checkForUpdatesResponse = value;
                RaisePropertyChangedEvent(nameof(CheckForUpdatesResponse));
            }
        }

        public bool IsApplyUpdateEnabled
        {
            get { return _isApplyUpdateEnabled; }
            set
            {
                _isApplyUpdateEnabled = value;
                RaisePropertyChangedEvent(nameof(IsApplyUpdateEnabled));
            }
        }
        public bool IsCheckForUpdatesEnabled
        {
            get { return _isCheckForUpdatesEnabled; }
            set
            {
                _isCheckForUpdatesEnabled = value;
                RaisePropertyChangedEvent(nameof(IsCheckForUpdatesEnabled));
            }
        }

        public ICommand SetUserBattleTagCommand
        {
            get { return new DelegateCommand(() => SetBattleTagName()); }
        }

        public ICommand CheckForUpdatesCommand
        {
            get { return new DelegateCommand(() => CheckForUpdates()); }
        }

        public ICommand ApplyUpdateCommand
        {
            get { return new DelegateCommand(() => ApplyUpdates()); }
        }

        /// <summary>
        /// Contructor
        /// </summary>
        public SettingsViewModel()
            :base()
        {
            IsCheckForUpdatesEnabled = true;
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

        private void CheckForUpdates()
        {
            Task.Run(async () =>
            {
                try
                {
                    IsApplyUpdateEnabled = false;
                    CheckForUpdatesResponse = "Checking for updates...";
                    AutoUpdater = new AutoUpdater();
                    if (await AutoUpdater.CheckForUpdates())
                    {
                        CheckForUpdatesResponse = $"Update is available ({AutoUpdater.LatestVersionString})";
                        IsApplyUpdateEnabled = true;
                    }
                    else
                        CheckForUpdatesResponse = "Heroes Parser Data is up to date";
                }
                catch (Exception ex)
                {
                    CheckForUpdatesResponse = "Failed to check for updates";
                    ExceptionLog.Log(LogLevel.Error, ex);
                }
            });
        }

        private void ApplyUpdates()
        {
            Task.Run(async () =>
            {
                try
                {
                    CheckForUpdatesResponse = "Downloading and applying updates...";
                    await AutoUpdater.ApplyReleases();
                    CheckForUpdatesResponse = $"Finished applying update ({AutoUpdater.LatestVersion.Major}.{AutoUpdater.LatestVersion.Minor}.{AutoUpdater.LatestVersion.Revision}). A restart is required for changes to apply.";
                    App.NewReleaseApplied = true;
                    IsCheckForUpdatesEnabled = false;
                    IsApplyUpdateEnabled = false;
                }
                catch (Exception ex)
                {
                    CheckForUpdatesResponse = "Failed to apply updates";
                    ExceptionLog.Log(LogLevel.Error, ex);
                }
            });
        }
    }
}
