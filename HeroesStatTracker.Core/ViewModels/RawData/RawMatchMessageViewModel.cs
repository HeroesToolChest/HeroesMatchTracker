using Heroes.Icons;
using HeroesStatTracker.Data.Models.Replays;
using HeroesStatTracker.Data.Queries.Replays;

namespace HeroesStatTracker.Core.ViewModels.RawData
{
    public class RawMatchMessageViewModel : RawDataViewModelBase<ReplayMatchMessage>
    {
        public RawMatchMessageViewModel(IRawDataQueries<ReplayMatchMessage> iRawDataQueries, IHeroesIconsService heroesIcons)
            : base(iRawDataQueries, heroesIcons)
        { }
    }
}
