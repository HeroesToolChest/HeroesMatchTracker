using Heroes.Helpers;
using HeroesMatchData.Core.Services;
using System.Collections.Generic;

namespace HeroesMatchData.Core.ViewModels.Statistics
{
    public class StatsHeroesViewModel : HmdViewModel
    {
        private string _selectedSeasonOption;
        private string _selectedHero;

        public StatsHeroesViewModel(IInternalService internalService)
            : base(internalService)
        {
            SeasonList.Add("Lifetime");
            SeasonList.AddRange(HeroesHelpers.Seasons.GetSeasonList());
            SelectedSeasonOption = SeasonList[0];

            HeroesList.AddRange(HeroesIcons.Heroes().GetListOfHeroes());
            SelectedHero = HeroesList[0];
        }

        public List<string> SeasonList { get; private set; } = new List<string>();
        public List<string> HeroesList { get; private set; } = new List<string>();

        public string SelectedSeasonOption
        {
            get => _selectedSeasonOption;
            set
            {
                _selectedSeasonOption = value;
                RaisePropertyChanged();
            }
        }

        public string SelectedHero
        {
            get => _selectedHero;
            set
            {
                _selectedHero = value;
                RaisePropertyChanged();
            }
        }
    }
}
