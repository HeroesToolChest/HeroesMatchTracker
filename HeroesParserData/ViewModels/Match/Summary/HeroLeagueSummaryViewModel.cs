using HeroesParserData.Messages;

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
            if (action.MatchSummary == MatchSummary.HeroLeague)
            {
                ExecuteSelectedReplay(action);
            }
        }
    }
}
