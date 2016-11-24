using HeroesParserData.Messages;
using System.Threading.Tasks;

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
            if (action.MatchSummary == MatchSummary.TeamLeague && action.Trigger == Trigger.Open)
                ExecuteSelectedReplay(action);
            else if (action.MatchSummary == MatchSummary.TeamLeague && action.Trigger == Trigger.Close)
                ClearSummaryDetails();
        }
    }
}
