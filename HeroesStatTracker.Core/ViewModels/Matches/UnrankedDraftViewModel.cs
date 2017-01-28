using Heroes.ReplayParser;
using HeroesStatTracker.Data;

namespace HeroesStatTracker.Core.ViewModels.Matches
{
    public class UnrankedDraftViewModel : MatchesBase
    {
        public UnrankedDraftViewModel(IDatabaseService iDatabaseService)
            : base(iDatabaseService, GameMode.UnrankedDraft)
        {
        }
    }
}
