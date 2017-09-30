using GalaSoft.MvvmLight.CommandWpf;
using Heroes.Helpers;
using Heroes.Icons.Models;
using HeroesMatchTracker.Core.Models.StatisticsModels;
using HeroesMatchTracker.Core.Services;
using HeroesMatchTracker.Core.ViewServices;
using Microsoft.Practices.ServiceLocation;
using NLog;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace HeroesMatchTracker.Core.ViewModels.Statistics
{
    public class StatsOverviewViewModel : HmtViewModel
    {
        private readonly string InitialSeasonListOption = "- Select Season -";

        private int _overallGamesPlayed;
        private int _overallTotalTakedowns;
        private bool _isQuickMatchSelected;
        private bool _isUnrankedDraftSelected;
        private bool _isHeroLeagueSelected;
        private bool _isTeamLeagueSelected;
        private bool _isCustomGameSelected;
        private bool _isBrawlSelected;
        private bool _isHeroStatPercentageDataGridVisible;
        private bool _isHeroStatDataGridVisible;
        private double _overallWinrate;
        private double _overallKDARatio;
        private double _overallAverageTakedowns;
        private string _selectedSeason;
        private string _selectedHeroStat;

        private ILoadingOverlayWindowService LoadingOverlayWindow;

        private ObservableCollection<StatsOverviewHeroes> _heroStatsCollection = new ObservableCollection<StatsOverviewHeroes>();
        private ObservableCollection<StatsOverviewHeroes> _heroStatsPercentageCollection = new ObservableCollection<StatsOverviewHeroes>();
        private ObservableCollection<StatsOverviewMaps> _mapsStatsCollection = new ObservableCollection<StatsOverviewMaps>();
        private ObservableCollection<int> _roleGamesCollection = new ObservableCollection<int>();
        private ObservableCollection<double> _roleWinrateCollection = new ObservableCollection<double>();

        // used to hold the stats for dynamic switching instead of requerying each time
        private Collection<StatsOverviewHeroes> HeroStatsWinsCollection = new Collection<StatsOverviewHeroes>();
        private Collection<StatsOverviewHeroes> HeroStatsDeathsCollection = new Collection<StatsOverviewHeroes>();
        private Collection<StatsOverviewHeroes> HeroStatsKillsCollection = new Collection<StatsOverviewHeroes>();
        private Collection<StatsOverviewHeroes> HeroStatsAssistsCollection = new Collection<StatsOverviewHeroes>();

        public StatsOverviewViewModel(IInternalService internalService, ILoadingOverlayWindowService loadingOverlayWindow)
            : base(internalService)
        {
            LoadingOverlayWindow = loadingOverlayWindow;

            SeasonList.Add(InitialSeasonListOption);
            SeasonList.AddRange(HeroesHelpers.Seasons.GetSeasonList());
            SelectedSeason = SeasonList[0];

            HeroStatsList.AddRange(HeroesHelpers.OverviewHeroStatOptions.GetOverviewHeroStatOptionList());
            SelectedHeroStat = HeroStatsList[0];

            IsHeroStatPercentageDataGridVisible = true;
            IsHeroStatDataGridVisible = false;
        }

        public IMainWindowDialogsService MainWindowDialog => ServiceLocator.Current.GetInstance<IMainWindowDialogsService>();

        public RelayCommand QueryOverviewStatsCommand => new RelayCommand(async () => await QueryOverviewStatsAsyncCommand());
        public RelayCommand QuerySelectedHeroStatCommand => new RelayCommand(QuerySelectedHeroStat);

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

        public bool IsHeroStatPercentageDataGridVisible
        {
            get => _isHeroStatPercentageDataGridVisible;
            set
            {
                _isHeroStatPercentageDataGridVisible = value;
                RaisePropertyChanged();
            }
        }

        public bool IsHeroStatDataGridVisible
        {
            get => _isHeroStatDataGridVisible;
            set
            {
                _isHeroStatDataGridVisible = value;
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

        public ObservableCollection<StatsOverviewHeroes> HeroStatsPercentageCollection
        {
            get => _heroStatsPercentageCollection;
            set
            {
                _heroStatsPercentageCollection = value;
                RaisePropertyChanged();
            }
        }

        public ObservableCollection<int> RoleGamesCollection
        {
            get => _roleGamesCollection;
            set
            {
                _roleGamesCollection = value;
                RaisePropertyChanged();
            }
        }

        public ObservableCollection<double> RoleWinrateCollection
        {
            get => _roleWinrateCollection;
            set
            {
                _roleWinrateCollection = value;
                RaisePropertyChanged();
            }
        }

        public int OverallGamesPlayed
        {
            get => _overallGamesPlayed;
            set
            {
                _overallGamesPlayed = value;
                RaisePropertyChanged();
            }
        }

        public int OverallTotalTakedowns
        {
            get => _overallTotalTakedowns;
            set
            {
                _overallTotalTakedowns = value;
                RaisePropertyChanged();
            }
        }

        public double OverallWinrate
        {
            get => _overallWinrate;
            set
            {
                _overallWinrate = value;
                RaisePropertyChanged();
            }
        }

        public double OverallKDARatio
        {
            get => _overallKDARatio;
            set
            {
                _overallKDARatio = value;
                RaisePropertyChanged();
            }
        }

        public double OverallAverageTakedowns
        {
            get => _overallAverageTakedowns;
            set
            {
                _overallAverageTakedowns = value;
                RaisePropertyChanged();
            }
        }

        // for query button
        private async Task QueryOverviewStatsAsyncCommand()
        {
            if (await MainWindowDialog.CheckBattleTagSetDialog())
                return;

            await Task.Run(async () =>
            {
                try
                {
                    LoadingOverlayWindow.ShowLoadingOverlay();
                    await Task.Delay(1);
                    await QueryOverviewStatsAsync();
                }
                catch (Exception ex)
                {
                    ExceptionLog.Log(LogLevel.Error, ex);
                    throw;
                }
            });

            LoadingOverlayWindow.CloseLoadingOverlay();
        }

        // just for the Hero Stats datagrid, switch data according to selected hero stat
        private void QuerySelectedHeroStat()
        {
            Enum.TryParse(SelectedHeroStat, out OverviewHeroStatOption selectedStatOption);

            IsHeroStatPercentageDataGridVisible = false;
            IsHeroStatDataGridVisible = false;

            if (selectedStatOption == OverviewHeroStatOption.HighestWinrate)
            {
                IsHeroStatPercentageDataGridVisible = true;
            }
            else if (selectedStatOption == OverviewHeroStatOption.MostWins)
            {
                IsHeroStatDataGridVisible = true;
                HeroStatsCollection = new ObservableCollection<StatsOverviewHeroes>(HeroStatsWinsCollection.OrderByDescending(x => x.Value));
            }
            else if (selectedStatOption == OverviewHeroStatOption.MostDeaths)
            {
                IsHeroStatDataGridVisible = true;
                HeroStatsCollection = new ObservableCollection<StatsOverviewHeroes>(HeroStatsDeathsCollection.OrderByDescending(x => x.Value));
            }
            else if (selectedStatOption == OverviewHeroStatOption.MostKills)
            {
                IsHeroStatDataGridVisible = true;
                HeroStatsCollection = new ObservableCollection<StatsOverviewHeroes>(HeroStatsKillsCollection.OrderByDescending(x => x.Value));
            }
            else if (selectedStatOption == OverviewHeroStatOption.MostAssists)
            {
                IsHeroStatDataGridVisible = true;
                HeroStatsCollection = new ObservableCollection<StatsOverviewHeroes>(HeroStatsAssistsCollection.OrderByDescending(x => x.Value));
            }
        }

        private async Task QueryOverviewStatsAsync()
        {
            Clear();

            if (SelectedSeason == InitialSeasonListOption || string.IsNullOrEmpty(SelectedSeason))
                return;

            HeroesIcons.LoadLatestHeroesBuild();

            Season selectedSeason = SelectedSeason.ConvertToEnum<Season>();

            GameMode selectedGameModes = SetSelectedGameModes();

            await SetHeroStats(selectedSeason, selectedGameModes, SelectedHeroStat.ConvertToEnum<OverviewHeroStatOption>());
            await SetMapStats(selectedSeason, selectedGameModes);
            await SetRoleStats(selectedSeason, selectedGameModes);
            SetOverallStats();
        }

        private async Task SetHeroStats(Season season, GameMode gameModes, OverviewHeroStatOption statOption)
        {
            var heroesList = HeroesIcons.Heroes().GetListOfHeroes(HeroesIcons.GetLatestHeroesBuild());
            var heroStatPercentageCollection = new Collection<StatsOverviewHeroes>();
            var heroStatCollection = new Collection<StatsOverviewHeroes>();

            foreach (var hero in heroesList)
            {
                // highest win rate
                {
                    int wins = Database.ReplaysDb().Statistics.ReadGameResults(hero, season, gameModes, true);
                    int losses = Database.ReplaysDb().Statistics.ReadGameResults(hero, season, gameModes, false);
                    int total = wins + losses;

                    StatsOverviewHeroes statsOverviewHeroes = new StatsOverviewHeroes()
                    {
                        HeroName = hero,
                        Value = total > 0 ? Utilities.CalculateWinValue(wins, total) : (double?)null,
                    };

                    heroStatPercentageCollection.Add(statsOverviewHeroes);
                }

                // most wins
                {
                    int wins = Database.ReplaysDb().Statistics.ReadGameResults(hero, season, gameModes, true);

                    StatsOverviewHeroes statsOverviewHeroes = new StatsOverviewHeroes()
                    {
                        HeroName = hero,
                        Value = wins > 0 ? wins : (double?)null,
                    };

                    HeroStatsWinsCollection.Add(statsOverviewHeroes);
                }

                // most deaths
                {
                    int deaths = Database.ReplaysDb().Statistics.ReadStatValue(hero, season, gameModes, OverviewHeroStatOption.MostDeaths);

                    StatsOverviewHeroes statsOverviewHeroes = new StatsOverviewHeroes()
                    {
                        HeroName = hero,
                        Value = deaths > 0 ? deaths : (double?)null,
                    };

                    HeroStatsDeathsCollection.Add(statsOverviewHeroes);
                }

                // most kills
                {
                    int kills = Database.ReplaysDb().Statistics.ReadStatValue(hero, season, gameModes, OverviewHeroStatOption.MostKills);

                    StatsOverviewHeroes statsOverviewHeroes = new StatsOverviewHeroes()
                    {
                        HeroName = hero,
                        Value = kills > 0 ? kills : (double?)null,
                    };

                    HeroStatsKillsCollection.Add(statsOverviewHeroes);
                }

                // most assists
                {
                    int assists = Database.ReplaysDb().Statistics.ReadStatValue(hero, season, gameModes, OverviewHeroStatOption.MostAssists);

                    StatsOverviewHeroes statsOverviewHeroes = new StatsOverviewHeroes()
                    {
                        HeroName = hero,
                        Value = assists > 0 ? assists : (double?)null,
                    };

                    HeroStatsAssistsCollection.Add(statsOverviewHeroes);
                }
            }

            IsHeroStatPercentageDataGridVisible = false;
            IsHeroStatDataGridVisible = false;

            // set according to selected hero stat
            if (statOption == OverviewHeroStatOption.HighestWinrate)
            {
                IsHeroStatPercentageDataGridVisible = true;
                await Application.Current.Dispatcher.InvokeAsync(() => { HeroStatsPercentageCollection = new ObservableCollection<StatsOverviewHeroes>(heroStatPercentageCollection.OrderByDescending(x => x.Value)); });
            }
            else if (statOption == OverviewHeroStatOption.MostWins)
            {
                IsHeroStatDataGridVisible = true;
                await Application.Current.Dispatcher.InvokeAsync(() => { HeroStatsCollection = new ObservableCollection<StatsOverviewHeroes>(HeroStatsWinsCollection.OrderByDescending(x => x.Value)); });
            }
            else if (statOption == OverviewHeroStatOption.MostDeaths)
            {
                IsHeroStatDataGridVisible = true;
                await Application.Current.Dispatcher.InvokeAsync(() => { HeroStatsCollection = new ObservableCollection<StatsOverviewHeroes>(HeroStatsDeathsCollection.OrderByDescending(x => x.Value)); });
            }
            else if (statOption == OverviewHeroStatOption.MostKills)
            {
                IsHeroStatDataGridVisible = true;
                await Application.Current.Dispatcher.InvokeAsync(() => { HeroStatsCollection = new ObservableCollection<StatsOverviewHeroes>(HeroStatsKillsCollection.OrderByDescending(x => x.Value)); });
            }
            else if (statOption == OverviewHeroStatOption.MostAssists)
            {
                IsHeroStatDataGridVisible = true;
                await Application.Current.Dispatcher.InvokeAsync(() => { HeroStatsCollection = new ObservableCollection<StatsOverviewHeroes>(HeroStatsAssistsCollection.OrderByDescending(x => x.Value)); });
            }
        }

        private async Task SetMapStats(Season season, GameMode gameModes)
        {
            var mapList = HeroesIcons.MapBackgrounds().GetMapsList();
            var mapStatTempCollection = new Collection<StatsOverviewMaps>();

            foreach (var map in mapList)
            {
                int wins = Database.ReplaysDb().Statistics.ReadMapResults(season, gameModes, true, map);
                int losses = Database.ReplaysDb().Statistics.ReadMapResults(season, gameModes, false, map);
                int total = wins + losses;

                StatsOverviewMaps statsOverviewMaps = new StatsOverviewMaps()
                {
                    MapName = map,
                    Wins = total > 0 ? wins : (int?)null,
                    Losses = total > 0 ? losses : (int?)null,
                    Winrate = total > 0 ? Utilities.CalculateWinValue(wins, total) : (double?)null,
                };

                mapStatTempCollection.Add(statsOverviewMaps);
            }

            await Application.Current.Dispatcher.InvokeAsync(() => { MapsStatsCollection = new ObservableCollection<StatsOverviewMaps>(mapStatTempCollection.OrderByDescending(x => x.Winrate)); });
        }

        /// <summary>
        /// Call after all the Observable collections / Collections have been set
        /// </summary>
        private void SetOverallStats()
        {
            OverallGamesPlayed = MapsStatsCollection.Sum(x => (x.Wins ?? 0) + (x.Losses ?? 0));
            OverallWinrate = Utilities.CalculateWinValue(MapsStatsCollection.Sum(x => x.Wins ?? 0), OverallGamesPlayed);

            int totalKills = (int)HeroStatsKillsCollection.Sum(x => x.Value);
            int totalDeaths = (int)HeroStatsDeathsCollection.Sum(x => x.Value);
            int totalAssists = (int)HeroStatsAssistsCollection.Sum(x => x.Value);

            OverallKDARatio = totalDeaths != 0 ? Math.Round((totalKills + totalAssists) / (double)totalDeaths, 1) : 0.0;
            OverallTotalTakedowns = totalKills + totalAssists;
            OverallAverageTakedowns = OverallGamesPlayed != 0 ? Math.Round((totalKills + totalAssists) / (double)OverallGamesPlayed, 1) : 0.0;
        }

        private GameMode SetSelectedGameModes()
        {
            GameMode gameModes = GameMode.Unknown;

            if (!IsQuickMatchSelected && !IsUnrankedDraftSelected && !IsHeroLeagueSelected && !IsTeamLeagueSelected && !IsCustomGameSelected && !IsBrawlSelected)
            {
                gameModes = GameMode.QuickMatch | GameMode.UnrankedDraft | GameMode.HeroLeague | GameMode.TeamLeague;
                IsQuickMatchSelected = true;
                IsUnrankedDraftSelected = true;
                IsHeroLeagueSelected = true;
                IsTeamLeagueSelected = true;
            }
            else
            {
                if (IsQuickMatchSelected)
                    gameModes |= GameMode.QuickMatch;
                if (IsUnrankedDraftSelected)
                    gameModes |= GameMode.UnrankedDraft;
                if (IsHeroLeagueSelected)
                    gameModes |= GameMode.HeroLeague;
                if (IsTeamLeagueSelected)
                    gameModes |= GameMode.TeamLeague;
                if (IsCustomGameSelected)
                    gameModes |= GameMode.Custom;
                if (IsBrawlSelected)
                    gameModes |= GameMode.Brawl;
            }

            return gameModes;
        }

        private async Task SetRoleStats(Season season, GameMode gameModes)
        {
            int warriorTotal = 0;
            int assassinTotal = 0;
            int supportTotal = 0;
            int specialistTotal = 0;
            int multiclassTotal = 0;
            int warriorWin = 0;
            int assassinWin = 0;
            int supportWin = 0;
            int specialistWin = 0;
            int multiclassWin = 0;

            var listHeroesPlayed = Database.ReplaysDb().Statistics.ReadListOfMatchPlayerHeroes(season, gameModes);

            foreach (var hero in listHeroesPlayed)
            {
                var heroRoles = HeroesIcons.Heroes().GetHeroInfo(hero.Character).Roles;

                switch (heroRoles[0])
                {
                    case HeroRole.Warrior:
                        {
                            warriorTotal++;
                            if (hero.IsWinner)
                                warriorWin++;
                            break;
                        }

                    case HeroRole.Assassin:
                        {
                            assassinTotal++;
                            if (hero.IsWinner)
                                assassinWin++;
                            break;
                        }

                    case HeroRole.Support:
                        {
                            supportTotal++;
                            if (hero.IsWinner)
                                supportWin++;
                            break;
                        }

                    case HeroRole.Specialist:
                        {
                            specialistTotal++;
                            if (hero.IsWinner)
                                specialistWin++;
                            break;
                        }

                    case HeroRole.Multiclass:
                        {
                            multiclassTotal++;
                            if (hero.IsWinner)
                                multiclassWin++;
                            break;
                        }

                    default:
                        ExceptionLog.Log(LogLevel.Info, $"SetRoleStats(): Hero: {hero.Character}, could not find role for hero or unknown hero name");
                        break;
                }
            }

            double warriorWinRate = Utilities.CalculateWinValue(warriorWin, warriorTotal);
            double assassinWinRate = Utilities.CalculateWinValue(assassinWin, assassinTotal);
            double supportWinRate = Utilities.CalculateWinValue(supportWin, supportTotal);
            double specialistWinRate = Utilities.CalculateWinValue(specialistWin, specialistTotal);
            double multiclassWinRate = Utilities.CalculateWinValue(multiclassWin, multiclassTotal);

            await Application.Current.Dispatcher.InvokeAsync(() =>
            {
                RoleGamesCollection.Add(warriorTotal);
                RoleGamesCollection.Add(assassinTotal);
                RoleGamesCollection.Add(supportTotal);
                RoleGamesCollection.Add(specialistTotal);
                RoleGamesCollection.Add(multiclassTotal);

                RoleWinrateCollection.Add(warriorWinRate);
                RoleWinrateCollection.Add(assassinWinRate);
                RoleWinrateCollection.Add(supportWinRate);
                RoleWinrateCollection.Add(specialistWinRate);
                RoleWinrateCollection.Add(multiclassWinRate);
            });
        }

        private void Clear()
        {
            ClearHeroStatGridOnly();

            MapsStatsCollection = null;

            OverallGamesPlayed = 0;
            OverallWinrate = 0.0;
            OverallKDARatio = 0.0;
            OverallTotalTakedowns = 0;
            OverallAverageTakedowns = 0;

            RoleGamesCollection = new ObservableCollection<int>();
            RoleWinrateCollection = new ObservableCollection<double>();
        }

        private void ClearHeroStatGridOnly()
        {
            IsHeroStatPercentageDataGridVisible = true;
            IsHeroStatDataGridVisible = false;

            HeroStatsCollection = null;
            HeroStatsPercentageCollection = null;

            HeroStatsWinsCollection = new Collection<StatsOverviewHeroes>();
            HeroStatsDeathsCollection = new Collection<StatsOverviewHeroes>();
            HeroStatsKillsCollection = new Collection<StatsOverviewHeroes>();
            HeroStatsAssistsCollection = new Collection<StatsOverviewHeroes>();
        }
    }
}
