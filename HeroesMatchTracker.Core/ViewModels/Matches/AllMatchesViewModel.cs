using Heroes.ReplayParser;
using HeroesMatchTracker.Core.Messaging;
using HeroesMatchTracker.Core.Services;
using HeroesMatchTracker.Core.ViewServices;

namespace HeroesMatchTracker.Core.ViewModels.Matches
{
    public class AllMatchesViewModel : MatchesBase
    {
        public AllMatchesViewModel(IInternalService internalService, IWebsiteService website, IMatchesTabService matchesTab)
            : base(internalService, website, matchesTab, GameMode.Brawl | GameMode.Custom | GameMode.HeroLeague | GameMode.QuickMatch | GameMode.TeamLeague | GameMode.UnrankedDraft, MatchesTab.AllMatches)
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
