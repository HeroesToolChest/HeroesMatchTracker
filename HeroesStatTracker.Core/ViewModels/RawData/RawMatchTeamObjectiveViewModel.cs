using Heroes.Icons;
using HeroesStatTracker.Data.Models.Replays;
using HeroesStatTracker.Data.Queries.Replays;

namespace HeroesStatTracker.Core.ViewModels.RawData
{
    public class RawMatchTeamObjectiveViewModel : RawDataViewModelBase<ReplayMatchTeamObjective>
    {
        public RawMatchTeamObjectiveViewModel(IRawDataQueries<ReplayMatchTeamObjective> iRawDataQueries, IHeroesIconsService heroesIcons)
            : base(iRawDataQueries, heroesIcons)
        { }
    }
}
