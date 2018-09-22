using Heroes.Icons;
using HeroesMatchTracker.Data.Models.Replays;
using HeroesMatchTracker.Data.Queries.Replays;

namespace HeroesMatchTracker.Core.ViewModels.RawData
{
    public class RawMatchAwardViewModel : RawDataViewModelBase<ReplayMatchAward>
    {
        public RawMatchAwardViewModel(IRawDataQueries<ReplayMatchAward> iRawDataQueries, IHeroesIcons heroesIcons)
            : base(iRawDataQueries, heroesIcons)
        { }
    }
}
