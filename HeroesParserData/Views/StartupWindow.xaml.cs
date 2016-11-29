using HeroesIcons;
using HeroesParserData.Models.DbModels;
using NLog;
using System;
using System.IO;
using System.Threading.Tasks;
using System.Windows;

namespace HeroesParserData.Views
{
    /// <summary>
    /// Interaction logic for StartupWindow.xaml
    /// </summary>
    public partial class StartupWindow : Window
    {
        private Logger StartupLogFile = LogManager.GetLogger("StartupLogFile");
        private Logger DatabaseMigrateLog = LogManager.GetLogger("DatabaseMigrateLogFile");

        public StartupWindow()
        {
            InitializeComponent();
            AppVersion.Content = HPDVersion.GetVersionAsString();
        }

        protected override async void OnContentRendered(EventArgs e)
        {
            base.OnContentRendered(e);
            await InitializeMainWindow();
        }

        private async Task InitializeMainWindow()
        {
            try
            {
                StatusLabel.Content = "Starting up...";

                // order is important
                await Message("Performing database migration");
                await InitializeDatabase();

                await ApplicationUpdater();

                await Message("Initializing Hero Icons");
                App.HeroesInfo = HeroesInfo.Initialize();

                // must be last
                await Message("Initializing Heroes Parser Data");
                MainWindow mainWindow = new MainWindow();
                mainWindow.WindowState = WindowState.Maximized;

                // finished
                // ---------------------------------------------------------
                StatusLabel.Content = "Done";
                await Message("Starting Heroes Parser Data");

                Close();
                mainWindow.Show();

                await Task.Delay(500);

                if (UserSettings.Default.IsNewUpdateApplied)
                {
                    UserSettings.Default.IsNewUpdateApplied = false;
                    ReleaseNotesWindow releaseNotesWindow = new ReleaseNotesWindow();
                    releaseNotesWindow.ShowDialog();
                }
            }
            catch (Exception ex)
            {
                StatusLabel.Content = "An error was encountered. Check the error logs for details";
                StartupLogFile.Log(LogLevel.Error, ex);
                for (int i = 4; i >= 0; i--)
                {
                    CurrentStatusLabel.Content = $"Shutting down in ({i})...";
                    await Task.Delay(1000);
                }
                Application.Current.Shutdown();
            }
        }

        private async Task InitializeDatabase()
        {
            string applicationPath = AppDomain.CurrentDomain.BaseDirectory;
            string databasePath = Path.Combine(applicationPath, "Database");

            if (!Directory.Exists(databasePath))
                Directory.CreateDirectory(databasePath);

            AppDomain.CurrentDomain.SetData("DataDirectory", Directory.GetCurrentDirectory());
            await (new HeroesParserDataContext()).Initialize(DatabaseMigrateLog);

            if (App.NewDatabaseCreated)
            {
                UserSettings.Default.SetDefaultSettings();

                try
                {
                    ReleaseNoteHandler releaseNoteHandler = new ReleaseNoteHandler();
                    await releaseNoteHandler.InitializeClient();
                    releaseNoteHandler.AddAllReleasesUpToCurrentVersion();
                }
                catch (Exception ex)
                {
                    await Message($"Could not retreive Release notes: {ex.Message}");
                }
            }
        }

        private async Task ApplicationUpdater()
        {
            try
            {
            #if !DEBUG
                AutoUpdater autoUpdater = new AutoUpdater();

                await Message("Checking for updates...");

                if (!await autoUpdater.CheckForUpdates())
                {
                    await Message("Already latest version");
                    return;
                }

                if (!UserSettings.Default.IsAutoUpdates)
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
                await AutoUpdater.RetrieveReleaseNotes();

                await Message("Copying database to new folder...");
                AutoUpdater.CopyDatabaseToLatestRelease();

                await Message("Restarting application...");
                await Task.Delay(1000);

                System.Diagnostics.Process.Start(Path.Combine(App.NewLatestDirectory, "HeroesParserData.exe"));
                Application.Current.Shutdown();
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
            CurrentStatusLabel.Content = message;
            StartupLogFile.Log(LogLevel.Info, message);
            await Task.Delay(1);
        }
    }
}
