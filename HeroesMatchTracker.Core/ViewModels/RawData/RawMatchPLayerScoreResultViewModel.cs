using Heroes.Icons;
using HeroesMatchTracker.Data.Models.Replays;
using HeroesMatchTracker.Data.Queries.Replays;

namespace HeroesMatchTracker.Core.ViewModels.RawData
{
    public class RawMatchPlayerScoreResultViewModel : RawDataViewModelBase<ReplayMatchPlayerScoreResult>
    {
        public RawMatchPlayerScoreResultViewModel(IRawDataQueries<ReplayMatchPlayerScoreResult> iRawDataQueries, IHeroesIcons heroesIcons)
            : base(iRawDataQueries, heroesIcons)
        { }
    }
}
