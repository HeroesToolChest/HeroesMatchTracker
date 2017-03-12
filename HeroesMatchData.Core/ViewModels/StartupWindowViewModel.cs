using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using HeroesMatchData.Core.Updater;
using HeroesMatchData.Core.ViewServices;
using HeroesMatchData.Data;
using Microsoft.Practices.ServiceLocation;
using NLog;
using System;
using System.Threading.Tasks;
using System.Windows;

namespace HeroesMatchData.Core.ViewModels
{
    public class StartupWindowViewModel : ViewModelBase
    {
        private Logger StartupLogFile = LogManager.GetLogger(LogFileNames.StartupLogFileName);

        private string _statusLabel;
        private string _detailedStatusLabel;

        private IDatabaseService Database;

        /// <summary>
        /// Constructor
        /// </summary>
        public StartupWindowViewModel(IDatabaseService database)
        {
            Database = database;
        }

        public string AppVersion { get { return AssemblyVersions.HeroesMatchDataVersion().ToString(); } }

        public RelayCommand ExecuteStartupCommand => new RelayCommand(async () => await ExecuteStartup());

        public string StatusLabel
        {
            get { return _statusLabel; }
            set
            {
                _statusLabel = value;
                RaisePropertyChanged(nameof(StatusLabel));
            }
        }

        public string DetailedStatusLabel
        {
            get { return _detailedStatusLabel; }
            set
            {
                _detailedStatusLabel = value;
                RaisePropertyChanged(nameof(DetailedStatusLabel));
            }
        }

        public IMainWindowService StartupWindowService
        {
            get { return ServiceLocator.Current.GetInstance<IMainWindowService>(); }
        }

        private async Task ExecuteStartup()
        {
            try
            {
                StatusLabel = "Starting up...";

                await Message("Initializing HeroesMatchData.Data");
                Data.Database.Initialize().ExecuteDatabaseMigrations();

                await ApplicationUpdater();

                await Message("Initializing Heroes Match Data");
                StartupWindowService.CreateMainWindow(); // create the main application window
            }
            catch (Exception ex)
            {
                StatusLabel = "An error was encountered. Check the error logs for details";
                StartupLogFile.Log(LogLevel.Error, ex);

                for (int i = 4; i >= 0; i--)
                {
                    DetailedStatusLabel = $"Shutting down in ({i})...";
                    await Task.Delay(1000);
                }

                Application.Current.Shutdown();
            }
        }

        private async Task ApplicationUpdater()
        {
            try
            {
#if !DEBUG
                AutoUpdater autoUpdater = new AutoUpdater(Database);

                await Message("Checking for updates...");

                if (!await autoUpdater.CheckForUpdates())
                {
                    await Message("Already latest version");
                    return;
                }

                if (!Database.SettingsDb().UserSettings.IsAutoUpdates)
                {
                    await Message("Update available, auto-update is disabled");
                    return;
                }

                await Message("Downloading and applying releases...");
                if (!await autoUpdater.ApplyReleases())
                {
                    await Message("Already latest version");
                    return;
                }

                await Message("Retrieving release notes...");
                await AutoUpdater.RetrieveReleaseNotes(Database);

                await Message("Copying database to new folder...");
                AutoUpdater.CopyDatabasesToLatestRelease();

                await Message("Restarting application...");
                await Task.Delay(1000);

                autoUpdater.RestartApp();
#endif
            }
            catch (AutoUpdaterException ex)
            {
                await Message("Could not check for updates or apply releases, check logs");
                StartupLogFile.Log(LogLevel.Error, ex);
                await Task.Delay(1000);
            }
        }

        private async Task Message(string message)
        {
            DetailedStatusLabel = message;
            StartupLogFile.Log(LogLevel.Info, message);
            await Task.Delay(1);
        }
    }
}
