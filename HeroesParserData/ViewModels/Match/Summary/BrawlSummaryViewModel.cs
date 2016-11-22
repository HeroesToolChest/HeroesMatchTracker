using HeroesParserData.Messages;

namespace HeroesParserData.ViewModels.Match.Summary
{
    public class BrawlSummaryViewModel : MatchSummaryContext
    {
        public BrawlSummaryViewModel()
            :base()
        {
            HasObservers = false;
            HasBans = false;
        }

        protected override void ReceiveMessage(MatchSummaryMessage action)
        {
            if (action.MatchSummary == MatchSummary.Brawl)
            {
                QuerySummaryDetails(action.ReplayId);
            }
        }
    }
}
