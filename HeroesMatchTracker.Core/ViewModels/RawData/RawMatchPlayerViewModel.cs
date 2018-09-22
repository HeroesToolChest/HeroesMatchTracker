using Heroes.Icons;
using HeroesMatchTracker.Data.Models.Replays;
using HeroesMatchTracker.Data.Queries.Replays;

namespace HeroesMatchTracker.Core.ViewModels.RawData
{
    public class RawMatchPlayerViewModel : RawDataViewModelBase<ReplayMatchPlayer>
    {
        public RawMatchPlayerViewModel(IRawDataQueries<ReplayMatchPlayer> iRawDataQueries, IHeroesIcons heroesIcons)
            : base(iRawDataQueries, heroesIcons)
        { }
    }
}
