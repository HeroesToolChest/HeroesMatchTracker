using HeroesParserData.Database;
using HeroesParserData.Properties;
using System;
using System.IO;
using System.Windows;

namespace HeroesParserData
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
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
    }
}
