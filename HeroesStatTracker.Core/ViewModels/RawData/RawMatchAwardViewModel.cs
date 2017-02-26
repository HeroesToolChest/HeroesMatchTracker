using Heroes.Icons;
using HeroesStatTracker.Data.Models.Replays;
using HeroesStatTracker.Data.Queries.Replays;

namespace HeroesStatTracker.Core.ViewModels.RawData
{
    public class RawMatchAwardViewModel : RawDataViewModelBase<ReplayMatchAward>
    {
        public RawMatchAwardViewModel(IRawDataQueries<ReplayMatchAward> iRawDataQueries, IHeroesIconsService heroesIcons)
            : base(iRawDataQueries, heroesIcons)
        { }
    }
}
