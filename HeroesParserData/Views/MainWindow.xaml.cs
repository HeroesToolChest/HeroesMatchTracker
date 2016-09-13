using GalaSoft.MvvmLight.Messaging;
using HeroesParserData.Messages;
using HeroesParserData.Properties;
using MahApps.Metro.Controls.Dialogs;
using NLog;
using Squirrel;
using System;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows;

namespace HeroesParserData.Views
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        private UpdateManager updateManager;
        private static Logger UpdaterLog = LogManager.GetLogger("UpdateLogFile");

        public MainWindow()
        {
            Application.Current.MainWindow.WindowState = WindowState.Maximized;
            SetTrayIcon();

            InitializeComponent();

            if (App.MigrateFailed)
                ErrorMessage();

        #if !DEBUG
            Task.Run(async () =>
            {
                await Task.Delay(4000);
                await AutoUpdateCheck();
            });
        #endif
        }

        private async Task AutoUpdateCheck()
        {
            for (;;)
            {
                try
                {
                    if (Settings.Default.IsAutoUpdates)
                    {
                        updateManager = await UpdateManager.GitHubUpdateManager(Properties.Settings.Default.UpdateUrl);
                        UpdaterLog.Log(LogLevel.Info, "Update Check");

                        if (updateManager != null)
                        {
                            UpdateInfo update = await updateManager.CheckForUpdate();

                            if (update != null)
                            {
                                string current = update.CurrentlyInstalledVersion != null ? update.CurrentlyInstalledVersion.Version.ToString() : string.Empty;
                                string latest = update.FutureReleaseEntry != null ? update.FutureReleaseEntry.Version.ToString() : string.Empty;

                                UpdaterLog.Log(LogLevel.Info, $"Current Version: {current}");
                                UpdaterLog.Log(LogLevel.Info, $"Latest Version: {latest}");

                                if (!string.IsNullOrEmpty(current) && current != latest)
                                {
                                    UpdaterLog.Log(LogLevel.Info, "Downloading...");
                                    await updateManager.DownloadReleases(update.ReleasesToApply);
                                    UpdaterLog.Log(LogLevel.Info, "Downloading...Completed");

                                    UpdaterLog.Log(LogLevel.Info, "Applying releases");
                                    string directoryPath = await updateManager.ApplyReleases(update);

                                    App.UpdateInProgress = true;
                                    App.NewLatestDirectory = directoryPath;

                                    UpdaterLog.Log(LogLevel.Info, $"New directory path: {directoryPath}");

                                    await Application.Current.Dispatcher.InvokeAsync(async delegate
                                    {
                                        await this.ShowMessageAsync($"Application updated to {latest}",
                                            "When you're ready to complete the update, please close the application and reopen.", MessageDialogStyle.Affirmative);
                                    });
                                }
                                else
                                    UpdaterLog.Log(LogLevel.Info, "Have latest already. No update needed. ");
                            }
                            else
                                UpdaterLog.Log(LogLevel.Info, "No update needed");
                        }
                        else
                            UpdaterLog.Log(LogLevel.Info, $"No updates found at {Properties.Settings.Default.UpdateUrl}");
                    }
                }
                catch (Exception ex)
                {
                    UpdaterLog.Log(LogLevel.Info, ex.Message);
                    UpdaterLog.Log(LogLevel.Info, ex.StackTrace);
                }

                await Task.Delay(360000000); // 1 hour
            }
        }

        private async void ErrorMessage()
        {
            await Task.Delay(2000);
            await this.ShowMessageAsync($"Update Error", string.Concat("There was an error with the database migration. Please check logs/DatabaseMigrateLog.txt for more information.\n\n",
                "If this application underwent an update, then you may encounter errors throughout its use, otherwise you may continue using the application normally and ignore this error."), 
                MessageDialogStyle.Affirmative);
        }

        private void SettingsButton_Click(object sender, RoutedEventArgs e)
        {
            SettingsFlyout.IsOpen = true;
        }

        private void SetTrayIcon()
        {
            // menu items
            var menuItem1 = new System.Windows.Forms.MenuItem();
            var menuItem2 = new System.Windows.Forms.MenuItem();

            // context menu
            var contextMenu = new System.Windows.Forms.ContextMenu();
            contextMenu.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] { menuItem1, menuItem2 });

            // menu items
            menuItem1.Index = 0;
            menuItem1.Text = "Open";
            menuItem1.Click += (Sender, e) =>
            {
                Show();
                WindowState = WindowState.Maximized;
            };

            menuItem2.Index = 1;
            menuItem2.Text = "Exit";
            menuItem2.Click += (Sender, e) =>
            {
                Application.Current.Shutdown();
            };



            App.NotifyIcon = new System.Windows.Forms.NotifyIcon
            {
                Icon = System.Drawing.Icon.ExtractAssociatedIcon(Assembly.GetExecutingAssembly().Location),
                Visible = false,
                ContextMenu = contextMenu
            };
            App.NotifyIcon.DoubleClick += (sender, e) =>
            {
                Show();
                WindowState = WindowState.Maximized;
            };
        }

        protected override void OnStateChanged(EventArgs e)
        {
            if (Settings.Default.IsMinimizeToTray && WindowState == WindowState.Minimized)
            { 
                Hide();
                App.NotifyIcon.Visible = true;
            }

            base.OnStateChanged(e);
        }
    }
}
