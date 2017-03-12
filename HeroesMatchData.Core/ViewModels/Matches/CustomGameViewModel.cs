using Heroes.ReplayParser;
using HeroesMatchData.Core.Messaging;
using HeroesMatchData.Core.Services;

namespace HeroesMatchData.Core.ViewModels.Matches
{
    public class CustomGameViewModel : MatchesBase
    {
        public CustomGameViewModel(IInternalService internalService, IWebsiteService website)
            : base(internalService, website, GameMode.Custom)
        { }

        protected override void ReceivedMatchSearchData(MatchesDataMessage message)
        {
            if (message.MatchTab == MatchesTab.Custom)
            {
                base.ReceivedMatchSearchData(message);
            }
        }
    }
}
