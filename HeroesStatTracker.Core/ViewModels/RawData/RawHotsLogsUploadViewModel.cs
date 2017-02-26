using Heroes.Icons;
using HeroesStatTracker.Data.Models.Replays;
using HeroesStatTracker.Data.Queries.Replays;

namespace HeroesStatTracker.Core.ViewModels.RawData
{
    public class RawHotsLogsUploadViewModel : RawDataViewModelBase<ReplayHotsLogsUpload>
    {
        public RawHotsLogsUploadViewModel(IRawDataQueries<ReplayHotsLogsUpload> iRawDataQueries, IHeroesIconsService heroesIcons)
            : base(iRawDataQueries, heroesIcons)
        { }
    }
}
