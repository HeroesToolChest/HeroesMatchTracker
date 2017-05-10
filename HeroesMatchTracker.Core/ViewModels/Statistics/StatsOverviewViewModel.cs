using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HeroesMatchTracker.Core.Services;
using GalaSoft.MvvmLight.CommandWpf;
using Heroes.Helpers;
using System.Collections.ObjectModel;
using HeroesMatchTracker.Core.Models.StatisticsModels;

namespace HeroesMatchTracker.Core.ViewModels.Statistics
{
    public class StatsOverviewViewModel : HmtViewModel
    {
        private readonly string InitialSeasonListOption = "- Select Season -";

        private bool _isQuickMatchSelected;
        private bool _isUnrankedDraftSelected;
        private bool _isHeroLeagueSelected;
        private bool _isTeamLeagueSelected;
        private bool _isCustomGameSelected;
        private bool _isBrawlSelected;
        private string _selectedSeason;
        private string _selectedHeroStat;

        private ObservableCollection<StatsOverviewHeroes> _heroStatsCollection = new ObservableCollection<StatsOverviewHeroes>();
        private ObservableCollection<StatsOverviewMaps> _mapsStatsCollection = new ObservableCollection<StatsOverviewMaps>();

        public StatsOverviewViewModel(IInternalService internalService)
            : base(internalService)
        {
            SeasonList.Add(InitialSeasonListOption);
            SeasonList.Add("Lifetime");
            SeasonList.AddRange(HeroesHelpers.Seasons.GetSeasonList());
            SelectedSeason = SeasonList[0];

            HeroStatsList.AddRange(HeroesHelpers.OverviewHeroStatOptions.GetOverviewHeroStatOptionList());
            SelectedHeroStat = HeroStatsList[0];
        }

        public RelayCommand QueryOverviewStatsCommand => new RelayCommand(async () => await QueryOverviewStatsAsyncCommand());

        public List<string> SeasonList { get; private set; } = new List<string>();
        public List<string> HeroStatsList { get; private set; } = new List<string>();

        public string SelectedSeason
        {
            get => _selectedSeason;
            set
            {
                _selectedSeason = value;
                RaisePropertyChanged();
            }
        }

        public string SelectedHeroStat
        {
            get => _selectedHeroStat;
            set
            {
                _selectedHeroStat = value;
                RaisePropertyChanged();
            }
        }


        public bool IsQuickMatchSelected
        {
            get => _isQuickMatchSelected;
            set
            {
                _isQuickMatchSelected = value;
                RaisePropertyChanged();
            }
        }

        public bool IsUnrankedDraftSelected
        {
            get => _isUnrankedDraftSelected;
            set
            {
                _isUnrankedDraftSelected = value;
                RaisePropertyChanged();
            }
        }

        public bool IsHeroLeagueSelected
        {
            get => _isHeroLeagueSelected;
            set
            {
                _isHeroLeagueSelected = value;
                RaisePropertyChanged();
            }
        }

        public bool IsTeamLeagueSelected
        {
            get => _isTeamLeagueSelected;
            set
            {
                _isTeamLeagueSelected = value;
                RaisePropertyChanged();
            }
        }

        public bool IsCustomGameSelected
        {
            get => _isCustomGameSelected;
            set
            {
                _isCustomGameSelected = value;
                RaisePropertyChanged();
            }
        }

        public bool IsBrawlSelected
        {
            get => _isBrawlSelected;
            set
            {
                _isBrawlSelected = value;
                RaisePropertyChanged();
            }
        }

        public ObservableCollection<StatsOverviewHeroes> HeroStatsCollection
        {
            get => _heroStatsCollection;
            set
            {
                _heroStatsCollection = value;
                RaisePropertyChanged();
            }
        }

        public ObservableCollection<StatsOverviewMaps> MapsStatsCollection
        {
            get => _mapsStatsCollection;
            set
            {
                _mapsStatsCollection = value;
                RaisePropertyChanged();
            }
        }

        private async Task QueryOverviewStatsAsyncCommand()
        {
            throw new NotImplementedException();
        }
    }
}
