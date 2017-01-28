using Heroes.Icons;
using HeroesStatTracker.Data.Models.Replays;
using HeroesStatTracker.Data.Queries.Replays;

namespace HeroesStatTracker.Core.ViewModels.RawData
{
    public class RawAllHotsPlayerHeroesViewModel : RawDataBase<ReplayAllHotsPlayerHero>
    {
        public RawAllHotsPlayerHeroesViewModel(IRawDataQueries<ReplayAllHotsPlayerHero> iRawDataQueries, IHeroesIconsService heroesIcons)
            : base(iRawDataQueries, heroesIcons)
        { }
    }
}
