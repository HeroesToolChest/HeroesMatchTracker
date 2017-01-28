using Heroes.Icons;
using HeroesStatTracker.Data.Models.Replays;
using HeroesStatTracker.Data.Queries.Replays;

namespace HeroesStatTracker.Core.ViewModels.RawData
{
    public class RawAllHotsPlayerViewModel : RawDataBase<ReplayAllHotsPlayer>
    {
        public RawAllHotsPlayerViewModel(IRawDataQueries<ReplayAllHotsPlayer> iRawDataQueries, IHeroesIconsService heroesIcons)
            : base(iRawDataQueries, heroesIcons)
        { }
    }
}
