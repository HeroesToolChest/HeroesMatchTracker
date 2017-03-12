using HeroesMatchData.Core.Services;
using HeroesMatchData.Data.Models.Replays;

namespace HeroesMatchData.Core.Models.MatchModels
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
