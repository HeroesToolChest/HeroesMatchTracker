using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using HeroesMatchTracker.Core.Updater;
using HeroesMatchTracker.Core.ViewServices;
using HeroesMatchTracker.Data;
using Microsoft.Practices.ServiceLocation;
using Microsoft.Win32;
using NLog;
using System;
using System.Threading.Tasks;
using System.Windows;

namespace HeroesMatchTracker.Core.ViewModels
{
    public class StartupWindowViewModel : ViewModelBase
    {
        private const string DotNetSubKey = @"SOFTWARE\Microsoft\NET Framework Setup\NDP\v4\Full\";

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

        public string AppVersion { get { return AssemblyVersions.HeroesMatchTrackerVersion().ToString(); } }

        public IDatabaseService GetDatabaseService => Database;

        public RelayCommand ExecuteStartupCommand => new RelayCommand(async () => await ExecuteStartup());

        public string StatusLabel
        {
            get => _statusLabel;
            set
            {
                _statusLabel = value;
                RaisePropertyChanged(nameof(StatusLabel));
            }
        }

        public string DetailedStatusLabel
        {
            get => _detailedStatusLabel;
            set
            {
                _detailedStatusLabel = value;
                RaisePropertyChanged(nameof(DetailedStatusLabel));
            }
        }

        public IMainWindowService StartupWindowService => ServiceLocator.Current.GetInstance<IMainWindowService>();

        private async Task ExecuteStartup()
        {
            try
            {
                await Message("Starting up...");

                GetSystemInformation();

                await Message("Initializing HeroesMatchTracker.Data");
                var databaseMigrations = Data.Database.Initialize().ExecuteDatabaseMigrations();
                await Message("Performing Database migrations...");
                await databaseMigrations;

#if !DEBUG
                await ApplicationUpdater();
#endif

                await Message("Initializing Heroes Match Tracker");
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
                AutoUpdater autoUpdater = new AutoUpdater(Database);

                await Message("Checking for updates...");

                if (!await autoUpdater.CheckForUpdates())
                {
                    await Message("Already latest version");

                    // make sure we have up to date release notes
                    await Message("Retrieving release notes...");
                    await AutoUpdater.RetrieveReleaseNotes(Database);
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

                await Message("Restarting application...");
                await Task.Delay(1000);

                if (Database.SettingsDb().UserSettings.IsWindowsStartup && Database.SettingsDb().UserSettings.IsStartedViaStartup)
                    autoUpdater.RestartApp(arguments: "/noshow /updated");
                else
                    autoUpdater.RestartApp(arguments: "/updated");
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

        private void GetSystemInformation()
        {
            StartupLogFile.Log(LogLevel.Info, "----------------------------------------------------------------");
            GetOperatingSystemInfo();
            GetDotNetRuntimeVersion();
            StartupLogFile.Log(LogLevel.Info, "----------------------------------------------------------------");
            StartupLogFile.Log(LogLevel.Info, string.Empty);
        }

        private void GetOperatingSystemInfo()
        {
            StartupLogFile.Log(LogLevel.Info, $"            OS Version: {Environment.OSVersion}");

            OperatingSystem os = Environment.OSVersion;
            Version version = os.Version;

            string operatingSystem = string.Empty;
            if (os.Platform == PlatformID.Win32NT)
            {
                switch (version.Major)
                {
                    case 3:
                        operatingSystem = "NT 3.51";
                        break;
                    case 4:
                        operatingSystem = "NT 4.0";
                        break;
                    case 5:
                        if (version.Minor == 0)
                            operatingSystem = "2000";
                        else if (version.Minor == 1)
                            operatingSystem = "XP";
                        else
                            operatingSystem = "XP 64-Bit Edition"; // Server 2003/Server 2003 R2
                        break;
                    case 6:
                        if (version.Minor == 0)
                            operatingSystem = "Vista"; // Server 2008
                        else if (version.Minor == 1)
                            operatingSystem = "7"; // Server 2008 R2
                        else if (version.Minor == 2)
                            operatingSystem = "8"; // Server 2012
                        else
                            operatingSystem = "8.1"; // Server 2012 R2
                        break;
                    case 10:
                        operatingSystem = "10"; // Server 2016
                        break;
                    default:
                        break;
                }

                operatingSystem = $"Windows {operatingSystem}";

                // service pack
                if (!string.IsNullOrEmpty(os.ServicePack))
                    operatingSystem = $"{operatingSystem} {os.ServicePack}";

                // architecture
                if (Environment.Is64BitOperatingSystem)
                    operatingSystem = $"{operatingSystem} 64-bit";
                else
                    operatingSystem = $"{operatingSystem} 32-bit";

                StartupLogFile.Log(LogLevel.Info, $"      Operating System: {operatingSystem}");
            }
            else if (os.Platform == PlatformID.Win32Windows)
            {
                StartupLogFile.Log(LogLevel.Info, "      Operating System: Pre-Vista OS");
            }
        }

        private void GetDotNetRuntimeVersion()
        {
            string dotNetFrameWorkVersion = ".NET Framework Version: ";
            using (RegistryKey ndpKey = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry32).OpenSubKey(DotNetSubKey))
            {
                if (ndpKey != null && ndpKey.GetValue("Release") != null)
                {
                    int releaseKey = (int)ndpKey.GetValue("Release");

                    if (releaseKey >= 460798)
                        dotNetFrameWorkVersion += "4.7 or later";
                    else if (releaseKey >= 394802)
                        dotNetFrameWorkVersion += "4.6.2";
                    else if (releaseKey >= 394254)
                        dotNetFrameWorkVersion += "4.6.1";
                    else if (releaseKey >= 393295)
                        dotNetFrameWorkVersion += "4.6";
                    else if (releaseKey >= 379893)
                        dotNetFrameWorkVersion += "4.5.2";
                    else if (releaseKey >= 378675)
                        dotNetFrameWorkVersion += "4.5.1";
                    else if (releaseKey >= 378389)
                        dotNetFrameWorkVersion += "4.5";
                    else
                        dotNetFrameWorkVersion += "No 4.5 or later version detected";
                }
                else
                {
                    dotNetFrameWorkVersion += "No 4.5 or later version detected";
                }
            }

            StartupLogFile.Log(LogLevel.Info, dotNetFrameWorkVersion);
        }
    }
}
