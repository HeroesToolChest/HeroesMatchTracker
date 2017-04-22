using Heroes.Icons;
using HeroesMatchTracker.Data.Models.Replays;
using HeroesMatchTracker.Data.Queries.Replays;

namespace HeroesMatchTracker.Core.ViewModels.RawData
{
    public class RawHotsLogsUploadViewModel : RawDataViewModelBase<ReplayHotsLogsUpload>
    {
        public RawHotsLogsUploadViewModel(IRawDataQueries<ReplayHotsLogsUpload> iRawDataQueries, IHeroesIconsService heroesIcons)
            : base(iRawDataQueries, heroesIcons)
        { }
    }
}
