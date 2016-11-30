using GalaSoft.MvvmLight.Messaging;
using HeroesParserData.Messages;
using NLog;
using System;
using System.Threading.Tasks;
using System.Windows.Input;

namespace HeroesParserData.ViewModels
{
    public class AboutViewModel : ViewModelBase
    {
        private string _checkForUpdatesResponse;
        private bool _isApplyUpdateButtonEnabled;
        private bool _isCheckForUpdatesButtonEnabled;

        private AutoUpdater AutoUpdater;

        public string AppVersion
        {
            get { return HPDVersion.GetVersionAsString(); }
        }

        public string HeroesIconsVersion
        {
            get { return HPDVersion.GetHeroesIconsVersionAsString(); }
        }

        public string HeroesReplayParserVersion
        {
            get { return HPDVersion.GetHeroesReplayParserVersionAsString(); }
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

        public bool IsApplyUpdateButtonEnabled
        {
            get { return _isApplyUpdateButtonEnabled; }
            set
            {
                _isApplyUpdateButtonEnabled = value;
                RaisePropertyChangedEvent(nameof(IsApplyUpdateButtonEnabled));
            }
        }

        public bool IsCheckForUpdatesButtonEnabled
        {
            get { return _isCheckForUpdatesButtonEnabled; }
            set
            {
                _isCheckForUpdatesButtonEnabled = value;
                RaisePropertyChangedEvent(nameof(IsCheckForUpdatesButtonEnabled));
            }
        }

        public ICommand CheckForUpdatesCommand
        {
            get { return new DelegateCommand(() => CheckForUpdates()); }
        }

        public ICommand ApplyUpdateCommand
        {
            get { return new DelegateCommand(() => ApplyUpdates()); }
        }

        public AboutViewModel()
            : base()
        {
            IsCheckForUpdatesButtonEnabled = true;
            PeriodicallyCheckUpdates();
        }

        private void CheckForUpdates()
        {
            Task.Run(async () =>
            {
                try
                {
                    IsCheckForUpdatesButtonEnabled = false;
                    IsApplyUpdateButtonEnabled = false;

                    CheckForUpdatesResponse = "Checking for updates...";
                    AutoUpdater = new AutoUpdater();
                    if (await AutoUpdater.CheckForUpdates())
                    {
                        CheckForUpdatesResponse = $"Update is available ({AutoUpdater.LatestVersionString})";
                        UpdateIsAvailableMessage();
                        IsApplyUpdateButtonEnabled = true;
                    }
                    else
                        CheckForUpdatesResponse = "Heroes Parser Data is up to date";
                }
                catch (Exception ex)
                {
                    CheckForUpdatesResponse = "Unable to check for updates";
                    WarningLog.Log(LogLevel.Warn, $"Unable to check for updates: {ex.Message}");
                }

                IsCheckForUpdatesButtonEnabled = true;
            });
        }

        private void ApplyUpdates()
        {
            Task.Run(async () =>
            {
                try
                {
                    // set both buttons to false and keep them false as a restart is required
                    IsCheckForUpdatesButtonEnabled = false;
                    IsApplyUpdateButtonEnabled = false;

                    CheckForUpdatesResponse = "Downloading and applying updates...";

                    await AutoUpdater.ApplyReleases();

                    CheckForUpdatesResponse = "Retreiving release notes...";
                    await AutoUpdater.RetrieveReleaseNotes();

                    CheckForUpdatesResponse = $"Finished applying update ({AutoUpdater.LatestVersion.Major}.{AutoUpdater.LatestVersion.Minor}.{AutoUpdater.LatestVersion.Build}). A restart is required for changes to apply.";
                    RestartIsRequiredMessage();
                    App.ManualUpdateApplied = true;
                }
                catch (Exception ex)
                {
                    CheckForUpdatesResponse = "Unable to apply updates. Check logs for details.";

                    IsCheckForUpdatesButtonEnabled = true;
                    IsApplyUpdateButtonEnabled = true;

                    ExceptionLog.Log(LogLevel.Error, ex);
                }
            });
        }

        private void PeriodicallyCheckUpdates()
        {
            Task.Run(async () =>
            {
                while (true)
                {
                    await Task.Delay(3600000); // 1 hour

                    if (IsCheckForUpdatesButtonEnabled == false)
                        continue;

                    try
                    {
                        IsCheckForUpdatesButtonEnabled = false;

                        AutoUpdater = new AutoUpdater();
                        if (await AutoUpdater.CheckForUpdates())
                        {
                            CheckForUpdatesResponse = $"Update is available ({AutoUpdater.LatestVersionString})";
                            UpdateIsAvailableMessage();
                            IsApplyUpdateButtonEnabled = true;
                        }
                        else
                            CheckForUpdatesResponse = string.Empty;
                    }
                    catch (Exception ex)
                    {
                        WarningLog.Log(LogLevel.Warn, $"Unable to periodically check for update: {ex.Message}");
                    }

                    IsCheckForUpdatesButtonEnabled = true;
                }
            });
        }

        private void UpdateIsAvailableMessage()
        {
            Messenger.Default.Send(new AboutUpdateMessage { Message = " (Update Available)", IsVisible = true });
        }

        private void RestartIsRequiredMessage()
        {
            Messenger.Default.Send(new AboutUpdateMessage { Message = " (Restart Required)", IsVisible = true });
        }
    }
}
