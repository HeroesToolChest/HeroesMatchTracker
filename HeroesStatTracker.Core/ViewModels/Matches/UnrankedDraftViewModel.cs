using Heroes.Icons;
using Heroes.ReplayParser;
using HeroesStatTracker.Core.Messaging;
using HeroesStatTracker.Core.User;
using HeroesStatTracker.Data;

namespace HeroesStatTracker.Core.ViewModels.Matches
{
    public class UnrankedDraftViewModel : MatchesBase
    {
        public UnrankedDraftViewModel(IDatabaseService database, IHeroesIconsService heroesIcons, IUserProfileService userProfile)
            : base(database, heroesIcons, userProfile, GameMode.UnrankedDraft)
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
