using GalaSoft.MvvmLight.Messaging;
using HeroesMatchTracker.Core.Messaging;
using HeroesMatchTracker.Core.Models.MatchHistoryModels;
using HeroesMatchTracker.Core.Services;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace HeroesMatchTracker.Core.ViewModels.Home
{
    public class HomeViewModel : HmtViewModel
    {
        private IWebsiteService Website;

        private DateTime? LatestReplayDateTime;
        private bool UserBattleTagUpdated;

        private ObservableCollection<MatchHistoryMatch> _matchCollection = new ObservableCollection<MatchHistoryMatch>();

        public HomeViewModel(IInternalService internalService, IWebsiteService website)
            : base(internalService)
        {
            Website = website;

            UserBattleTagUpdated = false;
            LatestReplayDateTime = DateTime.MinValue;

            Messenger.Default.Register<NotificationMessage>(this, (message) => ReceivedMessage(message));

            InitialMatchHistoryLoad();
            InitDynamicMatchLoading();
        }

        public ObservableCollection<MatchHistoryMatch> MatchCollection
        {
            get => _matchCollection;
            set
            {
                _matchCollection = value;
                RaisePropertyChanged();
            }
        }

        private void InitialMatchHistoryLoad()
        {
            var replays = Database.ReplaysDb().MatchReplay.ReadLatestReplaysByDateTimeList(20);

            foreach (var replay in replays)
            {
                MatchHistoryMatch match = new MatchHistoryMatch(InternalService, Website, Database.ReplaysDb().MatchReplay.ReadReplayIncludeAssociatedRecords(replay.ReplayId));

                MatchCollection.Add(match);
            }

            if (replays.Count > 0)
                LatestReplayDateTime = replays[0].TimeStamp;
        }

        private void InitDynamicMatchLoading()
        {
            Task.Run(async () =>
            {
                while (true)
                {
                    if (LatestReplayDateTime < Database.ReplaysDb().MatchReplay.ReadLatestReplayByDateTime())
                    {
                        var replays = Database.ReplaysDb().MatchReplay.ReadNewestLatestReplayByDateTimeList(LatestReplayDateTime);

                        foreach (var replay in replays)
                        {
                            MatchHistoryMatch match = new MatchHistoryMatch(InternalService, Website, Database.ReplaysDb().MatchReplay.ReadReplayIncludeAssociatedRecords(replay.ReplayId));
                            LatestReplayDateTime = replay.TimeStamp;

                            if (!MatchCollection.Any(x => x.ReplayId == match.ReplayId))
                            {
                                await Application.Current.Dispatcher.InvokeAsync(() =>
                                {
                                    MatchCollection.Insert(0, match);

                                    if (MatchCollection.Count > 20)
                                        MatchCollection.RemoveAt(MatchCollection.Count - 1);
                                });
                            }
                        }
                    }

                    if (UserBattleTagUpdated)
                    {
                        // update the current matches in the match list
                        for (int i = 0; i < MatchCollection.Count; i++)
                        {
                            var updated = new MatchHistoryMatch(InternalService, Website, Database.ReplaysDb().MatchReplay.ReadReplayIncludeAssociatedRecords(MatchCollection[i].ReplayId));

                            await Application.Current.Dispatcher.InvokeAsync(() =>
                            {
                                MatchCollection[i] = updated;
                            });
                        }

                        UserBattleTagUpdated = false;
                    }

                    await Task.Delay(2000);
                }
            });
        }

        private void ReceivedMessage(NotificationMessage message)
        {
            if (message.Notification == StaticMessage.UpdateUserBattleTag)
            {
                UserBattleTagUpdated = true;
            }
        }
    }
}
