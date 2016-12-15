using GalaSoft.MvvmLight.Messaging;
using HeroesParserData.Messages;
using MahApps.Metro.Controls.Dialogs;
using System;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Interop;

namespace HeroesParserData.Views
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        public MainWindow()
        {
            SetTrayIcon();
            Messenger.Default.Register<StatisticsTabMessage>(this, async (action) => await ReceiveMessage(action));
            Messenger.Default.Register<MatchSummaryMessage>(this, (action) => ReceiveMessage(action));

            InitializeComponent();
        }

        protected override void OnStateChanged(EventArgs e)
        {
            if (UserSettings.Default.IsMinimizeToTray && WindowState == WindowState.Minimized)
            {
                Hide();
                App.NotifyIcon.Visible = true;
            }

            base.OnStateChanged(e);
        }

        protected override void OnSourceInitialized(EventArgs e)
        {
            base.OnSourceInitialized(e);
            HwndSource source = PresentationSource.FromVisual(this) as HwndSource;
            source.AddHook(WndProc);
        }

        private void SettingsButton_Click(object sender, RoutedEventArgs e)
        {
            if (SettingsFlyout.IsOpen)
                SettingsFlyout.IsOpen = false;
            else
            {
                SettingsFlyout.IsOpen = true;
                AboutFlyout.IsOpen = false;
            }

        }

        private void AboutButton_Click(object sender, RoutedEventArgs e)
        {
            if (AboutFlyout.IsOpen)
                AboutFlyout.IsOpen = false;
            else
            {
                AboutFlyout.IsOpen = true;
                SettingsFlyout.IsOpen = false;
            }
        }

        private void ChangeLogButton_Click(object sender, RoutedEventArgs e)
        {
            ReleaseNotesWindow changeLog = new ReleaseNotesWindow();
            changeLog.ShowDialog();
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

        private async Task ReceiveMessage(StatisticsTabMessage action)
        {
            if (action.StatisticsTab == StatisticsTab.Overview && UserSettings.Default.UserPlayerId < 1)
            {
                await this.ShowMessageAsync("Statistics", "To view your stats, enter your BattleTag in the Settings menu.");
            }
        }

        private void ReceiveMessage(MatchSummaryMessage action)
        {
            if (action.Trigger == Messages.Trigger.Open)
            {
                if (action.MatchSummary == MatchSummary.LatestMatches)
                    LatestMatchesSummaryFlyout.IsOpen = true;
                else if (action.MatchSummary == MatchSummary.QuickMatch)
                    QuickMatchSummaryFlyout.IsOpen = true;
                else if (action.MatchSummary == MatchSummary.UnrankedDraft)
                    UnrankedDraftSummaryFlyout.IsOpen = true;
                else if (action.MatchSummary == MatchSummary.HeroLeague)
                    HeroLeagueSummaryFlyout.IsOpen = true;
                else if (action.MatchSummary == MatchSummary.TeamLeague)
                    TeamLeagueSummaryFlyout.IsOpen = true;
                else if (action.MatchSummary == MatchSummary.Brawl)
                    BrawlSummaryFlyout.IsOpen = true;
                else if (action.MatchSummary == MatchSummary.CustomGame)
                    CustomGameSummaryFlyout.IsOpen = true;
            }
            else if (action.Trigger == Messages.Trigger.Close)
            {
                if (action.MatchSummary == MatchSummary.LatestMatches)
                    LatestMatchesSummaryFlyout.IsOpen = false;
                else if (action.MatchSummary == MatchSummary.QuickMatch)
                    QuickMatchSummaryFlyout.IsOpen = false;
                else if (action.MatchSummary == MatchSummary.UnrankedDraft)
                    UnrankedDraftSummaryFlyout.IsOpen = false;
                else if (action.MatchSummary == MatchSummary.HeroLeague)
                    HeroLeagueSummaryFlyout.IsOpen = false;
                else if (action.MatchSummary == MatchSummary.TeamLeague)
                    TeamLeagueSummaryFlyout.IsOpen = false;
                else if (action.MatchSummary == MatchSummary.Brawl)
                    BrawlSummaryFlyout.IsOpen = false;
                else if (action.MatchSummary == MatchSummary.CustomGame)
                    CustomGameSummaryFlyout.IsOpen = false;
                else
                {
                    QuickMatchSummaryFlyout.IsOpen = false;
                    UnrankedDraftSummaryFlyout.IsOpen = false;
                    HeroLeagueSummaryFlyout.IsOpen = false;
                    TeamLeagueSummaryFlyout.IsOpen = false;
                    BrawlSummaryFlyout.IsOpen = false;
                    CustomGameSummaryFlyout.IsOpen = false;
                }
            }
        }

        private IntPtr WndProc(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
        {
            if (msg == NativeMethods.WM_SHOWME)
            {
                Show();
                Activate();
                WindowState = WindowState.Maximized;
            }

            return IntPtr.Zero;
        }

        private void LatestMatchesSummaryFlyout_ClosingFinished(object sender, RoutedEventArgs e)
        {
            Messenger.Default.Send(new MatchSummaryMessage { MatchSummary = MatchSummary.LatestMatches, Trigger = Messages.Trigger.Close });
        }

        private void QuickMatchSummaryFlyout_ClosingFinished(object sender, RoutedEventArgs e)
        {
            Messenger.Default.Send(new MatchSummaryMessage { MatchSummary = MatchSummary.QuickMatch, Trigger = Messages.Trigger.Close });
        }

        private void UnrankedDraftSummaryFlyout_ClosingFinished(object sender, RoutedEventArgs e)
        {
            Messenger.Default.Send(new MatchSummaryMessage { MatchSummary = MatchSummary.UnrankedDraft, Trigger = Messages.Trigger.Close });
        }

        private void HeroLeagueSummaryFlyout_ClosingFinished(object sender, RoutedEventArgs e)
        {
            Messenger.Default.Send(new MatchSummaryMessage { MatchSummary = MatchSummary.HeroLeague, Trigger = Messages.Trigger.Close });
        }

        private void TeamLeagueSummaryFlyout_ClosingFinished(object sender, RoutedEventArgs e)
        {
            Messenger.Default.Send(new MatchSummaryMessage { MatchSummary = MatchSummary.TeamLeague, Trigger = Messages.Trigger.Close });
        }

        private void BrawlSummaryFlyout_ClosingFinished(object sender, RoutedEventArgs e)
        {
            Messenger.Default.Send(new MatchSummaryMessage { MatchSummary = MatchSummary.Brawl, Trigger = Messages.Trigger.Close });
        }

        private void CustomGameSummaryFlyout_ClosingFinished(object sender, RoutedEventArgs e)
        {
            Messenger.Default.Send(new MatchSummaryMessage { MatchSummary = MatchSummary.CustomGame, Trigger = Messages.Trigger.Close });
        }
    }
}
