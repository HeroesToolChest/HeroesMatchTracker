using Heroes.Icons;
using HeroesMatchTracker.Data.Models.Replays;
using HeroesMatchTracker.Data.Queries.Replays;

namespace HeroesMatchTracker.Core.ViewModels.RawData
{
    public class RawMatchPlayerTalentViewModel : RawDataViewModelBase<ReplayMatchPlayerTalent>
    {
        public RawMatchPlayerTalentViewModel(IRawDataQueries<ReplayMatchPlayerTalent> iRawDataQueries, IHeroesIconsService heroesIcons)
            : base(iRawDataQueries, heroesIcons)
        { }
    }
}
