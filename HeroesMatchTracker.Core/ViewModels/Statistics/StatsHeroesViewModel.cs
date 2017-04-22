using GalaSoft.MvvmLight.CommandWpf;
using Heroes.Helpers;
using Heroes.ReplayParser;
using HeroesMatchTracker.Core.Services;
using HeroesMatchTracker.Core.ViewServices;
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
        private int _heroWinrate;
        private int _heroKills;
        private int _heroAssists;
        private int _heroDeaths;
        private int _heroKADRatio;
        private int _heroAwards;
        private int _heroMVP;
        private int _heroAwardsRatio;
        private int _heroMVPRatio;
        private bool _isTotalsAveragesChecked;
        private bool _isTalentsChecked;
        private bool _isAwardsChecked;
        private double _heroKAD;
        private double _heroKD;
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
        private List<string> SelectedBuilds = new List<string>();

        public StatsHeroesViewModel(IInternalService internalService, ILoadingOverlayWindowService loadingOverlayWindow)
            : base(internalService)
        {
            LoadingOverlayWindow = loadingOverlayWindow;

            IsTotalsAveragesChecked = true;
            IsTalentsChecked = true;
            IsAwardsChecked = true;

            SeasonList.Add(InitialSeasonListOption);
            SeasonList.Add("Lifetime");
            SeasonList.AddRange(HeroesHelpers.Seasons.GetSeasonList());
            SelectedSeason = SeasonList[0];

            HeroesList.Add(InitialHeroListOption);
            HeroesList.AddRange(HeroesIcons.Heroes().GetListOfHeroes());
            SelectedHero = HeroesList[0];

            GameModeList.AddRange(HeroesHelpers.GameModes.GetAllGameModeList());
            MapList.AddRange(HeroesIcons.MapBackgrounds().GetMapsList());

            StatsHeroesDataViewModel = new StatsHeroesDataViewModel(internalService, MapList);
        }

        public List<string> SeasonList { get; private set; } = new List<string>();
        public List<string> HeroesList { get; private set; } = new List<string>();
        public List<string> GameModeList { get; private set; } = new List<string>();
        public List<string> MapList { get; private set; } = new List<string>();
        public List<string> TimeList { get; private set; } = new List<string>();
        public List<string> BuildsList { get; private set; } = new List<string>();

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

        public int HeroWinrate
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

        public int HeroKADRatio
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

        public int HeroAwardsRatio
        {
            get => _heroAwardsRatio;
            set
            {
                _heroAwardsRatio = value;
                RaisePropertyChanged();
            }
        }

        public int HeroMVPRatio
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
            LoadingOverlayWindow.ShowLoadingOverlay();

            await Task.Run(async () =>
            {
                try
                {
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

            Season selectedSeason = HeroesHelpers.EnumParser.ConvertSeasonStringToEnum(SelectedSeason);

            SelectedHeroPortrait = HeroesIcons.Heroes().GetHeroPortrait(SelectedHero);
            HeroName = SelectedHero;
            HeroRole = HeroesIcons.Heroes().GetHeroRoleList(SelectedHero)[0].ToString();
            HeroLevel = Database.ReplaysDb().MatchPlayer.ReadHighestLevelOfHero(SelectedHero, selectedSeason).ToString();

            // set selected gamemodes
            GameMode gameModes = GameMode.Unknown;
            if (SelectedGameModes.Count <= 0)
            {
                gameModes = GameMode.QuickMatch | GameMode.UnrankedDraft | GameMode.HeroLeague | GameMode.TeamLeague;
            }
            else
            {
                foreach (var gameMode in SelectedGameModes)
                {
                    gameModes |= HeroesHelpers.EnumParser.ConvertGameModeStringToEnum(gameMode);
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

            HeroWins = StatsHeroesDataViewModel.StatsHeroesDataTotalCollection[0].Wins;
            HeroLosses = StatsHeroesDataViewModel.StatsHeroesDataTotalCollection[0].Losses;
            HeroGames = HeroWins + HeroLosses;
            HeroWinrate = StatsHeroesDataViewModel.StatsHeroesDataTotalCollection[0].WinPercentage.Value;

            HeroKills = StatsHeroesDataViewModel.StatsHeroesDataTotalCollection[0].Kills;
            HeroAssists = StatsHeroesDataViewModel.StatsHeroesDataTotalCollection[0].Assists;
            HeroDeaths = StatsHeroesDataViewModel.StatsHeroesDataTotalCollection[0].Deaths;
            HeroKD = Math.Round((double)HeroKills / HeroDeaths, 2);
            HeroKAD = Math.Round((double)(HeroKills + HeroAssists) / HeroDeaths, 2);
            HeroKADRatio = Utilities.CalculateWinPercentage(HeroKills + HeroAssists, HeroKills + HeroAssists + HeroDeaths);

            HeroAwards = StatsHeroesDataViewModel.StatsHeroesAwardsTotalCollection[0].Total;
            HeroMVP = StatsHeroesDataViewModel.MVPCount;
            HeroAwardsRatio = Utilities.CalculateWinPercentage(HeroAwards, HeroGames);
            HeroMVPRatio = Utilities.CalculateWinPercentage(HeroMVP, HeroGames);
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
