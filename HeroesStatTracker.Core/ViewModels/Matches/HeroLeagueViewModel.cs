using Heroes.ReplayParser;
using HeroesStatTracker.Data;

namespace HeroesStatTracker.Core.ViewModels.Matches
{
    public class HeroLeagueViewModel : MatchesBase
    {
        public HeroLeagueViewModel(IDatabaseService iDatabaseService)
            : base(iDatabaseService, GameMode.HeroLeague)
        {
        }
    }
}
