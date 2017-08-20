using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using Heroes.Icons;
using HeroesMatchTracker.Core.Updater;
using HeroesMatchTracker.Core.ViewServices;
using HeroesMatchTracker.Data;
using Microsoft.Practices.ServiceLocation;
using NLog;
using System;
using System.Threading.Tasks;
using System.Windows;

namespace HeroesMatchTracker.Core.ViewModels.TitleBar
{
    public class AboutControlViewModel : ViewModelBase
    {
        private Logger ExceptionLog = LogManager.GetLogger(LogFileNames.Exceptions);
        private Logger WarningLog = LogManager.GetLogger(LogFileNames.WarningLogFileName);

        private string _checkForUpdatesResponse;
        private bool _isApplyUpdateButtonEnabled;
        private bool _isCheckForUpdatesButtonEnabled;

        private AutoUpdater AutoUpdater;
        private IDatabaseService Database;
        private IMainTabService MainTab;
        private IHeroesIconsService HeroesIcons;

        public AboutControlViewModel(IDatabaseService database, IMainTabService mainTab, IHeroesIconsService heroesIcons)
        {
            Database = database;
            MainTab = mainTab;
            HeroesIcons = heroesIcons;

            IsCheckForUpdatesButtonEnabled = true;
            PeriodicallyCheckUpdates();
        }

        public string HeroesMatchTrackerCoreVersion => AssemblyVersions.HeroesMatchTrackerCoreVersion().ToString();
        public string HeroesMatchTrackerDataVersion => AssemblyVersions.HeroesMatchTrackerDataVersion().ToString();
        public string HeroesMatchTrackerHelpersVersion => AssemblyVersions.HeroesHelpersVersion().ToString();
        public string HeroesIconsVersion => $"{AssemblyVersions.HeroesIconsVersion().ToString()} ({HeroesIcons.GetLatestHeroesBuild()})";
        public string HeroesReplayParserVersion => $"{AssemblyVersions.HeroesReplayParserVersion().ToString()} ({Heroes.ReplayParser.Replay.LatestSupportedBuild})";

        public RelayCommand CheckForUpdatesCommand => new RelayCommand(CheckForUpdates);
        public RelayCommand ApplyUpdateCommand => new RelayCommand(ApplyUpdates);

        public IToasterUpdateWindowService ToasterUpdateWindow => ServiceLocator.Current.GetInstance<IToasterUpdateWindowService>();

        public string CheckForUpdatesResponse
        {
            get => _checkForUpdatesResponse;
            set
            {
                _checkForUpdatesResponse = value;
                RaisePropertyChanged();
            }
        }

        public bool IsApplyUpdateButtonEnabled
        {
            get => _isApplyUpdateButtonEnabled;
            set
            {
                _isApplyUpdateButtonEnabled = value;
                RaisePropertyChanged();
            }
        }

        public bool IsCheckForUpdatesButtonEnabled
        {
            get => _isCheckForUpdatesButtonEnabled;
            set
            {
                _isCheckForUpdatesButtonEnabled = value;
                RaisePropertyChanged();
            }
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
                    AutoUpdater = new AutoUpdater(Database);
                    if (await AutoUpdater.CheckForUpdates())
                    {
                        CheckForUpdatesResponse = $"Update is available ({AutoUpdater.LatestVersionString})";
                        MainTab.SetExtendedAboutText(" (Update Available)");
                        IsApplyUpdateButtonEnabled = true;
                        Database.SettingsDb().UserSettings.IsUpdateAvailableKnown = true;
                    }
                    else
                    {
                        CheckForUpdatesResponse = "Heroes Match Tracker is up to date";
                    }
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
                    Database.SettingsDb().UserSettings.IsUpdateAvailableKnown = true;

                    // set both buttons to false and keep them false as a restart is required
                    IsCheckForUpdatesButtonEnabled = false;
                    IsApplyUpdateButtonEnabled = false;

                    CheckForUpdatesResponse = "Downloading and applying updates...";

                    await AutoUpdater.ApplyReleases();

                    CheckForUpdatesResponse = "Retreiving release notes...";
                    await AutoUpdater.RetrieveReleaseNotes(Database);

                    CheckForUpdatesResponse = $"Finished applying update ({AutoUpdater.LatestVersion.Major}.{AutoUpdater.LatestVersion.Minor}.{AutoUpdater.LatestVersion.Build}). A restart is required for changes to apply.";
                    MainTab.SetExtendedAboutText(" (Restart Required)");
                    Properties.Settings.Default.IsManualUpdateApplied = true;
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

                        AutoUpdater = new AutoUpdater(Database);
                        if (await AutoUpdater.CheckForUpdates())
                        {
                            CheckForUpdatesResponse = $"Update is available ({AutoUpdater.LatestVersionString})";
                            MainTab.SetExtendedAboutText(" (Update Available)");
                            IsApplyUpdateButtonEnabled = true;

                            await Application.Current.Dispatcher.InvokeAsync(() => ToasterUpdateWindow.ShowToaster(AssemblyVersions.HeroesMatchTrackerVersion().ToString(), AutoUpdater.LatestVersionString));
                        }
                        else
                        {
                            CheckForUpdatesResponse = string.Empty;
                        }
                    }
                    catch (Exception)
                    {
                        WarningLog.Log(LogLevel.Warn, $"Unable to periodically check for an update");
                    }

                    IsCheckForUpdatesButtonEnabled = true;
                }
            });
        }
    }
}
