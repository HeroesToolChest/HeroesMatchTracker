using GalaSoft.MvvmLight.CommandWpf;
using Heroes.Helpers;
using Heroes.Icons.Models;
using HeroesMatchTracker.Core.Services;
using HeroesMatchTracker.Core.ViewServices;
using Microsoft.Practices.ServiceLocation;
using NLog;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace HeroesMatchTracker.Core.ViewModels.Statistics
{
    public class StatsHeroesViewModel : HmtViewModel
    {
        private readonly string InitialHeroListOption = "- Select Hero -";
        private readonly string InitialSeasonListOption = "- Select Season -";

        private int _heroWins;
        private int _heroLosses;
        private int _heroGames;
        private int _heroKills;
        private int _heroAssists;
        private int _heroDeaths;
        private int _heroAwards;
        private int _heroMVP;
        private bool _isTotalsAveragesChecked;
        private bool _isTalentsChecked;
        private bool _isAwardsChecked;
        private double _heroKADRatio;
        private double _heroWinrate;
        private double _heroKAD;
        private double _heroKD;
        private double _heroAwardsRatio;
        private double _heroMVPRatio;
        private string _selectedSeason;
        private string _selectedHero;
        private string _heroName;
        private string _heroRole;
        private string _heroLevel;

        private BitmapImage _selectedHeroPortrait;

        private StatsHeroesDataViewModel _statsHeroesDataViewModel;

        private ILoadingOverlayWindowService LoadingOverlayWindow;

        private List<string> SelectedGameModes = new List<string>();
        private List<string> SelectedMaps = new List<string>();

        public StatsHeroesViewModel(IInternalService internalService, ILoadingOverlayWindowService loadingOverlayWindow)
            : base(internalService)
        {
            LoadingOverlayWindow = loadingOverlayWindow;

            IsTotalsAveragesChecked = true;
            IsTalentsChecked = true;
            IsAwardsChecked = true;

            SeasonList.Add(InitialSeasonListOption);
            SeasonList.AddRange(HeroesHelpers.Seasons.GetSeasonList());
            SelectedSeason = SeasonList[0];

            HeroesList.Add(InitialHeroListOption);
            HeroesList.AddRange(HeroesIcons.Heroes().GetListOfHeroes(HeroesIcons.GetLatestHeroesBuild()));
            SelectedHero = HeroesList[0];

            GameModeList.AddRange(HeroesHelpers.GameModes.GetAllGameModeList());
            MapList.AddRange(HeroesIcons.MapBackgrounds().GetMapsList());

            StatsHeroesDataViewModel = new StatsHeroesDataViewModel(internalService, MapList);
        }

        public List<string> SeasonList { get; private set; } = new List<string>();
        public List<string> HeroesList { get; private set; } = new List<string>();
        public List<string> GameModeList { get; private set; } = new List<string>();
        public List<string> MapList { get; private set; } = new List<string>();

        public string SelectedSeason
        {
            get => _selectedSeason;
            set
            {
                _selectedSeason = value;
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

        public BitmapImage SelectedHeroPortrait
        {
            get => _selectedHeroPortrait;
            set
            {
                _selectedHeroPortrait = value;
                RaisePropertyChanged();
            }
        }

        public bool IsTotalsAveragesChecked
        {
            get => _isTotalsAveragesChecked;
            set
            {
                _isTotalsAveragesChecked = value;
                RaisePropertyChanged();
            }
        }

        public bool IsTalentsChecked
        {
            get => _isTalentsChecked;
            set
            {
                _isTalentsChecked = value;
                RaisePropertyChanged();
            }
        }

        public bool IsAwardsChecked
        {
            get => _isAwardsChecked;
            set
            {
                _isAwardsChecked = value;
                RaisePropertyChanged();
            }
        }

        public IMainWindowDialogsService MainWindowDialog => ServiceLocator.Current.GetInstance<IMainWindowDialogsService>();

        public RelayCommand QueryStatsCommand => new RelayCommand(async () => await QueryStatsHeroStatsAsyncCommmand());
        public RelayCommand<object> SelectedGameModesCommand => new RelayCommand<object>((list) => SetSelectedGameModes(list));
        public RelayCommand<object> SelectedMapListCommand => new RelayCommand<object>((list) => SetSelectedMaps(list));

        public StatsHeroesDataViewModel StatsHeroesDataViewModel { get => _statsHeroesDataViewModel; set => _statsHeroesDataViewModel = value; }
        public int HeroWins
        {
            get => _heroWins;
            set
            {
                _heroWins = value;
                RaisePropertyChanged();
            }
        }

        public int HeroLosses
        {
            get => _heroLosses;
            set
            {
                _heroLosses = value;
                RaisePropertyChanged();
            }
        }

        public int HeroGames
        {
            get => _heroGames;
            set
            {
                _heroGames = value;
                RaisePropertyChanged();
            }
        }

        public string HeroName
        {
            get => _heroName;
            set
            {
                _heroName = value;
                RaisePropertyChanged();
            }
        }

        public string HeroRole
        {
            get => _heroRole;
            set
            {
                _heroRole = value;
                RaisePropertyChanged();
            }
        }

        public string HeroLevel
        {
            get => _heroLevel;
            set
            {
                _heroLevel = value;
                RaisePropertyChanged();
            }
        }

        public double HeroWinrate
        {
            get => _heroWinrate;
            set
            {
                _heroWinrate = value;
                RaisePropertyChanged();
            }
        }

        public int HeroKills
        {
            get => _heroKills;
            set
            {
                _heroKills = value;
                RaisePropertyChanged();
            }
        }

        public int HeroAssists
        {
            get => _heroAssists;
            set
            {
                _heroAssists = value;
                RaisePropertyChanged();
            }
        }

        public int HeroDeaths
        {
            get => _heroDeaths;
            set
            {
                _heroDeaths = value;
                RaisePropertyChanged();
            }
        }

        public double HeroKADRatio
        {
            get => _heroKADRatio;
            set
            {
                _heroKADRatio = value;
                RaisePropertyChanged();
            }
        }

        public double HeroKAD
        {
            get => _heroKAD;
            set
            {
                _heroKAD = value;
                RaisePropertyChanged();
            }
        }

        public double HeroKD
        {
            get => _heroKD;
            set
            {
                _heroKD = value;
                RaisePropertyChanged();
            }
        }

        public double HeroAwardsRatio
        {
            get => _heroAwardsRatio;
            set
            {
                _heroAwardsRatio = value;
                RaisePropertyChanged();
            }
        }

        public double HeroMVPRatio
        {
            get => _heroMVPRatio;
            set
            {
                _heroMVPRatio = value;
                RaisePropertyChanged();
            }
        }

        public int HeroAwards
        {
            get => _heroAwards;
            set
            {
                _heroAwards = value;
                RaisePropertyChanged();
            }
        }

        public int HeroMVP
        {
            get => _heroMVP;
            set
            {
                _heroMVP = value;
                RaisePropertyChanged();
            }
        }

        private async Task QueryStatsHeroStatsAsyncCommmand()
        {
            if (await MainWindowDialog.CheckBattleTagSetDialog())
                return;

            await Task.Run(async () =>
            {
                try
                {
                    LoadingOverlayWindow.ShowLoadingOverlay();
                    await QueryStatsHeroStatsAsync();
                }
                catch (Exception ex)
                {
                    ExceptionLog.Log(LogLevel.Error, ex);
                    throw;
                }
            });

            LoadingOverlayWindow.CloseLoadingOverlay();
        }

        private async Task QueryStatsHeroStatsAsync()
        {
            if (SelectedHero == InitialHeroListOption || string.IsNullOrEmpty(SelectedHero) ||
                SelectedSeason == InitialSeasonListOption || string.IsNullOrEmpty(SelectedSeason))
                return;

            HeroesIcons.LoadLatestHeroesBuild();

            Enum.TryParse(SelectedSeason, out Season selectedSeason);
            Hero hero = HeroesIcons.Heroes().GetHeroInfo(SelectedHero);

            SelectedHeroPortrait = hero.GetPortrait();
            HeroName = SelectedHero;
            HeroRole = hero.Roles[0].ToString();
            HeroLevel = Database.ReplaysDb().MatchPlayer.ReadHighestLevelOfHero(SelectedHero, selectedSeason).ToString();

            // set selected gamemodes
            GameMode gameModes = GameMode.Unknown;
            if (SelectedGameModes.Count <= 0)
            {
                gameModes = GameMode.AllGameMode;
            }
            else
            {
                foreach (var gameMode in SelectedGameModes)
                {
                    gameModes |= gameMode.ConvertToEnum<GameMode>();
                }
            }

            // set selected maps
            if (SelectedMaps.Count <= 0)
            {
                SelectedMaps = HeroesIcons.MapBackgrounds().GetMapsListExceptCustomOnly();
            }

            // query the data
            StatsHeroesDataViewModel.QueryTotalsAndAverages = IsTotalsAveragesChecked;
            StatsHeroesDataViewModel.QueryTalents = IsTalentsChecked;
            StatsHeroesDataViewModel.QueryAwards = IsAwardsChecked;
            await StatsHeroesDataViewModel.SetDataAsync(SelectedHero, selectedSeason, gameModes, SelectedMaps);

            if (IsTotalsAveragesChecked)
            {
                HeroWins = StatsHeroesDataViewModel.StatsHeroesDataTotalCollection[0].Wins ?? 0;
                HeroLosses = StatsHeroesDataViewModel.StatsHeroesDataTotalCollection[0].Losses ?? 0;
                HeroGames = HeroWins + HeroLosses;
                HeroWinrate = Math.Round((StatsHeroesDataViewModel.StatsHeroesDataTotalCollection[0].WinPercentage ?? 0) * 100, 1);

                HeroKills = StatsHeroesDataViewModel.StatsHeroesDataTotalCollection[0].Kills ?? 0;
                HeroAssists = StatsHeroesDataViewModel.StatsHeroesDataTotalCollection[0].Assists ?? 0;
                HeroDeaths = StatsHeroesDataViewModel.StatsHeroesDataTotalCollection[0].Deaths ?? 0;
                HeroKD = Utilities.CalculateWinPercentage(HeroKills, HeroDeaths) / 100;
                HeroKAD = Utilities.CalculateWinPercentage(HeroKills + HeroAssists, HeroDeaths) / 100;
                HeroKADRatio = Utilities.CalculateWinPercentage(HeroKills + HeroAssists, HeroKills + HeroAssists + HeroDeaths);
            }

            if (IsAwardsChecked)
            {
                HeroAwards = StatsHeroesDataViewModel.StatsHeroesAwardsTotalCollection[0].Total ?? 0;
                HeroMVP = StatsHeroesDataViewModel.MVPCount;
                HeroAwardsRatio = Utilities.CalculateWinPercentage(HeroAwards, HeroGames);
                HeroMVPRatio = Utilities.CalculateWinPercentage(HeroMVP, HeroGames);
            }
        }

        private void SetSelectedGameModes(object list)
        {
            SelectedGameModes = ((IEnumerable)list).Cast<string>().ToList();
        }

        private void SetSelectedMaps(object list)
        {
            SelectedMaps = ((IEnumerable)list).Cast<string>().ToList();
        }
    }
}
