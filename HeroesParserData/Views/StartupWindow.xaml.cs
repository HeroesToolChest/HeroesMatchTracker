using HeroesIcons;
using HeroesParserData.Models.DbModels;
using HeroesParserData.Properties;
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
        private Logger DatabaseCopyLog = LogManager.GetLogger("DatabaseCopyLogFile");

        public StartupWindow()
        {
            InitializeComponent();
            AppVersion.Content = HPDVersion.GetVersion();
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
                await ApplicationUpdater();

                await Message("Initializing Hero Icons");
                App.HeroesInfo = HeroesInfo.Initialize();

                await Message("Performing database migration");
                await InitializeDatabase();

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
            }
            catch (Exception ex)
            {
                await Message("An error was encountered. Check the error logs for details");
                StartupLogFile.Log(LogLevel.Error, ex);
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

                if (!Settings.Default.IsAutoUpdates)
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

                await Message("Copying database to new folder...");
                CopyDatabaseToLatestRelease();

                await Message("Restarting application...");
                await Task.Delay(1000);

                System.Diagnostics.Process.Start(Application.ResourceAssembly.Location);
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

        private void CopyDatabaseToLatestRelease()
        {
            string dbFile = Settings.Default.DatabaseFile;
            string dbFilePath = $@"Database\{dbFile}";
            string newAppDirectory = Path.Combine(App.NewLatestDirectory, "Database");

            try
            {
                if (!File.Exists(dbFilePath))
                {
                    DatabaseCopyLog.Log(LogLevel.Info, $"Database file not found: {dbFilePath}");
                    DatabaseCopyLog.Log(LogLevel.Info, "Nothing to copy, update completed");

                    return;
                }

                Directory.CreateDirectory(newAppDirectory);
                DatabaseCopyLog.Log(LogLevel.Info, $"Directory created: {newAppDirectory}");

                File.Copy(dbFilePath, Path.Combine(newAppDirectory, dbFile));

                DatabaseCopyLog.Log(LogLevel.Info, $"Database file copied to: {Path.Combine(newAppDirectory, dbFile)}");
                DatabaseCopyLog.Log(LogLevel.Info, "Update completed");
            }
            catch (Exception ex)
            {
                DatabaseCopyLog.Log(LogLevel.Info, ex);
                throw;
            }
        }

        private async Task Message(string message)
        {
            CurrentStatusLabel.Content = message;
            StartupLogFile.Log(LogLevel.Info, message);
            await Task.Delay(100);
        }
    }
}
