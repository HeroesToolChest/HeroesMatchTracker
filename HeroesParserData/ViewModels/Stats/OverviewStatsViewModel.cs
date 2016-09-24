using Heroes.ReplayParser;
using HeroesParserData.DataQueries.ReplayData;
using HeroesParserData.Models.StatsModels;
using HeroesParserData.Properties;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace HeroesParserData.ViewModels.Stats
{
    public class OverviewStatsViewModel : ViewModelBase
    {
        private BitmapImage _backgroundMapImage;
        private Color _labelGlowColor;
        private List<string> _mostStatsList = new List<string>();
        private List<string> _seasonList = new List<string>();
        private List<string> _gameModeList = new List<string>();
        private ObservableCollection<StatsMostAmounts> _statsMostAmounts = new ObservableCollection<StatsMostAmounts>();
        private ObservableCollection<StatsMapPercentages> _statsMapPercentages = new ObservableCollection<StatsMapPercentages>();

        private string _selectedMostStatsOption;
        private string _selectedSeasonOption;
        private string _selectedGameModeOption;

        #region public properties
        public CollectionViewSource MostStatsViewSource { get; set; } = new CollectionViewSource();
        public CollectionViewSource MapStatsViewSource { get; set; } = new CollectionViewSource();

        public BitmapImage BackgroundMapImage
        {
            get { return _backgroundMapImage; }
            set
            {
                _backgroundMapImage = value;
                RaisePropertyChangedEvent(nameof(BackgroundMapImage));
            }
        }

        public Color LabelGlowColor
        {
            get { return _labelGlowColor; }
            set
            {
                _labelGlowColor = value;
                RaisePropertyChangedEvent(nameof(LabelGlowColor));
            }
        }

        public List<string> MostStatsList
        {
            get { return _mostStatsList; }
            set
            {
                _mostStatsList = value;
                RaisePropertyChangedEvent(nameof(MostStatsList));
            }
        }

        public ObservableCollection<StatsMostAmounts> StatsMostAmounts
        {
            get { return _statsMostAmounts; }
            set
            {
                _statsMostAmounts = value;
                RaisePropertyChangedEvent(nameof(StatsMostAmounts));
            }
        }

        public string SelectedMostStatsOption
        {
            get { return _selectedMostStatsOption; }
            set
            {
                _selectedMostStatsOption = value;
                RefreshMostStats();
                RaisePropertyChangedEvent(nameof(SelectedMostStatsOption));
            }
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

        public List<string> GameModeList
        {
            get { return _gameModeList; }
            set
            {
                _gameModeList = value;
                RaisePropertyChangedEvent(nameof(GameModeList));
            }
        }

        public string SelectedSeasonOption
        {
            get { return _selectedSeasonOption; }
            set
            {
                _selectedSeasonOption = value;
                RefreshMostStats();
                RefreshMapStats();
                RaisePropertyChangedEvent(nameof(SelectedSeasonOption));
            }
        }

        public string SelectedGameModeOption
        {
            get { return _selectedGameModeOption; }
            set
            {
                _selectedGameModeOption = value;
                RefreshMostStats();
                RefreshMapStats();
                RaisePropertyChangedEvent(nameof(SelectedGameModeOption));
            }
        }

        public ObservableCollection<StatsMapPercentages> StatsMapPercentages
        {
            get { return _statsMapPercentages; }
            set
            {
                _statsMapPercentages = value;
                RaisePropertyChangedEvent(nameof(StatsMapPercentages));
            }
        }
        #endregion public properties

        /// <summary>
        /// Constructor
        /// </summary>
        public OverviewStatsViewModel()
            : base()
        {
            SetBackgroundImage();

            SetMostStatsList();
            SetSeasonList();
            SetGameModesList();

            MostStatsViewSource.Source = StatsMostAmounts;
            MostStatsViewSource.SortDescriptions.Add(new SortDescription("Amount", ListSortDirection.Descending));

            MapStatsViewSource.Source = StatsMapPercentages;
            MapStatsViewSource.SortDescriptions.Add(new SortDescription("WinPercentage", ListSortDirection.Descending));
        }

        private void SetBackgroundImage()
        {
            BackgroundMapImage = new BitmapImage(new Uri("pack://application:,,,/HeroesIcons;component/Icons/Homescreens/storm_ui_homescreenbackground_nexus.jpg", UriKind.Absolute));
            LabelGlowColor = Colors.Purple;
        }

        private void SetMostStatsList()
        {
            MostStatsList.Add("Most Kills");
            MostStatsList.Add("Most Assists");
            MostStatsList.Add("Most Deaths");
        }

        private void SetSeasonList()
        {
            SeasonList = Utilities.GetSeasonList();
        }

        private void SetGameModesList()
        {
            GameModeList = Utilities.GetGameModes();
        }

        private void RefreshMostStats()
        {
            if (SelectedMostStatsOption == "Most Kills")
                SetMostStats(StatType.kills);
            else if (SelectedMostStatsOption == "Most Assists")
                SetMostStats(StatType.assists);
            else if (SelectedMostStatsOption == "Most Deaths")
                SetMostStats(StatType.deaths);
        }

        private void RefreshMapStats()
        {
            SetMapStats();
        }

        private void SetMostStats(StatType statType)
        {
            StatsMostAmounts.Clear();

            if (SelectedSeasonOption == null || SelectedGameModeOption == null || string.IsNullOrEmpty(Settings.Default.UserBattleTagName))
                return;

            Season season = Utilities.GetSeasonFromString(SelectedSeasonOption);
            GameMode gameMode = Utilities.GetGameModeFromString(SelectedGameModeOption);

            var heroesList = HeroesInfo.GetListOfHeroes();

            foreach (var heroName in heroesList)
            {
                StatsMostAmounts statsMostAmounts = new StatsMostAmounts
                {
                    HeroName = heroName,
                    Amount = Query.PlayerStatistics.ReadTotalStatTypeForCharacter(statType, season, gameMode, heroName)
                };

                StatsMostAmounts.Add(statsMostAmounts);
            }
        }

        private void SetMapStats()
        {
            StatsMapPercentages.Clear();

            if (SelectedSeasonOption == null || SelectedGameModeOption == null || string.IsNullOrEmpty(Settings.Default.UserBattleTagName))
                return;

            Season season = Utilities.GetSeasonFromString(SelectedSeasonOption);
            GameMode gameMode = Utilities.GetGameModeFromString(SelectedGameModeOption);

            var maps = Maps.GetMaps();

            foreach (var map in maps)
            {
                int wins = Query.PlayerStatistics.ReadMapWins(season, gameMode, map);
                int losses = Query.PlayerStatistics.ReadMapLosses(season, gameMode, map);
                double total = wins + losses;
                StatsMapPercentages statsMapPercentages = new StatsMapPercentages
                {
                    MapName = map,
                    Wins = wins,
                    Losses = losses,
                    WinPercentage = total != 0? (int)(Math.Round(wins / total, 2) * 100) : 0   
                };

                StatsMapPercentages.Add(statsMapPercentages);
            }
        }
    }
}
