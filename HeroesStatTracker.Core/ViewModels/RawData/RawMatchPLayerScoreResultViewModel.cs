using Heroes.Icons;
using HeroesStatTracker.Data.Models.Replays;
using HeroesStatTracker.Data.Queries.Replays;

namespace HeroesStatTracker.Core.ViewModels.RawData
{
    public class RawMatchPlayerScoreResultViewModel : RawDataBase<ReplayMatchPlayerScoreResult>
    {
        public RawMatchPlayerScoreResultViewModel(IRawDataQueries<ReplayMatchPlayerScoreResult> iRawDataQueries, IHeroesIconsService heroesIcons)
            : base(iRawDataQueries, heroesIcons)
        { }
    }
}
