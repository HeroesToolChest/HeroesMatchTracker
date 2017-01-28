using Heroes.Icons;
using Heroes.ReplayParser;
using HeroesStatTracker.Data;

namespace HeroesStatTracker.Core.ViewModels.Matches
{
    public class BrawlViewModel : MatchesBase
    {
        public BrawlViewModel(IDatabaseService iDatabaseService, IHeroesIconsService heroesIcons)
            : base(iDatabaseService, heroesIcons, GameMode.Brawl)
        {
        }
    }
}
