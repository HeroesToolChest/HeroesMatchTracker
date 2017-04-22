using Heroes.Icons;
using HeroesMatchTracker.Data.Models.Replays;
using HeroesMatchTracker.Data.Queries.Replays;

namespace HeroesMatchTracker.Core.ViewModels.RawData
{
    public class RawAllHotsPlayerViewModel : RawDataViewModelBase<ReplayAllHotsPlayer>
    {
        public RawAllHotsPlayerViewModel(IRawDataQueries<ReplayAllHotsPlayer> iRawDataQueries, IHeroesIconsService heroesIcons)
            : base(iRawDataQueries, heroesIcons)
        { }
    }
}
