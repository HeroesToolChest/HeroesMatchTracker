using Heroes.ReplayParser;
using HeroesStatTracker.Data;

namespace HeroesStatTracker.Core.ViewModels.Matches
{
    public class CustomGameViewModel : MatchesBase
    {
        public CustomGameViewModel(IDatabaseService iDatabaseService)
            : base(iDatabaseService, GameMode.Custom)
        {
        }
    }
}
