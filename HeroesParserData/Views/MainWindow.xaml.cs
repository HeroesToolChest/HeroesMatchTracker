using MahApps.Metro.Controls.Dialogs;
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

        public MainWindow()
        {
            Application.Current.MainWindow.WindowState = WindowState.Maximized;

            InitializeComponent();

        #if !DEBUG
            Task.Run(async () =>
            {
                await Task.Delay(3000);
                await AutoUpdateCheck();
            });
        #endif
        }

        private async Task AutoUpdateCheck()
        {
            for (;;)
            {
                using (StreamWriter writer = new StreamWriter("_UpdateLog.txt", true))
                {
                    try
                    {
                        updateManager = await UpdateManager.GitHubUpdateManager(Properties.Settings.Default.UpdateUrl);
                        writer.WriteLine(LogWriteLine("Update Check"));

                        if (updateManager != null)
                        {
                            UpdateInfo update = await updateManager.CheckForUpdate();

                            if (update != null)
                            {
                                string current = update.CurrentlyInstalledVersion != null ? update.CurrentlyInstalledVersion.Version.ToString() : string.Empty;
                                string latest = update.FutureReleaseEntry != null ? update.FutureReleaseEntry.Version.ToString() : string.Empty;

                                writer.WriteLine(LogWriteLine($"Current Version: {current}"));
                                writer.WriteLine(LogWriteLine($"Lastest Version: {latest}"));

                                if (!string.IsNullOrEmpty(current) && current != latest)
                                {
                                    writer.WriteLine(LogWriteLine("Downloading..."));
                                    await updateManager.DownloadReleases(update.ReleasesToApply);
                                    writer.WriteLine(LogWriteLine("Downloading...Completed"));

                                    writer.WriteLine(LogWriteLine("Applying releases"));
                                    string directoryPath = await updateManager.ApplyReleases(update);

                                    App.UpdateInProgress = true;
                                    App.NewLastestDirectory = directoryPath;

                                    writer.WriteLine(LogWriteLine($"New directory path: {directoryPath}"));

                                    await Application.Current.Dispatcher.InvokeAsync(async delegate
                                    {
                                        await this.ShowMessageAsync($"Application updated to {latest}",
                                            "When you're ready to complete the update, please close the application and reopen.", MessageDialogStyle.Affirmative);
                                    });
                                }
                                else
                                {
                                    writer.WriteLine(LogWriteLine("Have latest already. No update needed. "));
                                }
                            }
                            else
                            {
                                writer.WriteLine(LogWriteLine("No update needed"));
                            }
                        }
                        else
                        {
                            writer.WriteLine(LogWriteLine($"No updates found at {Properties.Settings.Default.UpdateUrl}"));
                        }
                    }
                    catch (Exception ex)
                    {
                        writer.WriteLine(LogWriteLine(ex.Message));
                        writer.WriteLine(LogWriteLine(ex.StackTrace));
                    }

                    writer.WriteLine();
                }

                await Task.Delay(360000000); // 1 hour
            }
        }

        private string LogWriteLine(string line)
        {
            return $"[{DateTime.Now}] {line}";
        }
    }
}
