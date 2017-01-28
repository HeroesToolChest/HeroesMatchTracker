using Heroes.Icons;
using HeroesStatTracker.Data.Models.Replays;
using HeroesStatTracker.Data.Queries.Replays;

namespace HeroesStatTracker.Core.ViewModels.RawData
{
    public class RawHotsLogsUploadViewModel : RawDataBase<ReplayHotsLogsUpload>
    {
        public RawHotsLogsUploadViewModel(IRawDataQueries<ReplayHotsLogsUpload> iRawDataQueries, IHeroesIconsService heroesIcons)
            : base(iRawDataQueries, heroesIcons)
        { }
    }
}
