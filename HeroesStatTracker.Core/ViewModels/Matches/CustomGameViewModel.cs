using Heroes.Icons;
using Heroes.ReplayParser;
using HeroesStatTracker.Core.Messaging;
using HeroesStatTracker.Core.User;
using HeroesStatTracker.Data;

namespace HeroesStatTracker.Core.ViewModels.Matches
{
    public class CustomGameViewModel : MatchesBase
    {
        public CustomGameViewModel(IDatabaseService database, IHeroesIconsService heroesIcons, IUserProfileService userProfile)
            : base(database, heroesIcons, userProfile, GameMode.Custom)
        { }

        protected override void ReceivedMatchSearchData(MatchesDataMessage message)
        {
            if (message.MatchTab == MatchesTab.Custom)
            {
                base.ReceivedMatchSearchData(message);
            }
        }
    }
}
