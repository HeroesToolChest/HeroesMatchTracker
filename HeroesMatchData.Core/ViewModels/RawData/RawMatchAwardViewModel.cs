using Heroes.Icons;
using HeroesMatchData.Data.Models.Replays;
using HeroesMatchData.Data.Queries.Replays;

namespace HeroesMatchData.Core.ViewModels.RawData
{
    public class RawMatchAwardViewModel : RawDataViewModelBase<ReplayMatchAward>
    {
        public RawMatchAwardViewModel(IRawDataQueries<ReplayMatchAward> iRawDataQueries, IHeroesIconsService heroesIcons)
            : base(iRawDataQueries, heroesIcons)
        { }
    }
}
