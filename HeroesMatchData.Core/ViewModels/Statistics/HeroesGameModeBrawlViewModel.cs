using HeroesMatchData.Core.Services;

namespace HeroesMatchData.Core.ViewModels.Statistics
{
    public class HeroesGameModeBrawlViewModel : HeroesGameModeViewModel
    {
        public HeroesGameModeBrawlViewModel(IInternalService internalService)
            : base(internalService)
        {
        }
    }
}
