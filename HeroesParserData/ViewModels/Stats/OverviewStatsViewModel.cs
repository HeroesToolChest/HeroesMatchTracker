using Heroes.ReplayParser;
using HeroesParserData.DataQueries;
using HeroesParserData.Models.StatsModels;
using HeroesParserData.Properties;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

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
        private ObservableCollection<StatsMostAmounts> _statsMostAmountsCollection = new ObservableCollection<StatsMostAmounts>();
        private ObservableCollection<StatsMapPercentages> _statsMapPercentagesCollection = new ObservableCollection<StatsMapPercentages>();
        private ObservableCollection<string> _totalGameModeGamesCollection = new ObservableCollection<string>();
        private ObservableCollection<string> _winGameModePercentagesCollection = new ObservableCollection<string>();

        private bool Lock;
        #region public properties

        public List<string> MostStatsList
        {
            get { return _mostStatsList; }
            set
            {
                _mostStatsList = value;
                RaisePropertyChangedEvent(nameof(MostStatsList));
            }
        }

        public ObservableCollection<StatsMostAmounts> StatsMostAmountsCollection
        {
            get { return _statsMostAmountsCollection; }
            set
            {
                _statsMostAmountsCollection = value;
                RaisePropertyChangedEvent(nameof(StatsMostAmountsCollection));
            }
        }

        public ObservableCollection<string> TotalGameModeGamesCollection
        {
            get { return _totalGameModeGamesCollection; }

            set
            {
                _totalGameModeGamesCollection = value;
                RaisePropertyChangedEvent(nameof(TotalGameModeGamesCollection));
            }
        }

        public ObservableCollection<string> WinGameModePercentagesCollection
        {
            get { return _winGameModePercentagesCollection; }
            set
            {
                _winGameModePercentagesCollection = value;
                RaisePropertyChangedEvent(nameof(WinGameModePercentagesCollection));
            }
        }

        public ObservableCollection<StatsMapPercentages> StatsMapPercentagesCollection
        {
            get { return _statsMapPercentagesCollection; }
            set
            {
                _statsMapPercentagesCollection = value;
                RaisePropertyChangedEvent(nameof(StatsMapPercentagesCollection));
            }
        }

        public string SelectedMostStatsOption
        {
            get { return _selectedMostStatsOption; }
            set
            {
                _selectedMostStatsOption = value;
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
                RaisePropertyChangedEvent(nameof(SelectedSeasonOption));
            }
        }

        public string SelectedGameModeOption
        {
            get { return _selectedGameModeOption; }
            set
            {
                _selectedGameModeOption = value;
                RaisePropertyChangedEvent(nameof(SelectedGameModeOption));
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

        public ICommand ChangeMostStatCommand
        {
            get { return new DelegateCommand(() => PerformMostStatCommand()); }
        }

        public ICommand ChangeSeasonCommand
        {
            get { return new DelegateCommand(() => PerformMapOtherStatCommand()); }
        }

        public ICommand ChangeGameModeCommand
        {
            get { return new DelegateCommand(() => PerformMapOtherStatCommand()); }
        }
        #endregion public properties

            /// <summary>
            /// Constructor
            /// </summary>
        public OverviewStatsViewModel()
            : base()
        {
            InitializeLists();
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
        }

        private void PerformMostStatCommand()
        {
            Task.Run(async () =>
            {
                if (SelectedMostStatsOption == "Most Wins As")
                    await SetMostStats(StatType.wins);
                else if (SelectedMostStatsOption == "Most Kills As")
                    await SetMostStats(StatType.kills);
                else if (SelectedMostStatsOption == "Most Assists As")
                    await SetMostStats(StatType.assists);
                else if (SelectedMostStatsOption == "Most Deaths As")
                    await SetMostStats(StatType.deaths);
            });
        }
        
        private void PerformMapOtherStatCommand()
        {
            PerformMostStatCommand();

            Task.Run(async () =>
            {
                await SetMapStats();
                await SetGameModesTotalGames();
            });
        }

        private async Task SetMostStats(StatType statType)
        {
            if (SelectedSeasonOption == null || SelectedGameModeOption == null || string.IsNullOrEmpty(Settings.Default.UserBattleTagName))
                return;

            StatsMostAmountsCollection = new ObservableCollection<StatsMostAmounts>();

            Season season = Utilities.GetSeasonFromString(SelectedSeasonOption);
            GameMode gameMode = Utilities.GetGameModeFromString(SelectedGameModeOption);

            var heroesList = HeroesInfo.GetListOfHeroes();

            foreach (var heroName in heroesList)
            {
                StatsMostAmounts statsMostAmounts = new StatsMostAmounts
                {
                    HeroName = heroName,
                    Amount = Query.PlayerStatistics.ReadTotalStatTypeForCharacter(statType, season, gameMode, heroName),
                };

                await Application.Current.Dispatcher.InvokeAsync(delegate
                {
                    StatsMostAmountsCollection.Add(statsMostAmounts);
                });
            }
        }

        private async Task SetMapStats()
        {
            if (SelectedSeasonOption == null || SelectedGameModeOption == null || string.IsNullOrEmpty(Settings.Default.UserBattleTagName))
                return;

            if (Lock == true)
                return;

            Lock = true;
            StatsMapPercentagesCollection = new ObservableCollection<StatsMapPercentages>();

            Season season = Utilities.GetSeasonFromString(SelectedSeasonOption);
            GameMode gameMode = Utilities.GetGameModeFromString(SelectedGameModeOption);

            var maps = Maps.GetMaps();

            foreach (var map in maps)
            {
                int wins = Query.PlayerStatistics.ReadMapWins(season, gameMode, map);
                int losses = Query.PlayerStatistics.ReadMapLosses(season, gameMode, map);
                double total = wins + losses;
                int winPercentage = Utilities.CalculateWinPercentage(wins, total);

                StatsMapPercentages statsMapPercentages = new StatsMapPercentages
                {
                    MapName = map,
                    Wins = wins,
                    Losses = losses,
                    WinPercentage = winPercentage,
                };

                await Application.Current.Dispatcher.InvokeAsync(delegate
                {
                    StatsMapPercentagesCollection.Add(statsMapPercentages);
                });        
            }

            SetTotalWonGames();
            Lock = false;
        }

        private void SetTotalWonGames()
        {
            int wins = 0;

            foreach (var map in StatsMapPercentagesCollection)
            {
                wins += map.Wins;
            }

            TotalGamesWonLabel = $"{wins.ToString()} Games Won";
        }

        private async Task SetGameModesTotalGames()
        {
            Season season = Utilities.GetSeasonFromString(SelectedSeasonOption);

            double quickMatchTotal = Query.PlayerStatistics.ReadGameModeTotalGames(season, GameMode.QuickMatch);
            double unrankedTotal = Query.PlayerStatistics.ReadGameModeTotalGames(season, GameMode.UnrankedDraft);
            double heroTotal = Query.PlayerStatistics.ReadGameModeTotalGames(season, GameMode.HeroLeague);
            double teamTotal = Query.PlayerStatistics.ReadGameModeTotalGames(season, GameMode.TeamLeague);

            int quickMatchWins = Query.PlayerStatistics.ReadGameModeTotalWins(season, GameMode.QuickMatch);
            int unrankedWins = Query.PlayerStatistics.ReadGameModeTotalWins(season, GameMode.UnrankedDraft);
            int heroWins = Query.PlayerStatistics.ReadGameModeTotalWins(season, GameMode.HeroLeague);
            int teamWins = Query.PlayerStatistics.ReadGameModeTotalWins(season, GameMode.TeamLeague);

            int quickMatchWinPercent = Utilities.CalculateWinPercentage(quickMatchWins, quickMatchTotal);
            int unrankedWinPercent = Utilities.CalculateWinPercentage(unrankedWins, unrankedTotal);
            int heroWinPercent = Utilities.CalculateWinPercentage(heroWins, heroTotal);
            int teamWinPercent = Utilities.CalculateWinPercentage(teamWins, teamTotal);

            await Application.Current.Dispatcher.InvokeAsync(delegate
            {
                TotalGameModeGamesCollection.Clear();
                TotalGameModeGamesCollection.Add($"{quickMatchTotal} total games");
                TotalGameModeGamesCollection.Add($"{unrankedTotal} total games");
                TotalGameModeGamesCollection.Add($"{heroTotal} total games");
                TotalGameModeGamesCollection.Add($"{teamTotal} total games");

                WinGameModePercentagesCollection.Clear();
                WinGameModePercentagesCollection.Add($"{quickMatchWinPercent}% winrate");
                WinGameModePercentagesCollection.Add($"{unrankedWinPercent}% winrate");
                WinGameModePercentagesCollection.Add($"{heroWinPercent}% winrate");
                WinGameModePercentagesCollection.Add($"{teamWinPercent}% winrate");
            });
        }
    }
}
