using HeroesParserData.DataQueries;
using HeroesParserData.Models.StatsModels;
using System.Collections.Generic;
using System.Windows.Input;

namespace HeroesParserData.ViewModels.Stats.HeroStats
{
    public abstract class HeroStatsContext : ViewModelBase
    {
        private string _selectedSeasonOption;

        private List<string> _seasonList = new List<string>();

        protected Season GetSeasonSelected
        {
            get { return Utilities.GetSeasonFromString(SelectedSeasonOption); }
        }

        public List<string> SeasonList
        {
            get { return _seasonList; }
            set
            {
                _seasonList = value;
                RaisePropertyChangedEvent(nameof(SeasonList));
            }
        }

        public string SelectedSeasonOption
        {
            get { return _selectedSeasonOption; }
            set
            {
                _selectedSeasonOption = value;
                RaisePropertyChangedEvent(nameof(SelectedSeasonOption));
            }
        }

        public ICommand RefreshStatsCommand
        {
            get { return new DelegateCommand(() => RefreshStats()); }
        }

        public HeroStatsContext()
            :base()
        {
            InitializeLists();
        }

        protected abstract void RefreshStats();

        protected int QueryHeroLevels(string heroName)
        {
            return Query.HeroStatsGameMode.GetHighestLevelOfHero(heroName);
        }

        private void InitializeLists()
        {
            SeasonList.Add("Lifetime");
            SeasonList.AddRange(Utilities.GetSeasonList());

            SelectedSeasonOption = SeasonList[SeasonList.Count - 1];
        }
    }
}
