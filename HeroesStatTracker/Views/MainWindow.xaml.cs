using GalaSoft.MvvmLight.Ioc;
using GalaSoft.MvvmLight.Messaging;
using HeroesStatTracker.Browser;
using HeroesStatTracker.Core;
using HeroesStatTracker.Core.Messaging;
using HeroesStatTracker.Core.ViewModels;
using HeroesStatTracker.Core.ViewServices;
using HeroesStatTracker.Data;
using HeroesStatTracker.Views.TitleBar;
using MahApps.Metro.Controls;
using System;
using System.Reflection;
using System.Windows;
using System.Windows.Interop;
using static HeroesStatTracker.App;

namespace HeroesStatTracker.Views
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : MetroWindow, IWhatsNewWindowService, IProfileWindowService, IBrowserWindowService
    {
        private MainWindowViewModel MainWindowViewModel;
        private IDatabaseService Database;
        private BrowserWindow BrowserWindow;

        public MainWindow()
        {
            InitializeComponent();

            MainWindowViewModel = (MainWindowViewModel)DataContext;
            Database = MainWindowViewModel.GetDatabaseService;

            SetTrayIcon();

            SimpleIoc.Default.Register<IWhatsNewWindowService>(() => this);
            SimpleIoc.Default.Register<IProfileWindowService>(() => this);
            SimpleIoc.Default.Register<IBrowserWindowService>(() => this);
        }

        public void CreateWhatsNewWindow()
        {
            WhatsNewWindow window = new WhatsNewWindow();
            window.ShowDialog();
        }

        public void CreateProfileWindow()
        {
            ProfileWindow window = new ProfileWindow();
            window.ShowDialog();
        }

        public void CreateBrowserWindow()
        {
            if (BrowserWindow == null)
                BrowserWindow = new BrowserWindow();

            BrowserWindow.Show();
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
    }
}
