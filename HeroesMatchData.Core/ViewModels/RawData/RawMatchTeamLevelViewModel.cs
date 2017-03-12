using Heroes.Icons;
using HeroesMatchData.Data.Models.Replays;
using HeroesMatchData.Data.Queries.Replays;

namespace HeroesMatchData.Core.ViewModels.RawData
{
    public class RawMatchTeamLevelViewModel : RawDataViewModelBase<ReplayMatchTeamLevel>
    {
        public RawMatchTeamLevelViewModel(IRawDataQueries<ReplayMatchTeamLevel> iRawDataQueries, IHeroesIconsService heroesIcons)
            : base(iRawDataQueries, heroesIcons)
        { }
    }
}
