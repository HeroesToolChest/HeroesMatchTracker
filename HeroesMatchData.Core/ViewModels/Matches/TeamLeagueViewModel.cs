using Heroes.ReplayParser;
using HeroesMatchData.Core.Messaging;
using HeroesMatchData.Core.Services;

namespace HeroesMatchData.Core.ViewModels.Matches
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
