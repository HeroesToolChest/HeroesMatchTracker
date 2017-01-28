using Heroes.Icons;
using Heroes.ReplayParser;
using HeroesStatTracker.Data;

namespace HeroesStatTracker.Core.ViewModels.Matches
{
    public class HeroLeagueViewModel : MatchesBase
    {
        public HeroLeagueViewModel(IDatabaseService database, IHeroesIconsService heroesIcons)
            : base(database, heroesIcons, GameMode.HeroLeague)
        {
        }
    }
}
