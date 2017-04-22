using Heroes.ReplayParser;
using HeroesMatchTracker.Core.Messaging;
using HeroesMatchTracker.Core.Services;
using HeroesMatchTracker.Core.ViewServices;

namespace HeroesMatchTracker.Core.ViewModels.Matches
{
    public class TeamLeagueViewModel : MatchesBase
    {
        public TeamLeagueViewModel(IInternalService internalService, IWebsiteService website, IMatchesTabService matchesTab)
            : base(internalService, website, matchesTab, GameMode.TeamLeague, MatchesTab.TeamLeague)
        { }

        protected override void ReceivedMatchSearchData(MatchesDataMessage message)
        {
            if (message.MatchTab == MatchesTab.TeamLeague)
            {
                base.ReceivedMatchSearchData(message);
            }
        }
    }
}
