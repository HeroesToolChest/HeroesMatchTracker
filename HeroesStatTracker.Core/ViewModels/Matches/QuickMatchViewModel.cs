using Heroes.Icons;
using Heroes.ReplayParser;
using HeroesStatTracker.Data;

namespace HeroesStatTracker.Core.ViewModels.Matches
{
    public class QuickMatchViewModel : MatchesBase
    {
        public QuickMatchViewModel(IDatabaseService database, IHeroesIconsService heroesIcons)
            : base(database, heroesIcons, GameMode.QuickMatch)
        {
        }
    }
}
