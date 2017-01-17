using HeroesStatTracker.Data.Models.Replays;
using HeroesStatTracker.Data.Queries.Replays;

namespace HeroesStatTracker.Core.ViewModels.RawData
{
    public class RawHotsLogsUploadViewModel : RawDataContextBase<ReplayHotsLogsUpload>
    {
        public RawHotsLogsUploadViewModel(IRawDataQueries<ReplayHotsLogsUpload> iRawDataQueries)
            : base(iRawDataQueries)
        { }
    }
}
