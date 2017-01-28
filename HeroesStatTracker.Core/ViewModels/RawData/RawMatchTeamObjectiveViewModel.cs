using HeroesStatTracker.Data.Models.Replays;
using HeroesStatTracker.Data.Queries.Replays;

namespace HeroesStatTracker.Core.ViewModels.RawData
{
    public class RawMatchTeamObjectiveViewModel : RawDataBase<ReplayMatchTeamObjective>
    {
        public RawMatchTeamObjectiveViewModel(IRawDataQueries<ReplayMatchTeamObjective> iRawDataQueries)
            : base(iRawDataQueries)
        { }
    }
}
