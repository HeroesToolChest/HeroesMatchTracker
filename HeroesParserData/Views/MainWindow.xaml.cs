using HeroesParserData.Properties;
using System;
using System.Reflection;
using System.Windows;

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

            InitializeComponent();
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
