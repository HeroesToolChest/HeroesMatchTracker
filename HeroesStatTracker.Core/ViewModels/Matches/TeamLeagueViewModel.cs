using Heroes.ReplayParser;
using HeroesStatTracker.Core.Messaging;
using HeroesStatTracker.Core.Services;

namespace HeroesStatTracker.Core.ViewModels.Matches
{
    public class TeamLeagueViewModel : MatchesBase
    {
        public TeamLeagueViewModel(IInternalService internalService, IWebsiteService website)
            : base(internalService, website, GameMode.TeamLeague)
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
