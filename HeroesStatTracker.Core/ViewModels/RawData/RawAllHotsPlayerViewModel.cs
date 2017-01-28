using HeroesStatTracker.Data.Models.Replays;
using HeroesStatTracker.Data.Queries.Replays;

namespace HeroesStatTracker.Core.ViewModels.RawData
{
    public class RawAllHotsPlayerViewModel : RawDataBase<ReplayAllHotsPlayer>
    {
        public RawAllHotsPlayerViewModel(IRawDataQueries<ReplayAllHotsPlayer> iRawDataQueries)
            : base(iRawDataQueries)
        { }
    }
}
