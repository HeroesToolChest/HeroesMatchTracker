using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;
using HeroesMatchTracker.Core.Services.Dialogs;
using HeroesMatchTracker.ViewModels.ReplayParser;
using ReactiveUI;
using System.Threading.Tasks;

namespace HeroesMatchTracker.Views.ReplayParser
{
    public class ReplayParserControlView : ReactiveUserControl<ReplayParserControlViewModel>, IReplayParserControl
    {
        public ReplayParserControlView()
        {
            this.WhenActivated(disposables =>
            {
            });

            InitializeComponent();
        }

        public Window GetWindow => (Window)VisualRoot;

        public async Task<string> OpenFolder()
        {
            OpenFolderDialog openFolderDialog = new OpenFolderDialog();
            return await openFolderDialog.ShowAsync(GetWindow);
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
