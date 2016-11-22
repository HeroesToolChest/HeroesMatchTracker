using HeroesParserData.Messages;

namespace HeroesParserData.ViewModels.Match.Summary
{
    public class UnrankedDraftSummaryViewModel : MatchSummaryContext
    {
        public UnrankedDraftSummaryViewModel()
            :base()
        {
            HasObservers = false;
            HasBans = true;
        }

        protected override void ReceiveMessage(MatchSummaryMessage action)
        {
            if (action.MatchSummary == MatchSummary.UnrankedDraft)
            {
                QuerySummaryDetails(action.ReplayId);
            }
        }
    }
}
