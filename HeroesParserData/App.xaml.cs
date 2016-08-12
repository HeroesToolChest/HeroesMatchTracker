using HeroesParserData.Database;
using HeroesParserData.Properties;
using System;
using System.Data.SqlClient;
using System.IO;
using System.Windows;

namespace HeroesParserData
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static bool UpdateInProgress { get; set; }
        public static string NewLastestDirectory { get; set; }

        protected override void OnStartup(StartupEventArgs e)
        {
            UpdateInProgress = false;

            InitDatabase();

            // set default location
            if (string.IsNullOrEmpty(Settings.Default.ReplaysLocation))
                Settings.Default.ReplaysLocation = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), @"Heroes of the Storm\Accounts");

            // add custom accent and theme resource dictionaries
            //ThemeManager.AddAccent("Theme", new Uri("pack://application:,,,/Resources/Theme.xaml"));

            //// get the theme from the current application
            //var theme = ThemeManager.DetectAppStyle(Application.Current);

            //// now use the custom accent
            //ThemeManager.ChangeAppStyle(Application.Current,
            //                        ThemeManager.GetAccent("Theme"),
            //                        theme.Item1);

            base.OnStartup(e);
        }

        protected override void OnExit(ExitEventArgs e)
        {
            Settings.Default.Save();

            if (UpdateInProgress && !string.IsNullOrEmpty(NewLastestDirectory))
            {
                SqlConnection.ClearAllPools();
            }

            base.OnExit(e);
        }

        private void InitDatabase()
        {
            string applicationPath = AppDomain.CurrentDomain.BaseDirectory;
            string databasePath = Path.Combine(applicationPath, "Database");

            if (!Directory.Exists(databasePath))
                Directory.CreateDirectory(databasePath);
                
            AppDomain.CurrentDomain.SetData("DataDirectory", Directory.GetCurrentDirectory());
            HeroesParserDataInit.InitiliazeHeroesParserDataStore();
        }

        private void CopyDatabaseToLatestRelease()
        {
            string ldfFile = "HeroesParserData_log.ldf";
            string mdfFile = "HeroesParserData.mdf";
            string ldfFilePath = @"Database\HeroesParserData_log.ldf";
            string mdfFilePath = @"Database\HeroesParserData.mdf";
            string newAppDirectory = Path.Combine(NewLastestDirectory, "Database");

            using (StreamWriter writer = new StreamWriter("_DatabaseCopyLog.txt", true))
            {
                try
                {
                    if (!File.Exists(ldfFilePath) && !File.Exists(mdfFilePath))
                    {
                        writer.WriteLine($"Database file not found: {ldfFilePath}, {mdfFilePath}");
                        writer.WriteLine("Nothing to copy, update completed");

                        return;
                    }

                    Directory.CreateDirectory(newAppDirectory);
                    writer.WriteLine($"Directory created: {newAppDirectory}");

                    File.Copy(ldfFilePath, Path.Combine(newAppDirectory, ldfFile));
                    File.Copy(mdfFilePath, Path.Combine(newAppDirectory, mdfFile));

                    writer.WriteLine($"Database file copied to: {Path.Combine(newAppDirectory, ldfFile)}");
                    writer.WriteLine($"Database file copied to: {Path.Combine(newAppDirectory, mdfFile)}");
                    writer.WriteLine("Update completed");
                }
                catch (Exception ex)
                {
                    writer.WriteLine(ex);
                }
            }
        }
    }
}
