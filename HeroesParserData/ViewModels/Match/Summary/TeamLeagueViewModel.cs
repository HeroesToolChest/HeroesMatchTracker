using Heroes.ReplayParser;
using HeroesParserData.DataQueries;
using System.Collections.ObjectModel;

namespace HeroesParserData.ViewModels.Match.Summary
{
    public class TeamLeagueViewModel : MatchSummaryContext
    {
        public TeamLeagueViewModel()
            :base()
        {
            HasObservers = false;
            HasBans = true;
        }
    }
}
