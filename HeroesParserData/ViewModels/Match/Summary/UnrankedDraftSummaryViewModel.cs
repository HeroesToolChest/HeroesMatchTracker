using HeroesParserData.Messages;
using System.Threading.Tasks;

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
            if (action.MatchSummary == MatchSummary.UnrankedDraft && action.Trigger == Trigger.Open)
                ExecuteSelectedReplay(action);
            else if (action.MatchSummary == MatchSummary.UnrankedDraft && action.Trigger == Trigger.Close)
                ClearSummaryDetails();
        }
    }
}
