using HeroesMatchTracker.Core;
using HeroesMatchTracker.Core.Startup;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using Splat;
using System;
using System.Reactive;
using System.Reactive.Disposables;
using System.Threading.Tasks;

namespace HeroesMatchTracker.ViewModels
{
    public class StartupWindowViewModel : ViewModelBase, IActivatableViewModel, IEnableLogger
    {
        private readonly ILoadStartup _startup;
        private readonly IDatabaseInit _databaseInit;
        private readonly IStartupWindow _startupWindow;

        private readonly ReactiveCommand<Unit, Unit> _closeStartupWindow;

        public StartupWindowViewModel(IStartupWindow? startupWindow = null, ILoadStartup? startup = null, IDatabaseInit? databaseInit = null)
        {
            _startup = startup ??= Locator.Current.GetService<ILoadStartup>();
            _databaseInit = databaseInit ??= Locator.Current.GetService<IDatabaseInit>();
            _startupWindow = startupWindow ??= Locator.Current.GetService<IStartupWindow>();

            if (_startup == null)
                this.Log().Fatal($"Could not find the service for {nameof(ILoadStartup)}");
            if (_databaseInit == null)
                this.Log().Fatal($"Could not find the service for {nameof(IDatabaseInit)}");

            _closeStartupWindow = ReactiveCommand.CreateFromTask(() => ExecuteStartup(), outputScheduler: RxApp.MainThreadScheduler);

            this.WhenActivated((CompositeDisposable disposables) =>
            {
                Disposable
                    .Create(() => HandleDeactivation())
                    .DisposeWith(disposables);

                _closeStartupWindow.Execute().Subscribe();
            });

            _closeStartupWindow.ThrownExceptions.Subscribe(error =>
            {
                this.Log().Fatal(error);
            });
        }

        public string AppVersion { get; } = AssemblyVersions.HeroesMatchTrackerVersion();

        [Reactive]
        public string StatusLabel { get; set; } = string.Empty;

        [Reactive]
        public string DetailedStatusLabel { get; set; } = string.Empty;

        public string CurrentStatus { get; set; } = string.Empty;

        public async Task ExecuteStartup()
        {
            StatusLabel = "Starting up...";
            DetailedStatusLabel = "Getting system info";

            await Task.Delay(100);

            _startup.LogSystemInformation();

            DetailedStatusLabel = "Database updates";

            await Task.Delay(100);

            _databaseInit.HMT2ReplayDbCheck();
            _databaseInit.InitHeroesReplaysDb();

            DetailedStatusLabel = "Initializing main window";

            await Task.Delay(100);

            _startupWindow.CreateMainWindow();
            _startupWindow.CloseStartupWindow();
        }

        private void HandleDeactivation()
        {
        }
    }
}
