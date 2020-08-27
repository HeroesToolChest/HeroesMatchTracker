using Avalonia;
using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;
using HeroesMatchTracker.ViewModels;

namespace HeroesMatchTracker.Views
{
    public class MainWindow : ReactiveWindow<MainWindowViewModel>
    {
        public MainWindow()
        {
            InitializeComponent();
#if DEBUG
            this.AttachDevTools();
#endif
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
