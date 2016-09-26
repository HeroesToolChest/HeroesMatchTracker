using Heroes.ReplayParser;
using HeroesParserData.DataQueries.ReplayData;
using HeroesParserData.Models.StatsModels;
using HeroesParserData.Properties;
using NLog;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace HeroesParserData.ViewModels.Stats
{
    public class OverviewStatsViewModel : ViewModelBase
    {
        private string _selectedMostStatsOption;
        private string _selectedSeasonOption;
        private string _selectedGameModeOption;
        private string _seasonLabel;
        private string _totalGamesWonLabel;
        private string _queryStatus;

        private List<string> _mostStatsList = new List<string>();
        private List<string> _seasonList = new List<string>();
        private List<string> _gameModeList = new List<string>();
        private ObservableCollection<StatsMostAmounts> _statsMostAmounts = new ObservableCollection<StatsMostAmounts>();
        private ObservableCollection<StatsMapPercentages> _statsMapPercentages = new ObservableCollection<StatsMapPercentages>();
        private ObservableCollection<string> _totalGameModeGames = new ObservableCollection<string>();
        private ObservableCollection<string> _winGameModePercentages = new ObservableCollection<string>();

        #region public properties
        public CollectionViewSource MostStatsViewSource { get; set; } = new CollectionViewSource();
        public CollectionViewSource MapStatsViewSource { get; set; } = new CollectionViewSource();

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
                RefreshStats();
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
                SeasonLabel = value;
                RefreshStats();
                RaisePropertyChangedEvent(nameof(SelectedSeasonOption));
            }
        }

        public string SelectedGameModeOption
        {
            get { return _selectedGameModeOption; }
            set
            {
                _selectedGameModeOption = value;
                RefreshStats();
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

        public string SeasonLabel
        {
            get { return _seasonLabel; }
            set
            {
                _seasonLabel = value;
                RaisePropertyChangedEvent(nameof(SeasonLabel));
            }
        }

        public string TotalGamesWonLabel
        {
            get { return _totalGamesWonLabel; }
            set
            {
                _totalGamesWonLabel = value;
                RaisePropertyChangedEvent(nameof(TotalGamesWonLabel));
            }
        }

        public string QueryStatus
        {
            get { return _queryStatus; }
            set
            {
                _queryStatus = value;
                RaisePropertyChangedEvent(nameof(QueryStatus));
            }
        }

        public ObservableCollection<string> TotalGameModeGames
        {
            get { return _totalGameModeGames; }

            set
            {
                _totalGameModeGames = value;
                RaisePropertyChangedEvent(nameof(TotalGameModeGames));
            }
        }

        public ObservableCollection<string> WinGameModePercentages
        {
            get { return _winGameModePercentages; }
            set
            {
                _winGameModePercentages = value;
                RaisePropertyChangedEvent(nameof(WinGameModePercentages));
            }
        }
        #endregion public properties

        /// <summary>
        /// Constructor
        /// </summary>
        public OverviewStatsViewModel()
            : base()
        {
            InitializeLists();

            MostStatsViewSource.Source = StatsMostAmounts;
            MostStatsViewSource.SortDescriptions.Add(new SortDescription("Amount", ListSortDirection.Descending));

            MapStatsViewSource.Source = StatsMapPercentages;
            MapStatsViewSource.SortDescriptions.Add(new SortDescription("WinPercentage", ListSortDirection.Descending));
        }

        private void InitializeLists()
        {
            MostStatsList.Add("Most Wins As");
            MostStatsList.Add("Most Kills As");
            MostStatsList.Add("Most Assists As");
            MostStatsList.Add("Most Deaths As");

            SeasonList.Add("Lifetime");
            SeasonList.AddRange(Utilities.GetSeasonList());

            GameModeList.Add("All Types");
            GameModeList.AddRange(Utilities.GetGameModes());

            var heroesList = HeroesInfo.GetListOfHeroes();

            foreach (var heroName in heroesList)
            {
                StatsMostAmounts statsMostAmounts = new StatsMostAmounts
                {
                    HeroName = heroName,
                    Amount = 0
                };

                StatsMostAmounts.Add(statsMostAmounts);
            }

            var maps = Maps.GetMaps();

            foreach (var map in maps)
            {
                StatsMapPercentages statsMapPercentages = new StatsMapPercentages
                {
                    MapName = map,
                    Wins = 0,
                    Losses = 0,
                    WinPercentage = "0%"
                };

                StatsMapPercentages.Add(statsMapPercentages);
            }
        }

        private void RefreshStats()
        {
            Task.Run(() =>
            {
                try
                {
                    QueryStatus = $"Retreiving stats for {SelectedSeasonOption} {SelectedGameModeOption}...";

                    if (SelectedMostStatsOption == "Most Wins As")
                        SetMostStats(StatType.wins);
                    else if (SelectedMostStatsOption == "Most Kills As")
                        SetMostStats(StatType.kills);
                    else if (SelectedMostStatsOption == "Most Assists As")
                        SetMostStats(StatType.assists);
                    else if (SelectedMostStatsOption == "Most Deaths As")
                        SetMostStats(StatType.deaths);

                    SetMapStats();
                    SetGameModesTotalGames();

                    QueryStatus = $"Query completed";
                }
                catch (Exception ex)
                {
                    ExceptionLog.Log(LogLevel.Error, ex);
                    QueryStatus = $"Query failed (Error)";
                }
            });
        }

        private void SetMostStats(StatType statType)
        {
            if (SelectedSeasonOption == null || SelectedGameModeOption == null || string.IsNullOrEmpty(Settings.Default.UserBattleTagName))
                return;

            Season season = Utilities.GetSeasonFromString(SelectedSeasonOption);
            GameMode gameMode = Utilities.GetGameModeFromString(SelectedGameModeOption);

            foreach (var hero in StatsMostAmounts)
            {
                hero.Amount = Query.PlayerStatistics.ReadTotalStatTypeForCharacter(statType, season, gameMode, hero.HeroName);
            }

            Application.Current.Dispatcher.Invoke(delegate
            {
                MostStatsViewSource.View.Refresh();
            });
        }

        private void SetMapStats()
        {
            if (SelectedSeasonOption == null || SelectedGameModeOption == null || string.IsNullOrEmpty(Settings.Default.UserBattleTagName))
                return;

            Season season = Utilities.GetSeasonFromString(SelectedSeasonOption);
            GameMode gameMode = Utilities.GetGameModeFromString(SelectedGameModeOption);

            foreach (var map in StatsMapPercentages)
            {
                int wins = Query.PlayerStatistics.ReadMapWins(season, gameMode, map.MapName);
                int losses = Query.PlayerStatistics.ReadMapLosses(season, gameMode, map.MapName);
                double total = wins + losses;
                int winPercentage = total != 0 ? (int)(Math.Round(wins / total, 2) * 100) : 0;

                map.Wins = wins;
                map.Losses = losses;
                if (wins == 0 && losses == 0)
                    map.WinPercentage = "---";
                else
                    map.WinPercentage = $"{winPercentage}%";
            }

            Application.Current.Dispatcher.Invoke(delegate
            {
                MapStatsViewSource.View.Refresh();
            });

            SetTotalWonGames();
        }

        private void SetTotalWonGames()
        {
            int wins = 0;

            foreach (var map in StatsMapPercentages)
            {
                wins += map.Wins;
            }

            TotalGamesWonLabel = $"{wins.ToString()} Games Won";
        }

        private void SetGameModesTotalGames()
        {
            Season season = Utilities.GetSeasonFromString(SelectedSeasonOption);

            double quickMatchTotal = Query.PlayerStatistics.ReadGameModeTotalGames(season, GameMode.QuickMatch);
            double unrankedTotal = Query.PlayerStatistics.ReadGameModeTotalGames(season, GameMode.UnrankedDraft);
            double heroTotal = Query.PlayerStatistics.ReadGameModeTotalGames(season, GameMode.HeroLeague);
            double TeamTotal = Query.PlayerStatistics.ReadGameModeTotalGames(season, GameMode.TeamLeague);

            int quickMatchWins = Query.PlayerStatistics.ReadGameModeTotalWins(season, GameMode.QuickMatch);
            int unrankedWins = Query.PlayerStatistics.ReadGameModeTotalWins(season, GameMode.UnrankedDraft);
            int heroWins = Query.PlayerStatistics.ReadGameModeTotalWins(season, GameMode.HeroLeague);
            int TeamWins = Query.PlayerStatistics.ReadGameModeTotalWins(season, GameMode.TeamLeague);

            int quickMatchWinPercent = quickMatchTotal != 0 ? (int)(Math.Round(quickMatchWins / quickMatchTotal, 2) * 100) : 0;
            int unrankedWinPercent = unrankedTotal != 0 ? (int)(Math.Round(unrankedWins / unrankedTotal, 2) * 100) : 0;
            int heroWinPercent = heroTotal != 0 ? (int)(Math.Round(heroWins / heroTotal, 2) * 100) : 0;
            int teamWinPercent = TeamTotal != 0 ? (int)(Math.Round(TeamWins / TeamTotal, 2) * 100) : 0;

            TotalGameModeGames.Clear();
            TotalGameModeGames.Add($"{quickMatchTotal.ToString()} total games");
            TotalGameModeGames.Add($"{unrankedTotal.ToString()} total games");
            TotalGameModeGames.Add($"{heroTotal.ToString()} total games");
            TotalGameModeGames.Add($"{TeamTotal.ToString()} total games");

            WinGameModePercentages.Clear();
            WinGameModePercentages.Add($"{quickMatchWinPercent}% winrate");
            WinGameModePercentages.Add($"{unrankedWinPercent}% winrate");
            WinGameModePercentages.Add($"{heroWinPercent}% winrate");
            WinGameModePercentages.Add($"{teamWinPercent}% winrate");
        }
    }
}
