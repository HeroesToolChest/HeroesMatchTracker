using Heroes.Icons;
using HeroesMatchData.Data.Models.Replays;
using HeroesMatchData.Data.Queries.Replays;

namespace HeroesMatchData.Core.ViewModels.RawData
{
    public class RawMatchReplayViewModel : RawDataViewModelBase<ReplayMatch>
    {
        public RawMatchReplayViewModel(IRawDataQueries<ReplayMatch> iRawDataQueries, IHeroesIconsService heroesIcons)
            : base(iRawDataQueries, heroesIcons)
        { }
    }
}
