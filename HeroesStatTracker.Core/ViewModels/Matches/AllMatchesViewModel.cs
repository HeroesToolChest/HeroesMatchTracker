using Heroes.Icons;
using Heroes.ReplayParser;
using HeroesStatTracker.Core.User;
using HeroesStatTracker.Data;

namespace HeroesStatTracker.Core.ViewModels.Matches
{
    public class AllMatchesViewModel : MatchesBase
    {
        public AllMatchesViewModel(IDatabaseService database, IHeroesIconsService heroesIcons, IUserProfileService userProfile)
            : base(database, heroesIcons, userProfile, GameMode.Brawl | GameMode.Custom | GameMode.HeroLeague | GameMode.QuickMatch | GameMode.TeamLeague | GameMode.UnrankedDraft)
        {
        }
    }
}
