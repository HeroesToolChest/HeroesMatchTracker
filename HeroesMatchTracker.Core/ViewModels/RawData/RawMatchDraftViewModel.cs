using Heroes.Icons;
using HeroesMatchTracker.Data.Models.Replays;
using HeroesMatchTracker.Data.Queries.Replays;

namespace HeroesMatchTracker.Core.ViewModels.RawData
{
    public class RawMatchDraftViewModel : RawDataViewModelBase<ReplayMatchDraftPick>
    {
        public RawMatchDraftViewModel(IRawDataQueries<ReplayMatchDraftPick> iRawDataQueries, IHeroesIcons heroesIcons) 
            : base(iRawDataQueries, heroesIcons)
        {
        }
    }
}
