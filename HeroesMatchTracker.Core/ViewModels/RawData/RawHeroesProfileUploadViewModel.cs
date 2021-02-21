using Heroes.Icons;
using HeroesMatchTracker.Data.Models.Replays;
using HeroesMatchTracker.Data.Queries.Replays;

namespace HeroesMatchTracker.Core.ViewModels.RawData
{
    public class RawHeroesProfileUploadViewModel : RawDataViewModelBase<ReplayHeroesProfileUpload>
    {
        public RawHeroesProfileUploadViewModel(IRawDataQueries<ReplayHeroesProfileUpload> iRawDataQueries, IHeroesIcons heroesIcons)
            : base(iRawDataQueries, heroesIcons)
        { }
    }
}
