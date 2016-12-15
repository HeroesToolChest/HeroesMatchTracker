using HeroesParserData.Messages;

namespace HeroesParserData.ViewModels.Match.Summary
{
    public class LatestMatchesSummaryViewModel : MatchSummaryContext
    {
        public LatestMatchesSummaryViewModel()
            :base()
        {
            HasObservers = false;
            HasBans = false;
        }

        protected override void ReceiveMessage(MatchSummaryMessage action)
        {
            if (action.MatchSummary == MatchSummary.LatestMatches && action.Trigger == Trigger.Open)
                ExecuteSelectedReplay(action);
            else if (action.MatchSummary == MatchSummary.LatestMatches && action.Trigger == Trigger.Close)
                ClearSummaryDetails();
        }
    }
}
