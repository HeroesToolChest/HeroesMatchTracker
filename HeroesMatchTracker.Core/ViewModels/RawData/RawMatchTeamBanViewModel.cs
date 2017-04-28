using Heroes.Icons;
using HeroesMatchTracker.Data.Models.Replays;
using HeroesMatchTracker.Data.Queries.Replays;

namespace HeroesMatchTracker.Core.ViewModels.RawData
{
    public class RawMatchTeamBanViewModel : RawDataViewModelBase<ReplayMatchTeamBan>
    {
        public RawMatchTeamBanViewModel(IRawDataQueries<ReplayMatchTeamBan> iRawDataQueries, IHeroesIconsService heroesIcons)
            : base(iRawDataQueries, heroesIcons)
        { }
    }
}
