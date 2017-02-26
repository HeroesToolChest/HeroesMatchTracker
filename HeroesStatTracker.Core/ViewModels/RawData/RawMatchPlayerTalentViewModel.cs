using Heroes.Icons;
using HeroesStatTracker.Data.Models.Replays;
using HeroesStatTracker.Data.Queries.Replays;

namespace HeroesStatTracker.Core.ViewModels.RawData
{
    public class RawMatchPlayerTalentViewModel : RawDataViewModelBase<ReplayMatchPlayerTalent>
    {
        public RawMatchPlayerTalentViewModel(IRawDataQueries<ReplayMatchPlayerTalent> iRawDataQueries, IHeroesIconsService heroesIcons)
            : base(iRawDataQueries, heroesIcons)
        { }
    }
}
