using Heroes.Icons;
using HeroesMatchTracker.Data.Models.Replays;
using HeroesMatchTracker.Data.Queries.Replays;

namespace HeroesMatchTracker.Core.ViewModels.RawData
{
    public class RawHotsApiUploadViewModel : RawDataViewModelBase<ReplayHotsApiUpload>
    {
        public RawHotsApiUploadViewModel(IRawDataQueries<ReplayHotsApiUpload> iRawDataQueries, IHeroesIcons heroesIcons)
            : base(iRawDataQueries, heroesIcons)
        { }
    }
}
