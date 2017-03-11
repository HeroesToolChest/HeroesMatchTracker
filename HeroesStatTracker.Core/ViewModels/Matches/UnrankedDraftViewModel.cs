using Heroes.ReplayParser;
using HeroesStatTracker.Core.Messaging;
using HeroesStatTracker.Core.Services;

namespace HeroesStatTracker.Core.ViewModels.Matches
{
    public class UnrankedDraftViewModel : MatchesBase
    {
        public UnrankedDraftViewModel(IInternalService internalService, IWebsiteService website)
            : base(internalService, website, GameMode.UnrankedDraft)
        { }

        protected override void ReceivedMatchSearchData(MatchesDataMessage message)
        {
            if (message.MatchTab == MatchesTab.UnrankedDraft)
            {
                base.ReceivedMatchSearchData(message);
            }
        }
    }
}
