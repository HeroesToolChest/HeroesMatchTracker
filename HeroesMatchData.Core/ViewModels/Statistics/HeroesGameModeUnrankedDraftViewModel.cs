using HeroesMatchData.Core.Services;

namespace HeroesMatchData.Core.ViewModels.Statistics
{
    public class HeroesGameModeUnrankedDraftViewModel : HeroesGameModeViewModel
    {
        public HeroesGameModeUnrankedDraftViewModel(IInternalService internalService)
            : base(internalService)
        {
        }
    }
}
