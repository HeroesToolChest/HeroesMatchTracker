using Heroes.Icons;
using HeroesMatchTracker.Data.Models.Replays;
using HeroesMatchTracker.Data.Queries.Replays;

namespace HeroesMatchTracker.Core.ViewModels.RawData
{
    public class RawRenamedPlayerViewModel : RawDataViewModelBase<ReplayRenamedPlayer>
    {
        public RawRenamedPlayerViewModel(IRawDataQueries<ReplayRenamedPlayer> iRawDataQueries, IHeroesIconsService heroesIcons)
            : base(iRawDataQueries, heroesIcons)
        { }
    }
}
