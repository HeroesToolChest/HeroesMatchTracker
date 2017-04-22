using Heroes.ReplayParser;
using HeroesMatchTracker.Core.Messaging;
using HeroesMatchTracker.Core.Services;
using HeroesMatchTracker.Core.ViewServices;

namespace HeroesMatchTracker.Core.ViewModels.Matches
{
    public class BrawlViewModel : MatchesBase
    {
        public BrawlViewModel(IInternalService internalService, IWebsiteService website, IMatchesTabService matchesTab)
            : base(internalService, website, matchesTab, GameMode.Brawl, MatchesTab.Brawl)
        { }

        protected override void ReceivedMatchSearchData(MatchesDataMessage message)
        {
            if (message.MatchTab == MatchesTab.Brawl)
            {
                base.ReceivedMatchSearchData(message);
            }
        }
    }
}
