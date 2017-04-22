using GalaSoft.MvvmLight.Ioc;
using HeroesMatchTracker.Core.Services;
using HeroesMatchTracker.Core.ViewServices;

namespace HeroesMatchTracker.Core.ViewModels.Matches
{
    public class MatchesViewModel : HmtViewModel, IMatchesTabService
    {
        private int _selectedMatchesTab;

        public MatchesViewModel(IInternalService internalService)
            : base(internalService)
        {
            SimpleIoc.Default.Register<IMatchesTabService>(() => this);
        }

        public int SelectedMatchesTab
        {
            get => _selectedMatchesTab;
            set
            {
                _selectedMatchesTab = value;
                RaisePropertyChanged();
            }
        }

        public void SwitchToTab(MatchesTab selectedMatchesTab) => SelectedMatchesTab = (int)selectedMatchesTab;
    }
}
