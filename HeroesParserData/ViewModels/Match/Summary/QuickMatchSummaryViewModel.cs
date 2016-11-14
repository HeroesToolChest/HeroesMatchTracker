using GalaSoft.MvvmLight.Messaging;
using HeroesParserData.Messages;

namespace HeroesParserData.ViewModels.Match.Summary
{
    public class QuickMatchSummaryViewModel : MatchSummaryContext
    {
        public QuickMatchSummaryViewModel()
            :base()
        {
            Messenger.Default.Register<MatchSummaryMessage>(this, (action) => ReceiveMessage(action));

            HasObservers = false;
            HasBans = false;
        }

        private void ReceiveMessage(MatchSummaryMessage action)
        {
            if (action.MatchSummary == MatchSummary.QuickMatch)
            {
                QuerySummaryDetails(action.ReplayId);
            }
        }
    }
}
