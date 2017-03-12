using Heroes.ReplayParser;
using HeroesMatchData.Core.Messaging;
using HeroesMatchData.Core.Services;

namespace HeroesMatchData.Core.ViewModels.Matches
{
    public class QuickMatchViewModel : MatchesBase
    {
        public QuickMatchViewModel(IInternalService internalService, IWebsiteService website)
            : base(internalService, website, GameMode.QuickMatch)
        { }

        protected override void ReceivedMatchSearchData(MatchesDataMessage message)
        {
            if (message.MatchTab == MatchesTab.QuickMatch)
            {
                base.ReceivedMatchSearchData(message);
            }
        }
    }
}
