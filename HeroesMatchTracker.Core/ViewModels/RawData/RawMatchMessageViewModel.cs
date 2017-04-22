using Heroes.Icons;
using HeroesMatchTracker.Data.Models.Replays;
using HeroesMatchTracker.Data.Queries.Replays;

namespace HeroesMatchTracker.Core.ViewModels.RawData
{
    public class RawMatchMessageViewModel : RawDataViewModelBase<ReplayMatchMessage>
    {
        public RawMatchMessageViewModel(IRawDataQueries<ReplayMatchMessage> iRawDataQueries, IHeroesIconsService heroesIcons)
            : base(iRawDataQueries, heroesIcons)
        { }
    }
}
