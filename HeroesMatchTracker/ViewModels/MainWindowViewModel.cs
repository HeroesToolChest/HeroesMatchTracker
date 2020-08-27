using HeroesMatchTracker.ViewModels.ReplayParser;
using ReactiveUI;
using Splat;
using System;

namespace HeroesMatchTracker.ViewModels
{
    public class MainWindowViewModel : ViewModelBase, IActivatableViewModel, IEnableLogger
    {
        public MainWindowViewModel()
        {
            ReplayParserControlViewModel = new ReplayParserControlViewModel();

            ThrownExceptions.Subscribe(error =>
            {
                this.Log().Fatal($"{error.Message} : {error}");
            });
        }

        public ReplayParserControlViewModel ReplayParserControlViewModel { get; }
    }
}
