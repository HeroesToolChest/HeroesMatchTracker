using HeroesParserData.Messages;

namespace HeroesParserData.ViewModels.Match.Summary
{
    public class TeamLeagueSummaryViewModel : MatchSummaryContext
    {
        public TeamLeagueSummaryViewModel()
            :base()
        {
            HasObservers = false;
            HasBans = true;
        }

        protected override void ReceiveMessage(MatchSummaryMessage action)
        {
            if (action.MatchSummary == MatchSummary.TeamLeague)
            {
                QuerySummaryDetails(action.ReplayId);
            }
        }
    }
}
