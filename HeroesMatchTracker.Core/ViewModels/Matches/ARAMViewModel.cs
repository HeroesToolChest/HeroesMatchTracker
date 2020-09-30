using Heroes.Helpers;
using HeroesMatchTracker.Core.Messaging;
using HeroesMatchTracker.Core.Services;
using HeroesMatchTracker.Core.ViewServices;

namespace HeroesMatchTracker.Core.ViewModels.Matches
{
    public class ARAMViewModel : MatchesBase
    {
        public ARAMViewModel(IInternalService internalService, IWebsiteService website, IMatchesTabService matchesTab)
            : base(internalService, website, matchesTab, GameMode.ARAM, MatchesTab.ARAM)
        { }

        protected override void ReceivedMatchSearchData(MatchesDataMessage message)
        {
            if (message.MatchTab == MatchesTab.ARAM)
            {
                base.ReceivedMatchSearchData(message);
            }
        }
    }
}
