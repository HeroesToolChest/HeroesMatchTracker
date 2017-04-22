using HeroesMatchData.Core.Updater;
using HeroesMatchData.Views;
using System;
using System.Data.SqlClient;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows;

namespace HeroesMatchData
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields", Justification = "Only used in Release mode")]
        private static readonly Mutex Mutex = new Mutex(true, "{66563FC5-C7B4-4CAA-84D7-ECE5819B6C8C}");

        internal App()
        {
            InitializeComponent();
        }

        public static System.Windows.Forms.NotifyIcon NotifyIcon { get; set; }

        public static string VersionAsString()
        {
            Version version = Assembly.GetEntryAssembly().GetName().Version;
            return $"{version.Major}.{version.Minor}.{version.Build}";
        }

        [STAThread]
        internal static void Main()
        {
#if !DEBUG
            // check if another instance is already running
            if (Mutex.WaitOne(TimeSpan.Zero, true))
            {
                App app = new App();
                StartupWindow startupWindow = new StartupWindow();
                app.Run(startupWindow);

                Mutex.ReleaseMutex();
            }
            else
            {
                // send message to maximize existing window
                NativeMethods.PostMessage((IntPtr)NativeMethods.HWND_BROADCAST, NativeMethods.WM_SHOWME, IntPtr.Zero, IntPtr.Zero);
            }
#endif
#if DEBUG
            App app = new App();
            StartupWindow startupWindow = new StartupWindow();
            app.Run(startupWindow);
#endif
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            Core.Properties.Settings.Default.IsManualUpdateApplied = false;

            base.OnStartup(e);
        }

        protected override void OnExit(ExitEventArgs e)
        {
            if (NotifyIcon != null)
            {
                NotifyIcon.Visible = false;
            }

            // this should only trigger if the update is applied through the settings menu
            if (Core.Properties.Settings.Default.IsManualUpdateApplied)
            {
                SqlConnection.ClearAllPools();
            }

            base.OnExit(e);
        }

        internal class NativeMethods
        {
            public const int HWND_BROADCAST = 0xffff;
            public static readonly int WM_SHOWME = RegisterWindowMessage("WM_SHOWME");

            private NativeMethods() { }

            [DllImport("user32")]
            public static extern bool PostMessage(IntPtr hwnd, int msg, IntPtr wparam, IntPtr lparam);
            [DllImport("user32", CharSet = CharSet.Unicode)]
            public static extern int RegisterWindowMessage(string message);
        }
    }
}
