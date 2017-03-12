using GalaSoft.MvvmLight.Ioc;
using HeroesStatTracker.Core.ViewServices;
using System.Windows;

namespace HeroesStatTracker.Views
{
    /// <summary>
    /// Interaction logic for StartupWindow.xaml
    /// </summary>
    public partial class StartupWindow : Window, IMainWindowService
    {
        public StartupWindow()
        {
            InitializeComponent();

            SimpleIoc.Default.Register<IMainWindowService>(() => this);
        }

        public void CreateMainWindow()
        {
            MainWindow mainWindow = new MainWindow()
            {
                WindowState = WindowState.Maximized,
            };

            Close();
            mainWindow.Show();
        }
    }
}
