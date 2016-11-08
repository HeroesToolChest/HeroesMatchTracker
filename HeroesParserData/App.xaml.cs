using HeroesIcons;
using HeroesParserData.Properties;
using HeroesParserData.Views;
using System;
using System.Data.SqlClient;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows;

namespace HeroesParserData
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private static Mutex Mutex = new Mutex(true, "4410ba2e-38b09f33-f4f696b-7b56458a");

        public static HeroesInfo HeroesInfo { get; set; }
        public static string NewLatestDirectory { get; set; }
        public static bool IsProcessingReplays { get; set; }
        public static System.Windows.Forms.NotifyIcon NotifyIcon { get; set; }
        public static bool NewReleaseApplied { get; set; }
        public static bool NewDatabaseCreated { get; private set; }

        App()
        {
            InitializeComponent();
        }

        [STAThread]
        static void Main()
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
            NewReleaseApplied = false;
            DatabaseFileExists();

            //// add custom accent and theme resource dictionaries
            //ThemeManager.AddAccent("Accent", new Uri("pack://application:,,,/Resources/CustomAccent.xaml"));

            //// get the theme from the current application
            //var theme = ThemeManager.DetectAppStyle();
            //ThemeManager.

            //// now use the custom accent
            //ThemeManager.ChangeAppStyle(Current, ThemeManager.GetAccent("Accent"), );

            base.OnStartup(e);
        }

        protected override void OnExit(ExitEventArgs e)
        {
            if (NotifyIcon != null)
            {
                NotifyIcon.Visible = false;
            }

            // this should only trigger if the update is applied through the settings menu
            if (NewReleaseApplied)
            {
                SqlConnection.ClearAllPools();
                AutoUpdater.CopyDatabaseToLatestRelease();
            }

            base.OnExit(e);
        }

        private void DatabaseFileExists()
        {
            if (!File.Exists($@"Database/{Settings.Default.DatabaseFile}"))
                NewDatabaseCreated = true;
            else
                NewDatabaseCreated = false;
        }
    }

    internal class NativeMethods
    {
        public const int HWND_BROADCAST = 0xffff;
        public static readonly int WM_SHOWME = RegisterWindowMessage("WM_SHOWME");
        [DllImport("user32")]
        public static extern bool PostMessage(IntPtr hwnd, int msg, IntPtr wparam, IntPtr lparam);
        [DllImport("user32")]
        public static extern int RegisterWindowMessage(string message);
    }
}
