using DynamicData;
using HeroesMatchTracker.Core.Models.ReplayParser;
using HeroesMatchTracker.Core.Services.Dialogs;
using HeroesMatchTracker.Core.Services.ReplayParser;
using HeroesMatchTracker.Infrastructure.Database.Contexts;
using Microsoft.EntityFrameworkCore;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using Splat;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Reactive;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Threading.Tasks;

namespace HeroesMatchTracker.ViewModels.ReplayParser
{
    public class ReplayParserControlViewModel : ViewModelBase, IActivatableViewModel, IEnableLogger
    {
        //private readonly IDbContextFactory<HeroesReplaysDbContext> _contextFactory;
        private readonly IReplayParserControl _replayParserControl;
        private readonly IReplayCollector _replayCollector;
        private readonly ReadOnlyObservableCollection<ReplayFile> _replayFiles;

        //private readonly ObservableAsPropertyHelper<bool> _isExecuting;

        public ReplayParserControlViewModel(IReplayParserControl? replayParserControl = null, IReplayCollector? replayCollector = null)
        {
            _replayParserControl = replayParserControl ?? Locator.Current.GetService<IReplayParserControl>();
            _replayCollector = replayCollector ?? Locator.Current.GetService<IReplayCollector>();

            var replayCollection = _replayCollector.Connect();
            replayCollection
                .Transform(x => x)
                //.Filter(x => x)
                .ObserveOn(RxApp.MainThreadScheduler)
                .StartWithEmpty()
                .Bind(out _replayFiles)
                .Subscribe();

            //_isExecuting = Scan
            //    .IsExecuting
            //    .ToProperty(this, nameof(IsExecuting));

            //IObservable<bool> canExecute = this
            //    .WhenAnyObservable(x => x.Scan.IsExecuting)
            //    .Select(x => !x);

            //IObservable<bool> canExecute = Observable.CombineLatest(
            //    this.WhenAnyObservable(x => x.Start.IsExecuting))
            //    .Select(x => x.Any(x => x));

            Scan = ReactiveCommand.CreateFromObservable(
                () => Observable
                    .Start(() => ScanImpl()));
            //Scan = ReactiveCommand.Create(
            //    () => ScanImpl(),
            //    canExecute,
            //    CurrentThreadScheduler.Instance);

            //Start = ReactiveCommand.CreateFromTask(
            //    () => StartImpl(),
            //    canExecute,
            //    RxApp.MainThreadScheduler);

            //Start = ReactiveCommand.CreateFromObservable(TestImpl);

            this.WhenActivated((CompositeDisposable disposables) =>
            {
                Disposable
                    .Create(() => HandleDeactivation())
                    .DisposeWith(disposables);
            });
        }

        public ReadOnlyObservableCollection<ReplayFile> ReplayFiles => _replayFiles;

        public ReactiveCommand<Unit, Unit> Scan { get; }

        public ReactiveCommand<Unit, Unit> Start { get; }

        [Reactive]
        public string ReplaysLocation { get; set; } = string.Empty;

        //public bool IsExecuting => _isExecuting.Value;

        public async Task BrowseReplayLocation()
        {
            ReplaysLocation = await _replayParserControl.OpenFolder();
        }

        public void ScanImpl()
        {
            if (string.IsNullOrWhiteSpace(ReplaysLocation))
                return;

            IEnumerable<ReplayFile> items = new DirectoryInfo(ReplaysLocation)
                .GetFiles($"*.StormReplay")
                .OrderBy(x => x.LastWriteTime)
                .Where(x => x.LastWriteTime > DateTime.MinValue)
                .Select(x => new ReplayFile(x.FullName, x.CreationTime));

            _replayCollector.Refresh(items);
        }

        //public IObservable<Unit> TestImpl()
        //{
        //    return Observable.Start(() => StartImpl());
        //}

        //public void StartImpl()
        //{
        //    for (int i = 0; i < 2000000000; i++)
        //    {
        //        int y = (23 * 345 / 100) + 5;
        //        y += 5;
        //    }

        //    for (int i = 0; i < 2000000000; i++)
        //    {
        //        int y = (23 * 345 / 100) + 5;
        //        y += 5;
        //    }

        //    for (int i = 0; i < 2000000000; i++)
        //    {
        //        int y = (23 * 345 / 100) + 5;
        //        y += 5;
        //    }
        //    // await Task.CompletedTask;
        //}

        private void HandleDeactivation()
        {
        }
    }
}
