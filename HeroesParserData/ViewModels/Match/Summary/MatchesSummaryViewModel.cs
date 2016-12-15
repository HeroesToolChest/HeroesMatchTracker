using HeroesParserData.Messages;

namespace HeroesParserData.ViewModels.Match.Summary
{
    public class MatchesSummaryViewModel : MatchSummaryContext
    {
        public MatchesSummaryViewModel()
            :base()
        {
            HasObservers = false;
            HasBans = false;
        }

        protected override void ReceiveMessage(MatchSummaryMessage action)
        {
            if (action.MatchSummary == MatchSummary.Matches && action.Trigger == Trigger.Open)
                ExecuteSelectedReplay(action);
            else if (action.MatchSummary == MatchSummary.Matches && action.Trigger == Trigger.Close)
                ClearSummaryDetails();
        }
    }
}
