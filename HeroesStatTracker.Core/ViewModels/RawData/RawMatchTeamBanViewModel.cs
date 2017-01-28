using Heroes.Icons;
using HeroesStatTracker.Data.Models.Replays;
using HeroesStatTracker.Data.Queries.Replays;

namespace HeroesStatTracker.Core.ViewModels.RawData
{
    public class RawMatchTeamBanViewModel : RawDataBase<ReplayMatchTeamBan>
    {
        public RawMatchTeamBanViewModel(IRawDataQueries<ReplayMatchTeamBan> iRawDataQueries, IHeroesIconsService heroesIcons)
            : base(iRawDataQueries, heroesIcons)
        { }
    }
}
