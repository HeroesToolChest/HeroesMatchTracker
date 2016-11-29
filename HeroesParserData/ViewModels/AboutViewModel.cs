using NLog;
using System;
using System.Threading.Tasks;
using System.Windows.Input;

namespace HeroesParserData.ViewModels
{
    public class AboutViewModel : ViewModelBase
    {
        private string _checkForUpdatesResponse;
        private bool _isApplyUpdateEnabled;
        private bool _isCheckForUpdatesEnabled;

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

        public ICommand CheckForUpdatesCommand
        {
            get { return new DelegateCommand(() => CheckForUpdates()); }
        }

        public ICommand ApplyUpdateCommand
        {
            get { return new DelegateCommand(() => ApplyUpdates()); }
        }

        public AboutViewModel()
            :base()
        {
            IsCheckForUpdatesEnabled = true;
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

                    CheckForUpdatesResponse = "Retreiving release notes...";
                    await AutoUpdater.RetrieveReleaseNotes();

                    CheckForUpdatesResponse = $"Finished applying update ({AutoUpdater.LatestVersion.Major}.{AutoUpdater.LatestVersion.Minor}.{AutoUpdater.LatestVersion.Build}). A restart is required for changes to apply.";
                    App.ManualUpdateApplied = true;
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
