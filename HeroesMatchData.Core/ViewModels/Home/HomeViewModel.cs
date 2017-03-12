using GalaSoft.MvvmLight.Messaging;
using HeroesMatchData.Core.Messaging;
using HeroesMatchData.Core.Models.MatchHistoryModels;
using HeroesMatchData.Core.Services;
using HeroesMatchData.Core.User;
using HeroesMatchData.Data;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows;

namespace HeroesMatchData.Core.ViewModels.Home
{
    public class HomeViewModel : HstViewModel
    {
        private IInternalService InternalService;
        private IDatabaseService Database;
        private IUserProfileService UserProfile;
        private IWebsiteService Website;

        private DateTime? LatestReplayDateTime;
        private bool UserBattleTagUpdated;

        private ObservableCollection<MatchHistoryMatch> _matchCollection = new ObservableCollection<MatchHistoryMatch>();

        public HomeViewModel(IInternalService internalService, IWebsiteService website)
            : base(internalService.HeroesIcons)
        {
            InternalService = internalService;
            Database = internalService.Database;
            UserProfile = internalService.UserProfile;
            Website = website;

            UserBattleTagUpdated = false;
            LatestReplayDateTime = DateTime.MinValue;

            InitialMatchHistoryLoad();
            InitDynamicMatchLoading();

            Messenger.Default.Register<NotificationMessage>(this, (message) => ReceivedMessage(message));
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
                try
                {
                    await Task.Delay(2000);

                    while (true)
                    {
                        if (LatestReplayDateTime < Database.ReplaysDb().MatchReplay.ReadLatestReplayByDateTime())
                        {
                            var replays = Database.ReplaysDb().MatchReplay.ReadNewestLatestReplayByDateTimeList(LatestReplayDateTime);

                            foreach (var replay in replays)
                            {
                                MatchHistoryMatch match = new MatchHistoryMatch(InternalService, Website, Database.ReplaysDb().MatchReplay.ReadReplayIncludeAssociatedRecords(replay.ReplayId));
                                LatestReplayDateTime = replay.TimeStamp;

                                await Application.Current.Dispatcher.InvokeAsync(() =>
                                {
                                    MatchCollection.Insert(0, match);

                                    if (MatchCollection.Count > 20)
                                        MatchCollection.RemoveAt(MatchCollection.Count - 1);
                                });
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
                }
                catch (Exception ex)
                {
                    ExceptionLog.Log(NLog.LogLevel.Error, ex, "Match History Loading error");
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
