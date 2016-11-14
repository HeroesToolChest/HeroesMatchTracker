using Heroes.ReplayParser;
using HeroesParserData.DataQueries;
using System.Collections.ObjectModel;

namespace HeroesParserData.ViewModels.Match.Summary
{
    public class HeroLeagueViewModel : MatchSummaryContext
    {
        public HeroLeagueViewModel()
            :base()
        {
            HasObservers = false;
            HasBans = true;
        }
    }
}
