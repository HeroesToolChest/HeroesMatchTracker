using HeroesParserData.Messages;
using System.Threading.Tasks;

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
            if (action.MatchSummary == MatchSummary.Brawl && action.Trigger == Trigger.Open)
                ExecuteSelectedReplay(action);
            else if (action.MatchSummary == MatchSummary.Brawl && action.Trigger == Trigger.Close)
                ClearSummaryDetails();
        }
    }
}
