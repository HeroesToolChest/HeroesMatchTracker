using HeroesStatTracker.Data.Models.Replays;
using HeroesStatTracker.Data.Queries.Replays;

namespace HeroesStatTracker.Core.ViewModels.RawData
{
    public class RawMatchPlayerViewModel : RawDataBase<ReplayMatchPlayer>
    {
        public RawMatchPlayerViewModel(IRawDataQueries<ReplayMatchPlayer> iRawDataQueries)
            : base(iRawDataQueries)
        { }
    }
}
