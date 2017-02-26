using Heroes.Icons;
using HeroesStatTracker.Core.User;
using HeroesStatTracker.Data;
using HeroesStatTracker.Data.Models.Replays;

namespace HeroesStatTracker.Core.Models.MatchModels
{
    public class MatchObserver : MatchPlayerBase
    {
        public MatchObserver(MatchPlayerBase matchPlayerBase)
            : base(matchPlayerBase)
        { }

        public MatchObserver(IDatabaseService database, IHeroesIconsService heroesIcons, IUserProfileService userProfile, ReplayMatchPlayer player)
            : base(database, heroesIcons, userProfile, player)
        { }
    }
}
