using Heroes.ReplayParser;
using HeroesParserData.DataQueries;
using System.Collections.ObjectModel;

namespace HeroesParserData.ViewModels.Match.Summary
{
    public class UnrankedDraftViewModel : MatchSummaryContext
    {
        public UnrankedDraftViewModel()
            :base()
        {
            HasObservers = false;
            HasBans = true;
        }
    }
}
