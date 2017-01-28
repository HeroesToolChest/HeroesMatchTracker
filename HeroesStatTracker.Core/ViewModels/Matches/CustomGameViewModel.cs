using Heroes.Icons;
using Heroes.ReplayParser;
using HeroesStatTracker.Data;

namespace HeroesStatTracker.Core.ViewModels.Matches
{
    public class CustomGameViewModel : MatchesBase
    {
        public CustomGameViewModel(IDatabaseService database, IHeroesIconsService heroesIcons)
            : base(database, heroesIcons, GameMode.Custom)
        {
        }
    }
}
