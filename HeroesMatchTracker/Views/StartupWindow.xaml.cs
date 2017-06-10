using GalaSoft.MvvmLight.Ioc;
using GalaSoft.MvvmLight.Messaging;
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

            if (Database.SettingsDb().UserSettings.RequeueAllFailedReplays && Database.SettingsDb().UserSettings.IsAutoRequeueOnUpdate)
            {
                var unparsedReplaysList = Database.SettingsDb().FailedReplays.ReadAllReplays();
                Database.SettingsDb().FailedReplays.DeleteAllFailedReplays();
                Messenger.Default.Send(unparsedReplaysList);

                Database.SettingsDb().UserSettings.RequeueAllFailedReplays = false;
            }

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
                var argsList = args.ToList();

                if (argsList.Contains("/noshow"))
                {
                    ShowInTaskbar = false;
                    Visibility = Visibility.Hidden;

                    Database.SettingsDb().UserSettings.IsStartedViaStartup = true;
                }
                else
                {
                    Database.SettingsDb().UserSettings.IsStartedViaStartup = false;
                }

                if (argsList.Contains("/updated"))
                {
                    Database.SettingsDb().UserSettings.ShowWhatsNewWindow = true;
                    Database.SettingsDb().UserSettings.RequeueAllFailedReplays = true;
                }
                else
                {
                    Database.SettingsDb().UserSettings.ShowWhatsNewWindow = false;
                    Database.SettingsDb().UserSettings.RequeueAllFailedReplays = false;
                }
            }
        }
    }
}
