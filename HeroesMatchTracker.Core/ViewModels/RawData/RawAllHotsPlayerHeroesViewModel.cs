using Heroes.Icons;
using HeroesMatchTracker.Data.Models.Replays;
using HeroesMatchTracker.Data.Queries.Replays;

namespace HeroesMatchTracker.Core.ViewModels.RawData
{
    public class RawAllHotsPlayerHeroesViewModel : RawDataViewModelBase<ReplayAllHotsPlayerHero>
    {
        public RawAllHotsPlayerHeroesViewModel(IRawDataQueries<ReplayAllHotsPlayerHero> iRawDataQueries, IHeroesIconsService heroesIcons)
            : base(iRawDataQueries, heroesIcons)
        { }
    }
}
