using HeroesParserData.Messages;
using System.Threading.Tasks;

namespace HeroesParserData.ViewModels.Match.Summary
{
    public class CustomGameSummaryViewModel : MatchSummaryContext
    {
        public CustomGameSummaryViewModel()
            : base()
        {

        }

        protected override void ReceiveMessage(MatchSummaryMessage action)
        {
            if (action.MatchSummary == MatchSummary.CustomGame && action.Trigger == Trigger.Open)
                ExecuteSelectedReplay(action);
            else if (action.MatchSummary == MatchSummary.CustomGame && action.Trigger == Trigger.Close)
                ClearSummaryDetails();
        }
    }
}
