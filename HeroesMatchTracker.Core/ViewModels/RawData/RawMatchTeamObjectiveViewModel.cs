using Heroes.Icons;
using HeroesMatchTracker.Data.Models.Replays;
using HeroesMatchTracker.Data.Queries.Replays;

namespace HeroesMatchTracker.Core.ViewModels.RawData
{
    public class RawMatchTeamObjectiveViewModel : RawDataViewModelBase<ReplayMatchTeamObjective>
    {
        public RawMatchTeamObjectiveViewModel(IRawDataQueries<ReplayMatchTeamObjective> iRawDataQueries, IHeroesIcons heroesIcons)
            : base(iRawDataQueries, heroesIcons)
        { }
    }
}
