using Heroes.ReplayParser;
using HeroesStatTracker.Core.Messaging;
using HeroesStatTracker.Core.Services;

namespace HeroesStatTracker.Core.ViewModels.Matches
{
    public class HeroLeagueViewModel : MatchesBase
    {
        public HeroLeagueViewModel(IInternalService internalService, IWebsiteService website)
            : base(internalService, website, GameMode.HeroLeague)
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
