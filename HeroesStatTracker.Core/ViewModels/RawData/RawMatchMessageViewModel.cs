using Heroes.Icons;
using HeroesMatchData.Data.Models.Replays;
using HeroesMatchData.Data.Queries.Replays;

namespace HeroesMatchData.Core.ViewModels.RawData
{
    public class RawMatchMessageViewModel : RawDataViewModelBase<ReplayMatchMessage>
    {
        public RawMatchMessageViewModel(IRawDataQueries<ReplayMatchMessage> iRawDataQueries, IHeroesIconsService heroesIcons)
            : base(iRawDataQueries, heroesIcons)
        { }
    }
}
