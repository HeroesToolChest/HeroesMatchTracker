using Heroes.ReplayParser;
using HeroesStatTracker.Core.Messaging;
using HeroesStatTracker.Core.Services;

namespace HeroesStatTracker.Core.ViewModels.Matches
{
    public class AllMatchesViewModel : MatchesBase
    {
        public AllMatchesViewModel(IInternalService internalService, IWebsiteService website)
            : base(internalService, website, GameMode.Brawl | GameMode.Custom | GameMode.HeroLeague | GameMode.QuickMatch | GameMode.TeamLeague | GameMode.UnrankedDraft)
        { }

        public void SendSearchData(MatchesDataMessage matchesDataMessage)
        {
            if (!string.IsNullOrEmpty(matchesDataMessage.SelectedCharacter))
                SelectedCharacter = matchesDataMessage.SelectedCharacter;

            if (!string.IsNullOrEmpty(matchesDataMessage.SelectedBattleTagName))
                SelectedPlayerBattleTag = matchesDataMessage.SelectedBattleTagName;

            if (!string.IsNullOrEmpty(matchesDataMessage.SelectedCharacter) && !string.IsNullOrEmpty(matchesDataMessage.SelectedBattleTagName))
                IsGivenBattleTagOnlyChecked = true;
        }

        protected override void ReceivedMatchSearchData(MatchesDataMessage message)
        {
            if (message.MatchTab == MatchesTab.AllMatches)
            {
                base.ReceivedMatchSearchData(message);
            }
        }
    }
}
