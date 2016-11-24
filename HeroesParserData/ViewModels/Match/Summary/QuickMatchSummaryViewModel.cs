using HeroesParserData.Messages;

namespace HeroesParserData.ViewModels.Match.Summary
{
    public class QuickMatchSummaryViewModel : MatchSummaryContext
    {
        public QuickMatchSummaryViewModel()
            :base()
        {
            HasObservers = false;
            HasBans = false;
        }

        protected override void ReceiveMessage(MatchSummaryMessage action)
        {
            if (action.MatchSummary == MatchSummary.QuickMatch)
            {
                ExecuteSelectedReplay(action);
            }
        }
    }
}
