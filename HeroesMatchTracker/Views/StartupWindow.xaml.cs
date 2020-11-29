using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;
using HeroesMatchTracker.Core.Startup;
using HeroesMatchTracker.ViewModels;
using ReactiveUI;

namespace HeroesMatchTracker.Views
{
    public class StartupWindow : ReactiveWindow<StartupWindowViewModel>, IStartupWindow
    {
        public StartupWindow()
        {
            this.WhenActivated(disposables =>
            {
            });

            InitializeComponent();
#if DEBUG
            this.AttachDevTools();
#endif
        }

        public void CloseStartupWindow()
        {
            Close();
        }

        public void CreateMainWindow()
        {
            if (Application.Current.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            {
                MainWindow mainWindow = new MainWindow()
                {
                    DataContext = new MainWindowViewModel(),
                };
                desktop.MainWindow = mainWindow;

                mainWindow.Show();
            }
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
