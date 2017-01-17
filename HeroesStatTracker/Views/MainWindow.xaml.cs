using GalaSoft.MvvmLight.Ioc;
using HeroesStatTracker.Core;
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
    public partial class MainWindow : MetroWindow, IWhatsNewWindowService
    {
        public MainWindow()
        {
            StylePalette.ApplyBase(QueryDb.SettingsDb.UserSettings.IsNightMode);
            StylePalette.ApplyPrimary(StylePalette.GetSwatchByName(QueryDb.SettingsDb.UserSettings.MainStylePrimary));
            StylePalette.ApplyAccent(StylePalette.GetSwatchByName(QueryDb.SettingsDb.UserSettings.MainStyleAccent));

            SetTrayIcon();

            InitializeComponent();

            SimpleIoc.Default.Register<IWhatsNewWindowService>(() => this);
        }

        public void CreateWhatsNewWindow()
        {
            WhatsNewWindow window = new WhatsNewWindow();
            window.ShowDialog();
        }

        protected override void OnStateChanged(EventArgs e)
        {
            base.OnStateChanged(e);
            if (QueryDb.SettingsDb.UserSettings.IsMinimizeToTray && WindowState == WindowState.Minimized)
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

        protected override void OnContentRendered(EventArgs e)
        {
            StylePalette.ApplyStyle(QueryDb.SettingsDb.UserSettings.IsAlternateStyle);
            base.OnContentRendered(e);
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
    }
}
