using Heroes.Icons;
using HeroesMatchData.Data.Models.Replays;
using HeroesMatchData.Data.Queries.Replays;

namespace HeroesMatchData.Core.ViewModels.RawData
{
    public class RawRenamedPlayerViewModel : RawDataViewModelBase<ReplayRenamedPlayer>
    {
        public RawRenamedPlayerViewModel(IRawDataQueries<ReplayRenamedPlayer> iRawDataQueries, IHeroesIconsService heroesIcons)
            : base(iRawDataQueries, heroesIcons)
        { }
    }
}
