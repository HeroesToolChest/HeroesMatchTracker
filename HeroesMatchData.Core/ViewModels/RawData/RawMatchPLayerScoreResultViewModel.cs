using Heroes.Icons;
using HeroesMatchData.Data.Models.Replays;
using HeroesMatchData.Data.Queries.Replays;

namespace HeroesMatchData.Core.ViewModels.RawData
{
    public class RawMatchPlayerScoreResultViewModel : RawDataViewModelBase<ReplayMatchPlayerScoreResult>
    {
        public RawMatchPlayerScoreResultViewModel(IRawDataQueries<ReplayMatchPlayerScoreResult> iRawDataQueries, IHeroesIconsService heroesIcons)
            : base(iRawDataQueries, heroesIcons)
        { }
    }
}
