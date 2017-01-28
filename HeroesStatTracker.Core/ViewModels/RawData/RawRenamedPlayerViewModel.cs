using HeroesStatTracker.Data.Models.Replays;
using HeroesStatTracker.Data.Queries.Replays;

namespace HeroesStatTracker.Core.ViewModels.RawData
{
    public class RawRenamedPlayerViewModel : RawDataBase<ReplayRenamedPlayer>
    {
        public RawRenamedPlayerViewModel(IRawDataQueries<ReplayRenamedPlayer> iRawDataQueries)
            : base(iRawDataQueries)
        { }
    }
}
