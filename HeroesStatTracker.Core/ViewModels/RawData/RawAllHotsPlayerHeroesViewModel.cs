using Heroes.Icons;
using HeroesMatchData.Data.Models.Replays;
using HeroesMatchData.Data.Queries.Replays;

namespace HeroesMatchData.Core.ViewModels.RawData
{
    public class RawAllHotsPlayerHeroesViewModel : RawDataViewModelBase<ReplayAllHotsPlayerHero>
    {
        public RawAllHotsPlayerHeroesViewModel(IRawDataQueries<ReplayAllHotsPlayerHero> iRawDataQueries, IHeroesIconsService heroesIcons)
            : base(iRawDataQueries, heroesIcons)
        { }
    }
}
