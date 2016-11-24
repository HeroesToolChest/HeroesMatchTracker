using HeroesParserData.Messages;
using System.Threading.Tasks;

namespace HeroesParserData.ViewModels.Match.Summary
{
    public class HeroLeagueSummaryViewModel : MatchSummaryContext
    {
        public HeroLeagueSummaryViewModel()
            :base()
        {
            HasObservers = false;
            HasBans = true;
        }

        protected override void ReceiveMessage(MatchSummaryMessage action)
        {
            if (action.MatchSummary == MatchSummary.HeroLeague && action.Trigger == Trigger.Open)
                ExecuteSelectedReplay(action);
            else if (action.MatchSummary == MatchSummary.HeroLeague && action.Trigger == Trigger.Close)
                ClearSummaryDetails();
        }
    }
}
