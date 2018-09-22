using Heroes.Icons;
using HeroesMatchTracker.Data.Models.Replays;
using HeroesMatchTracker.Data.Queries.Replays;

namespace HeroesMatchTracker.Core.ViewModels.RawData
{
    public class RawMatchTeamLevelViewModel : RawDataViewModelBase<ReplayMatchTeamLevel>
    {
        public RawMatchTeamLevelViewModel(IRawDataQueries<ReplayMatchTeamLevel> iRawDataQueries, IHeroesIcons heroesIcons)
            : base(iRawDataQueries, heroesIcons)
        { }
    }
}
