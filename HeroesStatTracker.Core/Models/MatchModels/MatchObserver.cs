using HeroesStatTracker.Core.Services;
using HeroesStatTracker.Data.Models.Replays;

namespace HeroesStatTracker.Core.Models.MatchModels
{
    public class MatchObserver : MatchPlayerBase
    {
        public MatchObserver(MatchPlayerBase matchPlayerBase)
            : base(matchPlayerBase)
        { }

        public MatchObserver(IInternalService internalService, IWebsiteService website, ReplayMatchPlayer player)
            : base(internalService, website, player)
        { }
    }
}
