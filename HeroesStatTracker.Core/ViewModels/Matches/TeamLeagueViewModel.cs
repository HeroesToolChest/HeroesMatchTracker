using Heroes.Icons;
using Heroes.ReplayParser;
using HeroesStatTracker.Core.Messaging;
using HeroesStatTracker.Core.User;
using HeroesStatTracker.Data;

namespace HeroesStatTracker.Core.ViewModels.Matches
{
    public class TeamLeagueViewModel : MatchesBase
    {
        public TeamLeagueViewModel(IDatabaseService database, IHeroesIconsService heroesIcons, IUserProfileService userProfile)
            : base(database, heroesIcons, userProfile, GameMode.TeamLeague)
        { }

        protected override void ReceivedMatchSearchData(MatchesDataMessage message)
        {
            if (message.MatchTab == MatchesTab.TeamLeague)
            {
                base.ReceivedMatchSearchData(message);
            }
        }
    }
}
