using Heroes.Icons;
using HeroesMatchTracker.Data.Models.Replays;
using HeroesMatchTracker.Data.Queries.Replays;

namespace HeroesMatchTracker.Core.ViewModels.RawData
{
    public class RawMatchTeamExperienceViewModel : RawDataViewModelBase<ReplayMatchTeamExperience>
    {
        public RawMatchTeamExperienceViewModel(IRawDataQueries<ReplayMatchTeamExperience> iRawDataQueries, IHeroesIconsService heroesIcons)
            : base(iRawDataQueries, heroesIcons)
        { }
    }
}
