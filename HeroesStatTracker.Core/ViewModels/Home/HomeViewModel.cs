using Heroes.Icons;
using HeroesStatTracker.Core.Models.MatchHistoryModels;
using HeroesStatTracker.Core.User;
using HeroesStatTracker.Data;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows;

namespace HeroesStatTracker.Core.ViewModels.Home
{
    public class HomeViewModel : HstViewModel
    {
        private IDatabaseService Database;
        private IUserProfileService UserProfile;

        private DateTime? LatestReplayDateTime;

        private ObservableCollection<MatchHistoryMatch> _matchCollection = new ObservableCollection<MatchHistoryMatch>();

        public HomeViewModel(IDatabaseService database, IHeroesIconsService heroesIcons, IUserProfileService userProfile)
            : base(heroesIcons)
        {
            Database = database;
            UserProfile = userProfile;

            LatestReplayDateTime = DateTime.MinValue;

            InitialMatchHistoryLoad();
            InitDynamicMatchLoading();
        }

        public ObservableCollection<MatchHistoryMatch> MatchCollection
        {
            get { return _matchCollection; }
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
                MatchHistoryMatch match = new MatchHistoryMatch(Database, HeroesIcons, UserProfile, Database.ReplaysDb().MatchReplay.ReadReplayIncludeAssociatedRecords(replay.ReplayId));

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
                                MatchHistoryMatch match = new MatchHistoryMatch(Database, HeroesIcons, UserProfile, Database.ReplaysDb().MatchReplay.ReadReplayIncludeAssociatedRecords(replay.ReplayId));
                                LatestReplayDateTime = replay.TimeStamp;

                                await Application.Current.Dispatcher.InvokeAsync(() =>
                                {
                                    MatchCollection.Insert(0, match);

                                    if (MatchCollection.Count > 20)
                                        MatchCollection.RemoveAt(MatchCollection.Count - 1);
                                });
                            }
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
    }
}
