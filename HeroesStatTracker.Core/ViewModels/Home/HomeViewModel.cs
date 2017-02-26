using Heroes.Icons;
using HeroesStatTracker.Core.Models.MatchHistoryModels;
using HeroesStatTracker.Core.User;
using HeroesStatTracker.Data;
using System.Collections.ObjectModel;

namespace HeroesStatTracker.Core.ViewModels.Home
{
    public class HomeViewModel : HstViewModel
    {
        private IDatabaseService Database;
        private IUserProfileService UserProfile;

        private ObservableCollection<MatchHistoryMatch> _matchCollection = new ObservableCollection<MatchHistoryMatch>();

        public HomeViewModel(IDatabaseService database, IHeroesIconsService heroesIcons, IUserProfileService userProfile)
            : base(heroesIcons)
        {
            Database = database;
            UserProfile = userProfile;

            InitialMatchHistoryLoad();
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
            var replays = Database.ReplaysDb().MatchReplay.ReadLatestReplaysByDateTime(20);

            foreach (var replay in replays)
            {
                MatchHistoryMatch match = new MatchHistoryMatch(Database, HeroesIcons, UserProfile, Database.ReplaysDb().MatchReplay.ReadReplayIncludeAssociatedRecords(replay.ReplayId));
                MatchCollection.Add(match);
            }
        }
    }
}
