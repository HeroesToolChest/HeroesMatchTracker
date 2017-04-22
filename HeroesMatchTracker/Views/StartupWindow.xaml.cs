using GalaSoft.MvvmLight.Ioc;
using HeroesMatchTracker.Core.ViewModels;
using HeroesMatchTracker.Core.ViewServices;
using HeroesMatchTracker.Data;
using System;
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

            SimpleIoc.Default.Register<IMainWindowService>(() => this);
        }

        public void CreateMainWindow()
        {
            MainWindow mainWindow = new MainWindow();

            if (Database.SettingsDb().UserSettings.IsStartedViaStartup)
                mainWindow.WindowState = WindowState.Minimized;
            else
                mainWindow.WindowState = WindowState.Maximized;

            mainWindow.Show();
            Close();
        }

        private void SetCommandLineArgs()
        {
            string[] args = Environment.GetCommandLineArgs();

            if (args != null && args.Length > 0)
            {
                if (args[0] == "/noshow")
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
