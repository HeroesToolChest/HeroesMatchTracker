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
    public partial class StartupWindow
    {
        private Logger StartupLogFile = LogManager.GetLogger("StartupLogFile");
        private Logger DatabaseMigrateLog = LogManager.GetLogger("DatabaseMigrateLogFile");

        public StartupWindow()
        {
            InitializeComponent();
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
                await Task.Delay(100);

                // order is important
                CurrentStatusLabel.Content = "Initializing Hero Icons";
                StartupLogFile.Log(LogLevel.Info, "Initializing Hero Icons");
                await Task.Delay(100);
                App.HeroesInfo = HeroesInfo.Initialize();

                CurrentStatusLabel.Content = "Performing database migration";
                StartupLogFile.Log(LogLevel.Info, "Performing database migration");
                await Task.Delay(100);
                InitializeDatabase();

                // must be last
                CurrentStatusLabel.Content = "Initializing Heroes Parser Data";
                StartupLogFile.Log(LogLevel.Info, "Initializing Heroes Parser Data");
                await Task.Delay(100);
                MainWindow mainWindow = new MainWindow();
                mainWindow.WindowState = WindowState.Maximized;

                // finished
                // ---------------------------------------------------------
                StatusLabel.Content = "Done";
                CurrentStatusLabel.Content = "Starting Heroes Parser Data";
                StartupLogFile.Log(LogLevel.Info, "Starting Heroes Parser Data");
                await Task.Delay(500);

                Close();
                mainWindow.Show();
            }
            catch (Exception ex)
            {
                StatusLabel.Content = "An error was encountered. Check the error logs for details";
                await Task.Delay(100);
                StartupLogFile.Log(LogLevel.Error, ex);
            }
        }

        private void InitializeDatabase()
        {
            string applicationPath = AppDomain.CurrentDomain.BaseDirectory;
            string databasePath = Path.Combine(applicationPath, "Database");

            if (!Directory.Exists(databasePath))
                Directory.CreateDirectory(databasePath);

            AppDomain.CurrentDomain.SetData("DataDirectory", Directory.GetCurrentDirectory());
            (new HeroesParserDataContext()).Initialize(DatabaseMigrateLog);
        }
    }
}
