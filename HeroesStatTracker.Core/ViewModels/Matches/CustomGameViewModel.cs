using Heroes.ReplayParser;
using HeroesStatTracker.Core.Messaging;
using HeroesStatTracker.Core.Services;

namespace HeroesStatTracker.Core.ViewModels.Matches
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
