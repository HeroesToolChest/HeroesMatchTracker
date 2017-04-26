using GalaSoft.MvvmLight.Ioc;
using HeroesMatchTracker.Core.ViewModels;
using HeroesMatchTracker.Core.ViewServices;
using HeroesMatchTracker.Data;
using System;
using System.IO;
using System.Linq;
using System.Windows;

namespace HeroesMatchTracker.Views
{
    /// <summary>
    /// Interaction logic for StartupWindow.xaml
    /// </summary>
    public partial class StartupWindow : Window, IMainWindowService
    {
        private StartupWindowViewModel StartupWindowViewModel;
        private IDatabaseService Database;

        public StartupWindow()
        {
            InitializeComponent();

            StartupWindowViewModel = (StartupWindowViewModel)DataContext;
            Database = StartupWindowViewModel.GetDatabaseService;

            SetCommandLineArgs();

            SimpleIoc.Default.Register<IMainWindowService>(() => this);
        }

        public void CreateMainWindow()
        {
            MainWindow mainWindow = new MainWindow();

            if (Database.SettingsDb().UserSettings.IsStartedViaStartup)
            {
                mainWindow.WindowState = WindowState.Minimized;
                mainWindow.ShowInTaskbar = false;
                mainWindow.Hide();
                App.NotifyIcon.Visible = true;
            }
            else
            {
                mainWindow.WindowState = WindowState.Maximized;
                mainWindow.Show();
            }

            Close();
        }

        private void SetCommandLineArgs()
        {
            AppDomain.CurrentDomain.SetData("DataDirectory", Data.Database.DatabasePath);

            if (!File.Exists(Path.Combine(Data.Database.DatabasePath, Data.Database.ReleaseNotesDbFileName)))
                return;

            string[] args = Environment.GetCommandLineArgs();

            if (args != null && args.Length > 0)
            {
                if (args.ToList().Contains("/noshow"))
                {
                    ShowInTaskbar = false;
                    Visibility = Visibility.Hidden;

                    Database.SettingsDb().UserSettings.IsStartedViaStartup = true;
                    return;
                }
            }

            Database.SettingsDb().UserSettings.IsStartedViaStartup = false;
        }
    }
}
