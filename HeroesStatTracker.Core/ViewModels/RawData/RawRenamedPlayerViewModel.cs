using Heroes.Icons;
using HeroesStatTracker.Data.Models.Replays;
using HeroesStatTracker.Data.Queries.Replays;

namespace HeroesStatTracker.Core.ViewModels.RawData
{
    public class RawRenamedPlayerViewModel : RawDataViewModelBase<ReplayRenamedPlayer>
    {
        public RawRenamedPlayerViewModel(IRawDataQueries<ReplayRenamedPlayer> iRawDataQueries, IHeroesIconsService heroesIcons)
            : base(iRawDataQueries, heroesIcons)
        { }
    }
}
