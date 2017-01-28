using Heroes.Icons;
using Heroes.ReplayParser;
using HeroesStatTracker.Data;

namespace HeroesStatTracker.Core.ViewModels.Matches
{
    public class UnrankedDraftViewModel : MatchesBase
    {
        public UnrankedDraftViewModel(IDatabaseService database, IHeroesIconsService heroesIcons)
            : base(database, heroesIcons, GameMode.UnrankedDraft)
        {
        }
    }
}
