using HeroesStatTracker.Data.Models.Replays;
using HeroesStatTracker.Data.Queries.Replays;

namespace HeroesStatTracker.Core.ViewModels.RawData
{
    public class RawMatchMessageViewModel : RawDataContextBase<ReplayMatchMessage>
    {
        public RawMatchMessageViewModel(IRawDataQueries<ReplayMatchMessage> iRawDataQueries)
            : base(iRawDataQueries)
        { }
    }
}
