using Heroes.ReplayParser;
using HeroesStatTracker.Data;

namespace HeroesStatTracker.Core.ViewModels.Matches
{
    public class TeamLeagueViewModel : MatchesBase
    {
        public TeamLeagueViewModel(IDatabaseService iDatabaseService)
            : base(iDatabaseService, GameMode.TeamLeague)
        {
        }
    }
}
