using System;
using GalaSoft.MvvmLight.Ioc;
using Heroes.Icons;
using HeroesStatTracker.Core.ViewServices;

namespace HeroesStatTracker.Core.ViewModels.Matches
{
    public class MatchesViewModel : HstViewModel, IMatchesTabService, IMainPage
    {
        private int _selectedMatchesTab;

        public MatchesViewModel(IHeroesIconsService heroesIcons)
            : base(heroesIcons)
        {
            //SimpleIoc.Default.Register<IMatchesTabService>(() => this);
        }

        public string Name => "Matches";

        public int SelectedMatchesTab
        {
            get { return _selectedMatchesTab; }
            set
            {
                _selectedMatchesTab = value;
                RaisePropertyChanged();
            }
        }

        public void SwitchToTab(MatchesTabs selectedMatchesTab)
        {
            SelectedMatchesTab = (int)selectedMatchesTab;
        }
    }
}
