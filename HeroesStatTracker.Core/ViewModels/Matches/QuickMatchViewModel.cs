using Heroes.ReplayParser;
using HeroesStatTracker.Data;

namespace HeroesStatTracker.Core.ViewModels.Matches
{
    public class QuickMatchViewModel : MatchesBase
    {
        public QuickMatchViewModel(IDatabaseService iDatabaseService)
            : base(iDatabaseService, GameMode.QuickMatch)
        {
        }
    }
}
