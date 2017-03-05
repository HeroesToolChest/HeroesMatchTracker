using Heroes.Icons;
using Heroes.ReplayParser;
using HeroesStatTracker.Core.Messaging;
using HeroesStatTracker.Core.User;
using HeroesStatTracker.Data;

namespace HeroesStatTracker.Core.ViewModels.Matches
{
    public class HeroLeagueViewModel : MatchesBase
    {
        public HeroLeagueViewModel(IDatabaseService database, IHeroesIconsService heroesIcons, IUserProfileService userProfile)
            : base(database, heroesIcons, userProfile, GameMode.HeroLeague)
        {
        }

        protected override void ReceivedMatchSearchData(MatchesDataMessage message)
        {
            if (message.MatchTab == MatchesTab.HeroLeague)
            {
                base.ReceivedMatchSearchData(message);
            }
        }
    }
}
