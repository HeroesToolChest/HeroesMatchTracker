using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using Heroes.Icons;
using HeroesStatTracker.Core.ViewServices;
using HeroesStatTracker.Data;
using Microsoft.Practices.ServiceLocation;
using NLog;
using System;
using System.Threading.Tasks;
using System.Windows;

namespace HeroesStatTracker.Core.ViewModels
{
    public class StartupWindowViewModel: ViewModelBase
    {
        private Logger StartupLogFile = LogManager.GetLogger(LogFileNames.StartupLogFileName);

        private string _statusLabel;
        private string _detailedStatusLabel;

        public string AppVersion { get { return AssemblyVersions.HeroesStatTrackerVersion().ToString(); } }

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

        /// <summary>
        /// Constructor
        /// </summary>
        public StartupWindowViewModel()
        {

        }

        private async Task ExecuteStartup()
        {
            try
            {
                StatusLabel = "Starting up...";

                await Message("Initializing HeroesStatTracker.Data");
                Database.Initialize().ExecuteDatabaseMigrations();

                // auto update stuff

                await Message("Initializing Heroes.Icons");
                CoreGlobal.HeroesIcons = HeroesIcons.Initialize(true);

                await Message("Initializing Heroes Stat Tracker");
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

        private async Task Message(string message)
        {
            DetailedStatusLabel = message;
            StartupLogFile.Log(LogLevel.Info, message);
            await Task.Delay(1);
        }
    }
}
