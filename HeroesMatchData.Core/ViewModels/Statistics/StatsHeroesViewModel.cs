using GalaSoft.MvvmLight.CommandWpf;
using Heroes.Helpers;
using Heroes.ReplayParser;
using HeroesMatchData.Core.Services;
using HeroesMatchData.Core.ViewServices;
using NLog;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Imaging;

namespace HeroesMatchData.Core.ViewModels.Statistics
{
    public class StatsHeroesViewModel : HmdViewModel
    {
        private readonly string InitialHeroListOption = "- Select Hero -";
        private readonly string InitialSeasonListOption = "- Select Season -";

        private string _selectedSeason;
        private string _selectedHero;
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

        public RelayCommand QueryStatsCommand => new RelayCommand(async () => await QueryStatsHeroStatsAsyncCommmand());
        public RelayCommand<object> SelectedGameModesCommand => new RelayCommand<object>((list) => SetSelectedGameModes(list));
        public RelayCommand<object> SelectedMapListCommand => new RelayCommand<object>((list) => SetSelectedMaps(list));

        public StatsHeroesDataViewModel StatsHeroesDataViewModel { get => _statsHeroesDataViewModel; set => _statsHeroesDataViewModel = value; }

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
                }
            });

            LoadingOverlayWindow.CloseLoadingOverlay();
        }

        private async Task QueryStatsHeroStatsAsync()
        {
            if (SelectedHero == InitialHeroListOption || string.IsNullOrEmpty(SelectedHero) ||
                SelectedSeason == InitialSeasonListOption || string.IsNullOrEmpty(SelectedSeason))
                return;

            SelectedHeroPortrait = HeroesIcons.Heroes().GetHeroPortrait(SelectedHero);
            Season selectedSeason = HeroesHelpers.EnumParser.ConvertSeasonStringToEnum(SelectedSeason);

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

            if (SelectedMaps.Count <= 0)
            {
                SelectedMaps = HeroesIcons.MapBackgrounds().GetMapsListExceptCustomOnly();
            }

            await StatsHeroesDataViewModel.SetData(SelectedHero, selectedSeason, gameModes, SelectedMaps);
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
