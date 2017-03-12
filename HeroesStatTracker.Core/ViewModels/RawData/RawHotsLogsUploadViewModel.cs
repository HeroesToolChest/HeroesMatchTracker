using Heroes.Icons;
using HeroesMatchData.Data.Models.Replays;
using HeroesMatchData.Data.Queries.Replays;

namespace HeroesMatchData.Core.ViewModels.RawData
{
    public class RawHotsLogsUploadViewModel : RawDataViewModelBase<ReplayHotsLogsUpload>
    {
        public RawHotsLogsUploadViewModel(IRawDataQueries<ReplayHotsLogsUpload> iRawDataQueries, IHeroesIconsService heroesIcons)
            : base(iRawDataQueries, heroesIcons)
        { }
    }
}
