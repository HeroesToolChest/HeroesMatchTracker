using GalaSoft.MvvmLight.Ioc;
using HeroesMatchData.Core.Services;
using HeroesMatchData.Core.ViewServices;

namespace HeroesMatchData.Core.ViewModels.Matches
{
    public class MatchesViewModel : HmdViewModel, IMatchesTabService
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
