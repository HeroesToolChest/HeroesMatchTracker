using HeroesStatTracker.Data.Models.Replays;
using HeroesStatTracker.Data.Queries.Replays;

namespace HeroesStatTracker.Core.ViewModels.RawData
{
    public class RawAllHotsPlayerHeroesViewModel : RawDataContextBase<ReplayAllHotsPlayerHero>
    {
        public RawAllHotsPlayerHeroesViewModel(IRawDataQueries<ReplayAllHotsPlayerHero> iRawDataQueries)
            : base(iRawDataQueries)
        { }
    }
}
