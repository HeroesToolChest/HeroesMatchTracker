using Heroes.Icons;
using HeroesStatTracker.Data;
using HeroesStatTracker.Data.Models.Replays;

namespace HeroesStatTracker.Core.Models.MatchModels
{
    public class MatchObserver : MatchPlayerBase
    {
        public MatchObserver(MatchPlayerBase matchPlayerBase)
            : base(matchPlayerBase)
        { }

        public MatchObserver(IDatabaseService database, IHeroesIconsService heroesIcons, ReplayMatchPlayer player)
            : base(database, heroesIcons, player)
        { }
    }
}
