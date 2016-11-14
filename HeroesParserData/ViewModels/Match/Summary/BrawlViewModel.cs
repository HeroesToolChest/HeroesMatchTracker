using Heroes.ReplayParser;
using HeroesParserData.DataQueries;
using System.Collections.ObjectModel;

namespace HeroesParserData.ViewModels.Match.Summary
{
    public class BrawlViewModel : MatchSummaryContext
    {
        public BrawlViewModel()
            :base()
        {
            HasObservers = false;
            HasBans = false;
        }
    }
}
