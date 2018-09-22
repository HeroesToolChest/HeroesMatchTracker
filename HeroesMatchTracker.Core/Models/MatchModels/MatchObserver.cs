using HeroesMatchTracker.Core.Services;
using HeroesMatchTracker.Data.Models.Replays;

namespace HeroesMatchTracker.Core.Models.MatchModels
{
    public class MatchObserver : MatchPlayerBase
    {
        public MatchObserver(MatchPlayerBase matchPlayerBase)
            : base(matchPlayerBase)
        { }

        public MatchObserver(IInternalService internalService, IWebsiteService website, ReplayMatchPlayer player, int build)
            : base(internalService, website, player, build)
        { }
    }
}
