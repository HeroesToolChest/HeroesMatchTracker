using Heroes.ReplayParser;
using HeroesStatTracker.Data;

namespace HeroesStatTracker.Core.ViewModels.Matches
{
    public class AllMatchesViewModel : MatchesBase
    {
        public AllMatchesViewModel(IDatabaseService iDatabaseService)
            : base(iDatabaseService, GameMode.Brawl | GameMode.Custom | GameMode.HeroLeague | GameMode.QuickMatch | GameMode.TeamLeague | GameMode.UnrankedDraft)
        {
        }
    }
}
