using Heroes.ReplayParser;
using HeroesMatchData.Core.Messaging;
using HeroesMatchData.Core.Services;

namespace HeroesMatchData.Core.ViewModels.Matches
{
    public class BrawlViewModel : MatchesBase
    {
        public BrawlViewModel(IInternalService internalService, IWebsiteService website)
            : base(internalService, website, GameMode.Brawl)
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
