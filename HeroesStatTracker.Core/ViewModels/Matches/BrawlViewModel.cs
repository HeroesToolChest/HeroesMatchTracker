using Heroes.Icons;
using Heroes.ReplayParser;
using HeroesStatTracker.Core.User;
using HeroesStatTracker.Data;

namespace HeroesStatTracker.Core.ViewModels.Matches
{
    public class BrawlViewModel : MatchesBase
    {
        public BrawlViewModel(IDatabaseService iDatabaseService, IHeroesIconsService heroesIcons, IUserProfileService userProfile)
            : base(iDatabaseService, heroesIcons, userProfile, GameMode.Brawl)
        {
        }
    }
}
