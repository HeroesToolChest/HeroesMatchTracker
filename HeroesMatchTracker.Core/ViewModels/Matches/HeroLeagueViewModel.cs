using Heroes.ReplayParser;
using HeroesMatchTracker.Core.Messaging;
using HeroesMatchTracker.Core.Services;
using HeroesMatchTracker.Core.ViewServices;

namespace HeroesMatchTracker.Core.ViewModels.Matches
{
    public class HeroLeagueViewModel : MatchesBase
    {
        public HeroLeagueViewModel(IInternalService internalService, IWebsiteService website, IMatchesTabService matchesTab)
            : base(internalService, website, matchesTab, GameMode.HeroLeague, MatchesTab.HeroLeague)
        {
        }

        protected override void ReceivedMatchSearchData(MatchesDataMessage message)
        {
            if (message.MatchTab == MatchesTab.HeroLeague)
            {
                base.ReceivedMatchSearchData(message);
            }
        }
    }
}
