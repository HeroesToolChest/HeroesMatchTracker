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
        private UpdateManager UpdateManager;
        private static Logger UpdaterLog = LogManager.GetLogger("UpdateLogFile");
        private string CurrentVersion;
        private string LatestVersion;

        public MainWindow()
        {
            SetTrayIcon();

            InitializeComponent();

        #if !DEBUG
            Task.Run(async () =>
            {
                await Task.Delay(4000);
                await AutoUpdateCheck();
            });
        #endif
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

        private async Task AutoUpdateCheck()
        {
            for (;;)
            {
                try
                {
                    UpdateInfo update = await CheckUpdates();

                    if (update != null)
                    {
                        if (Settings.Default.IsAutoUpdates)
                        {
                            await PerformUpdates(update, LatestVersion, CurrentVersion);
                        }
                    }
                    else
                        UpdaterLog.Log(LogLevel.Info, "Have latest already. No update needed. ");
                }
                catch (Exception ex)
                {
                    UpdaterLog.Log(LogLevel.Info, ex.Message);
                    UpdaterLog.Log(LogLevel.Info, ex.StackTrace);
                }
                finally
                {
                    await Task.Delay(360000000); // 1 hour
                }
            }
        }

        private async Task<UpdateInfo> CheckUpdates()
        {
            UpdateManager = await UpdateManager.GitHubUpdateManager(Settings.Default.UpdateUrl);
            UpdaterLog.Log(LogLevel.Info, "Update Check");

            UpdateInfo update = await UpdateManager.CheckForUpdate();

            CurrentVersion = update.CurrentlyInstalledVersion != null ? update.CurrentlyInstalledVersion.Version.ToString() : string.Empty;
            LatestVersion = update.FutureReleaseEntry != null ? update.FutureReleaseEntry.Version.ToString() : string.Empty;

            UpdaterLog.Log(LogLevel.Info, $"Current Version: {CurrentVersion}");
            UpdaterLog.Log(LogLevel.Info, $"Latest Version: {LatestVersion}");

            if (!string.IsNullOrEmpty(CurrentVersion) && CurrentVersion != LatestVersion)
                return update;
            else
                return null;
        }

        private async Task PerformUpdates(UpdateInfo update, string latest, string current)
        {
            UpdaterLog.Log(LogLevel.Info, "Downloading...");
            await UpdateManager.DownloadReleases(update.ReleasesToApply);
            UpdaterLog.Log(LogLevel.Info, "Downloading...Completed");

            UpdaterLog.Log(LogLevel.Info, "Applying releases");
            string directoryPath = await UpdateManager.ApplyReleases(update);

            App.UpdateInProgress = true;
            App.NewLatestDirectory = directoryPath;

            UpdaterLog.Log(LogLevel.Info, $"New directory path: {directoryPath}");

            await Application.Current.Dispatcher.InvokeAsync(async delegate
            {
                await this.ShowMessageAsync($"Application updated to {latest} from {current}",
                    "When you're ready to complete the update, please close the application and reopen.", MessageDialogStyle.Affirmative);
            });
        }

        private void SettingsButton_Click(object sender, RoutedEventArgs e)
        {
            if (SettingsFlyout.IsOpen)
                SettingsFlyout.IsOpen = false;
            else
                SettingsFlyout.IsOpen = true;
        }

        private void SetTrayIcon()
        {
            Version version = Assembly.GetEntryAssembly().GetName().Version;

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
                ContextMenu = contextMenu,
                Text = $"Heroes Parser Data {version.Major}.{version.Minor}.{version.Build}"
            };
            App.NotifyIcon.DoubleClick += (sender, e) =>
            {
                Show();
                WindowState = WindowState.Maximized;
            };
        }
    }
}
