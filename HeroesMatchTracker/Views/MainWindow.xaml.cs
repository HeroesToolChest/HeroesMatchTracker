using GalaSoft.MvvmLight.Ioc;
using GalaSoft.MvvmLight.Messaging;
using HeroesMatchTracker.Core.Messaging;
using HeroesMatchTracker.Core.ViewModels;
using HeroesMatchTracker.Core.ViewServices;
using HeroesMatchTracker.Data;
using HeroesMatchTracker.Views.Replays;
using HeroesMatchTracker.Views.TitleBar;
using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using System;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Interop;
using static HeroesMatchTracker.App;

namespace HeroesMatchTracker.Views
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : MetroWindow, ICreateWindowService, IMainWindowDialogsService
    {
        private MainWindowViewModel MainWindowViewModel;
        private IDatabaseService Database;

        public MainWindow()
        {
            InitializeComponent();

            MainWindowViewModel = (MainWindowViewModel)DataContext;
            Database = MainWindowViewModel.GetDatabaseService;

            SetTrayIcon();

            SimpleIoc.Default.Register<ICreateWindowService>(() => this);
            SimpleIoc.Default.Register<IMainWindowDialogsService>(() => this);
        }

        public void ShowWhatsNewWindow()
        {
            WhatsNewWindow window = new WhatsNewWindow();
            window.ShowDialog();
        }

        public void ShowProfileWindow()
        {
            ProfileWindow window = new ProfileWindow();
            window.ShowDialog();
        }

        public void ShowUnParsedReplaysWindow()
        {
            UnparsedReplaysWindow window = new UnparsedReplaysWindow();
            window.ShowDialog();
        }

        public async Task<bool> CheckBattleTagSetDialog()
        {
            if (Database.SettingsDb().UserSettings.UserPlayerId < 1)
            {
                await this.ShowMessageAsync("Statistics", "To view your stats, enter your BattleTag in the Profile menu.");
                return true;
            }
            else
            {
                return false;
            }
        }

        protected override void OnStateChanged(EventArgs e)
        {
            base.OnStateChanged(e);
            if (Database.SettingsDb().UserSettings.IsMinimizeToTray && WindowState == WindowState.Minimized)
            {
                Hide();
                NotifyIcon.Visible = true;
            }
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
            {
                SettingsFlyout.IsOpen = false;
            }
            else
            {
                SettingsFlyout.IsOpen = true;
                AboutFlyout.IsOpen = false;
            }
        }

        private void AboutButton_Click(object sender, RoutedEventArgs e)
        {
            if (AboutFlyout.IsOpen)
            {
                AboutFlyout.IsOpen = false;
            }
            else
            {
                AboutFlyout.IsOpen = true;
                SettingsFlyout.IsOpen = false;
            }
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
            menuItem1.Click += (sender, e) =>
            {
                Show();
                WindowState = WindowState.Maximized;
            };

            menuItem2.Index = 1;
            menuItem2.Text = "Exit";
            menuItem2.Click += (sender, e) =>
            {
                Application.Current.Shutdown();
            };

            NotifyIcon = new System.Windows.Forms.NotifyIcon
            {
                Icon = System.Drawing.Icon.ExtractAssociatedIcon(Assembly.GetExecutingAssembly().Location),
                Visible = false,
                ContextMenu = contextMenu,
                Text = $"Heroes Stat Tracker {VersionAsString()}",
            };
            NotifyIcon.DoubleClick += (sender, e) =>
            {
                Show();
                WindowState = WindowState.Maximized;
            };
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

        private void MatchSummaryFlyout_ClosingFinished(object sender, RoutedEventArgs e)
        {
            Messenger.Default.Send(new NotificationMessage(StaticMessage.MatchSummaryClosed));
        }

        private async void MetroWindow_Loaded(object sender, RoutedEventArgs e)
        {
            await Task.Delay(1000);

            if (Database.SettingsDb().UserSettings.ShowWhatsNewWindow)
            {
                Database.SettingsDb().UserSettings.ShowWhatsNewWindow = false;
                ShowWhatsNewWindow();
            }
        }
    }
}
