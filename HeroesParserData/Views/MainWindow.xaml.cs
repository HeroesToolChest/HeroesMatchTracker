using MahApps.Metro.Controls.Dialogs;
using NLog;
using Squirrel;
using System;
using System.IO;
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

            InitializeComponent();

        #if !DEBUG
            Task.Run(async () =>
            {
                await Task.Delay(2000);
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
                            UpdaterLog.Log(LogLevel.Info, $"Lastest Version: {latest}");

                            if (!string.IsNullOrEmpty(current) && current != latest)
                            {
                                UpdaterLog.Log(LogLevel.Info, "Downloading...");
                                await updateManager.DownloadReleases(update.ReleasesToApply);
                                UpdaterLog.Log(LogLevel.Info, "Downloading...Completed");

                                UpdaterLog.Log(LogLevel.Info, "Applying releases");
                                string directoryPath = await updateManager.ApplyReleases(update);

                                App.UpdateInProgress = true;
                                App.NewLastestDirectory = directoryPath;

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
                catch (Exception ex)
                {
                    UpdaterLog.Log(LogLevel.Info, ex.Message);
                    UpdaterLog.Log(LogLevel.Info, ex.StackTrace);
                }

                await Task.Delay(360000000); // 1 hour
            }
        }
    }
}
