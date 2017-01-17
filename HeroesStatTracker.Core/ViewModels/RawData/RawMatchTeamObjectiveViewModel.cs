using HeroesStatTracker.Data.Models.Replays;
using HeroesStatTracker.Data.Queries.Replays;

namespace HeroesStatTracker.Core.ViewModels.RawData
{
    public class RawMatchTeamObjectiveViewModel : RawDataContextBase<ReplayMatchTeamObjective>
    {
        public RawMatchTeamObjectiveViewModel(IRawDataQueries<ReplayMatchTeamObjective> iRawDataQueries)
            : base(iRawDataQueries)
        { }
    }
}
