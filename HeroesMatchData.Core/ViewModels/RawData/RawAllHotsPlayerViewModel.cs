using Heroes.Icons;
using HeroesMatchData.Data.Models.Replays;
using HeroesMatchData.Data.Queries.Replays;

namespace HeroesMatchData.Core.ViewModels.RawData
{
    public class RawAllHotsPlayerViewModel : RawDataViewModelBase<ReplayAllHotsPlayer>
    {
        public RawAllHotsPlayerViewModel(IRawDataQueries<ReplayAllHotsPlayer> iRawDataQueries, IHeroesIconsService heroesIcons)
            : base(iRawDataQueries, heroesIcons)
        { }
    }
}
