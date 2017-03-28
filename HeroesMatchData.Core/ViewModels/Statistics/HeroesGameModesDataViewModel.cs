using HeroesMatchData.Core.Models.StatisticsModels;
using HeroesMatchData.Core.Services;
using System.Collections.ObjectModel;

namespace HeroesMatchData.Core.ViewModels.Statistics
{
    public class HeroesGameModesDataViewModel : HmdViewModel
    {
        private ObservableCollection<StatsHeroesGamesModes> _statsHeroesQuickMatchDataCollection = new ObservableCollection<StatsHeroesGamesModes>();
        private ObservableCollection<StatsHeroesGamesModes> _statsHeroesUnrankedDraftDataCollection = new ObservableCollection<StatsHeroesGamesModes>();
        private ObservableCollection<StatsHeroesGamesModes> _statsHeroesHeroLeagueDataCollection = new ObservableCollection<StatsHeroesGamesModes>();
        private ObservableCollection<StatsHeroesGamesModes> _statsHeroesTeamLeagueDataCollection = new ObservableCollection<StatsHeroesGamesModes>();
        private ObservableCollection<StatsHeroesGamesModes> _statsHeroesCustomGameDataCollection = new ObservableCollection<StatsHeroesGamesModes>();

        private ObservableCollection<StatsHeroesGamesModes> _statsHeroesQuickMatchDataTotalCollection = new ObservableCollection<StatsHeroesGamesModes>();
        private ObservableCollection<StatsHeroesGamesModes> _statsHeroesUnrankedDraftDataTotalCollection = new ObservableCollection<StatsHeroesGamesModes>();
        private ObservableCollection<StatsHeroesGamesModes> _statsHeroesHeroLeagueDataTotalCollection = new ObservableCollection<StatsHeroesGamesModes>();
        private ObservableCollection<StatsHeroesGamesModes> _statsHeroesTeamLeagueDataTotalCollection = new ObservableCollection<StatsHeroesGamesModes>();
        private ObservableCollection<StatsHeroesGamesModes> _statsHeroesCustomGameDataTotalCollection = new ObservableCollection<StatsHeroesGamesModes>();

        private ObservableCollection<StatsHeroesGamesModes> _statsHeroesQuickMatchDataAverageCollection = new ObservableCollection<StatsHeroesGamesModes>();
        private ObservableCollection<StatsHeroesGamesModes> _statsHeroesUnrankedDraftDataAverageCollection = new ObservableCollection<StatsHeroesGamesModes>();
        private ObservableCollection<StatsHeroesGamesModes> _statsHeroesHeroLeagueDataAverageCollection = new ObservableCollection<StatsHeroesGamesModes>();
        private ObservableCollection<StatsHeroesGamesModes> _statsHeroesTeamLeagueDataAverageCollection = new ObservableCollection<StatsHeroesGamesModes>();
        private ObservableCollection<StatsHeroesGamesModes> _statsHeroesCustomGameDataAverageCollection = new ObservableCollection<StatsHeroesGamesModes>();

        private ObservableCollection<StatsHeroesGamesModes> _statsHeroesQuickMatchDataAverageTotalCollection = new ObservableCollection<StatsHeroesGamesModes>();
        private ObservableCollection<StatsHeroesGamesModes> _statsHeroesUnrankedDraftDataAverageTotalCollection = new ObservableCollection<StatsHeroesGamesModes>();
        private ObservableCollection<StatsHeroesGamesModes> _statsHeroesHeroLeagueDataAverageTotalCollection = new ObservableCollection<StatsHeroesGamesModes>();
        private ObservableCollection<StatsHeroesGamesModes> _statsHeroesTeamLeagueDataAverageTotalCollection = new ObservableCollection<StatsHeroesGamesModes>();
        private ObservableCollection<StatsHeroesGamesModes> _statsHeroesCustomGameDataAverageTotalCollection = new ObservableCollection<StatsHeroesGamesModes>();

        protected HeroesGameModesDataViewModel(IInternalService internalService)
            : base(internalService)
        {
        }

        public ObservableCollection<StatsHeroesGamesModes> StatsHeroesQuickMatchDataCollection
        {
            get => _statsHeroesQuickMatchDataCollection;
            set
            {
                _statsHeroesQuickMatchDataCollection = value;
                RaisePropertyChanged();
            }
        }

        public ObservableCollection<StatsHeroesGamesModes> StatsHeroesUnrankedDraftDataCollection
        {
            get => _statsHeroesUnrankedDraftDataCollection;
            set
            {
                _statsHeroesUnrankedDraftDataCollection = value;
                RaisePropertyChanged();
            }
        }

        public ObservableCollection<StatsHeroesGamesModes> StatsHeroesHeroLeagueDataCollection
        {
            get => _statsHeroesHeroLeagueDataCollection;
            set
            {
                _statsHeroesHeroLeagueDataCollection = value;
                RaisePropertyChanged();
            }
        }

