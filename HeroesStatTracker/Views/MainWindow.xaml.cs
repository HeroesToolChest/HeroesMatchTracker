using GalaSoft.MvvmLight.Ioc;
using HeroesStatTracker.Core.ViewServices;
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
            SetTrayIcon();
            InitializeComponent();

            SimpleIoc.Default.Register<IWhatsNewWindowService>(() => this);
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

            NotifyIcon = new System.Windows.Forms.NotifyIcon
            {
                Icon = System.Drawing.Icon.ExtractAssociatedIcon(Assembly.GetExecutingAssembly().Location),
                Visible = false,
                ContextMenu = contextMenu,
                Text = $"Heroes Stat Tracker {VersionAsString()}"
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

        public void CreateWhatsNewWindow()
        {
            WhatsNewWindow window = new WhatsNewWindow();
            window.ShowDialog();
        }
    }
}
