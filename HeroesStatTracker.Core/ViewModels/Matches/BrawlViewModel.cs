using Heroes.ReplayParser;
using HeroesStatTracker.Data;

namespace HeroesStatTracker.Core.ViewModels.Matches
{
    public class BrawlViewModel : MatchesBase
    {
        public BrawlViewModel(IDatabaseService iDatabaseService)
            : base(iDatabaseService, GameMode.Brawl)
        {
        }
    }
}
