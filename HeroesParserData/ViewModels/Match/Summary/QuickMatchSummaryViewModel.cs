using HeroesParserData.Messages;
using System.Threading.Tasks;

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
            if (action.MatchSummary == MatchSummary.QuickMatch && action.Trigger == Trigger.Open)
                ExecuteSelectedReplay(action);
            else if (action.MatchSummary == MatchSummary.QuickMatch && action.Trigger == Trigger.Close)
                ClearSummaryDetails();
        }
    }
}
