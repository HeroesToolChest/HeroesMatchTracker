using Heroes.Icons;
using HeroesMatchTracker.Data.Models.Replays;
using HeroesMatchTracker.Data.Queries.Replays;

namespace HeroesMatchTracker.Core.ViewModels.RawData
{
    public class RawRenamedPlayerViewModel : RawDataViewModelBase<ReplayRenamedPlayer>
    {
        public RawRenamedPlayerViewModel(IRawDataQueries<ReplayRenamedPlayer> iRawDataQueries, IHeroesIcons heroesIcons)
            : base(iRawDataQueries, heroesIcons)
        { }
    }
}
