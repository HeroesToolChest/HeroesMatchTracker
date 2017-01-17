using HeroesStatTracker.Data.Models.Replays;
using HeroesStatTracker.Data.Queries.Replays;

namespace HeroesStatTracker.Core.ViewModels.RawData
{
    public class RawMatchTeamLevelViewModel : RawDataContextBase<ReplayMatchTeamLevel>
    {
        public RawMatchTeamLevelViewModel(IRawDataQueries<ReplayMatchTeamLevel> iRawDataQueries)
            : base(iRawDataQueries)
        { }
    }
}
