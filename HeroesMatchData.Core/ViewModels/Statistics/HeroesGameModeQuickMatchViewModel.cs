using HeroesMatchData.Core.Services;

namespace HeroesMatchData.Core.ViewModels.Statistics
{
    public class HeroesGameModeQuickMatchViewModel : HeroesGameModeViewModel
    {
        public HeroesGameModeQuickMatchViewModel(IInternalService internalService)
            : base(internalService)
        {
        }
    }
}
