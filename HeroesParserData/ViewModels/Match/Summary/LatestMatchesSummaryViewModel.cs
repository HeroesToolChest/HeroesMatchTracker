using HeroesParserData.Messages;

namespace HeroesParserData.ViewModels.Match.Summary
{
    public class LatestMatchesSummaryViewModel : MatchSummaryContext
    {
        private bool IsRefreshLastMatchDataOn;

        public LatestMatchesSummaryViewModel()
            :base()
        {
            IsRefreshLastMatchDataOn = false;

            IsLeftChangeButtonEnabled = false;
            IsRightChangeButtonEnabled = false;
            IsLeftChangeButtonVisible = false;
            IsRightChangeButtonVisible = false;
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
