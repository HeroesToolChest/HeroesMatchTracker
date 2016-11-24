using HeroesParserData.Messages;

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
            if (action.MatchSummary == MatchSummary.CustomGame)
            {
                ExecuteSelectedReplay(action);
            }
        }
    }
}
