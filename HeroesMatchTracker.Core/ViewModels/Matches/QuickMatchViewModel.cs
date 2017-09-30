using Heroes.Helpers;
using HeroesMatchTracker.Core.Messaging;
using HeroesMatchTracker.Core.Services;
using HeroesMatchTracker.Core.ViewServices;

namespace HeroesMatchTracker.Core.ViewModels.Matches
{
    public class QuickMatchViewModel : MatchesBase
    {
        public QuickMatchViewModel(IInternalService internalService, IWebsiteService website, IMatchesTabService matchesTab)
            : base(internalService, website, matchesTab, GameMode.QuickMatch, MatchesTab.QuickMatch)
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
