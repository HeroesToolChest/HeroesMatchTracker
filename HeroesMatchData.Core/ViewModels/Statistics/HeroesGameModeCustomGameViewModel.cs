using HeroesMatchData.Core.Services;

namespace HeroesMatchData.Core.ViewModels.Statistics
{
    public class HeroesGameModeCustomGameViewModel : HeroesGameModeViewModel
    {
        public HeroesGameModeCustomGameViewModel(IInternalService internalService)
            : base(internalService)
        {
        }
    }
}
