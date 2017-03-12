using GalaSoft.MvvmLight.Ioc;
using Heroes.Icons;
using HeroesMatchData.Core.ViewServices;

namespace HeroesMatchData.Core.ViewModels.Matches
{
    public class MatchesViewModel : HstViewModel, IMatchesTabService
    {
        private int _selectedMatchesTab;

        public MatchesViewModel(IHeroesIconsService heroesIcons)
            : base(heroesIcons)
        {
            SimpleIoc.Default.Register<IMatchesTabService>(() => this);
        }

        public int SelectedMatchesTab
        {
            get { return _selectedMatchesTab; }
            set
            {
                _selectedMatchesTab = value;
                RaisePropertyChanged();
            }
        }

        public void SwitchToTab(MatchesTab selectedMatchesTab)
        {
            SelectedMatchesTab = (int)selectedMatchesTab;
        }
    }
}
