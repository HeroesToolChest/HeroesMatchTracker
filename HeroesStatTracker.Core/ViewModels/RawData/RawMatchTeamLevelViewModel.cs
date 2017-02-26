using Heroes.Icons;
using HeroesStatTracker.Data.Models.Replays;
using HeroesStatTracker.Data.Queries.Replays;

namespace HeroesStatTracker.Core.ViewModels.RawData
{
    public class RawMatchTeamLevelViewModel : RawDataViewModelBase<ReplayMatchTeamLevel>
    {
        public RawMatchTeamLevelViewModel(IRawDataQueries<ReplayMatchTeamLevel> iRawDataQueries, IHeroesIconsService heroesIcons)
            : base(iRawDataQueries, heroesIcons)
        { }
    }
}