        public ObservableCollection<StatsHeroesGamesModes> StatsHeroesTeamLeagueDataCollection
        {
            get => _statsHeroesTeamLeagueDataCollection;
            set
            {
                _statsHeroesTeamLeagueDataCollection = value;
                RaisePropertyChanged();
            }
        }

        public ObservableCollection<StatsHeroesGamesModes> StatsHeroesCustomGameDataCollection
        {
            get => _statsHeroesCustomGameDataCollection;
            set
            {
                _statsHeroesCustomGameDataCollection = value;
                RaisePropertyChanged();
            }
        }

        public ObservableCollection<StatsHeroesGamesModes> StatsHeroesQuickMatchDataTotalCollection
        {
            get => _statsHeroesQuickMatchDataTotalCollection;
            set
            {
                _statsHeroesQuickMatchDataTotalCollection = value;
                RaisePropertyChanged();
            }
        }

        public ObservableCollection<StatsHeroesGamesModes> StatsHeroesUnrankedDraftDataTotalCollection
        {
            get => _statsHeroesUnrankedDraftDataTotalCollection;
            set
            {
                _statsHeroesUnrankedDraftDataTotalCollection = value;
                RaisePropertyChanged();
            }
        }

        public ObservableCollection<StatsHeroesGamesModes> StatsHeroesHeroLeagueDataTotalCollection
        {
            get => _statsHeroesHeroLeagueDataTotalCollection;
            set
            {
                _statsHeroesHeroLeagueDataTotalCollection = value;
                RaisePropertyChanged();
            }
        }

        public ObservableCollection<StatsHeroesGamesModes> StatsHeroesTeamLeagueDataTotalCollection
        {
            get => _statsHeroesTeamLeagueDataTotalCollection;
            set
            {
                _statsHeroesTeamLeagueDataTotalCollection = value;
                RaisePropertyChanged();
            }
        }

        public ObservableCollection<StatsHeroesGamesModes> StatsHeroesCustomGameDataTotalCollection
        {
            get => _statsHeroesCustomGameDataTotalCollection;
            set
            {
                _statsHeroesCustomGameDataTotalCollection = value;
                RaisePropertyChanged();
            }
        }

        public ObservableCollection<StatsHeroesGamesModes> StatsHeroesQuickMatchDataAverageCollection
        {
            get => _statsHeroesQuickMatchDataAverageCollection;
            set
            {
                _statsHeroesQuickMatchDataAverageCollection = value;
                RaisePropertyChanged();
            }
        }

        public ObservableCollection<StatsHeroesGamesModes> StatsHeroesUnrankedDraftDataAverageCollection
        {
            get => _statsHeroesUnrankedDraftDataAverageCollection;
            set
            {
                _statsHeroesUnrankedDraftDataAverageCollection = value;
                RaisePropertyChanged();
            }
        }

        public ObservableCollection<StatsHeroesGamesModes> StatsHeroesHeroLeagueDataAverageCollection
        {
            get => _statsHeroesHeroLeagueDataAverageCollection;
            set
            {
                _statsHeroesHeroLeagueDataAverageCollection = value;
                RaisePropertyChanged();
            }
        }

        public ObservableCollection<StatsHeroesGamesModes> StatsHeroesTeamLeagueDataAverageCollection
        {
            get => _statsHeroesTeamLeagueDataAverageCollection;
            set
            {
                _statsHeroesTeamLeagueDataAverageCollection = value;
                RaisePropertyChanged();
            }
        }

        public ObservableCollection<StatsHeroesGamesModes> StatsHeroesCustomGameDataAverageCollection
        {
            get => _statsHeroesCustomGameDataAverageCollection;
            set
            {
                _statsHeroesCustomGameDataAverageCollection = value;
                RaisePropertyChanged();
            }
        }

        public ObservableCollection<StatsHeroesGamesModes> StatsHeroesQuickMatchDataAverageTotalCollection
        {
            get => _statsHeroesQuickMatchDataAverageTotalCollection;
            set
            {
                _statsHeroesQuickMatchDataAverageTotalCollection = value;
                RaisePropertyChanged();
            }
        }

        public ObservableCollection<StatsHeroesGamesModes> StatsHeroesUnrankedDraftDataAverageTotalCollection
        {
            get => _statsHeroesUnrankedDraftDataAverageTotalCollection;
            set
            {
                _statsHeroesUnrankedDraftDataAverageTotalCollection = value;
                RaisePropertyChanged();
            }
        }

        public ObservableCollection<StatsHeroesGamesModes> StatsHeroesHeroLeagueDataAverageTotalCollection
        {
            get => _statsHeroesHeroLeagueDataAverageTotalCollection;
            set
            {
                _statsHeroesHeroLeagueDataAverageTotalCollection = value;
                RaisePropertyChanged();
            }
        }

        public ObservableCollection<StatsHeroesGamesModes> StatsHeroesTeamLeagueDataAverageTotalCollection
        {
            get => _statsHeroesTeamLeagueDataAverageTotalCollection;
            set
            {
                _statsHeroesTeamLeagueDataAverageTotalCollection = value;
                RaisePropertyChanged();
            }
        }
    }
}
