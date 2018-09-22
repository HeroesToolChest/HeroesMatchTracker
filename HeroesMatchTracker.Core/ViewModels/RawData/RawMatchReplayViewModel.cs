using Heroes.Icons;
using HeroesMatchTracker.Data.Models.Replays;
using HeroesMatchTracker.Data.Queries.Replays;

namespace HeroesMatchTracker.Core.ViewModels.RawData
{
    public class RawMatchReplayViewModel : RawDataViewModelBase<ReplayMatch>
    {
        public RawMatchReplayViewModel(IRawDataQueries<ReplayMatch> iRawDataQueries, IHeroesIcons heroesIcons)
            : base(iRawDataQueries, heroesIcons)
        { }
    }
}
