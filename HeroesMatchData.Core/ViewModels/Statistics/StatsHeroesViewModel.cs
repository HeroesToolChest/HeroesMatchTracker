using GalaSoft.MvvmLight.CommandWpf;
using Heroes.Helpers;
using Heroes.ReplayParser;
using HeroesMatchData.Core.Services;
using System.Collections.Generic;
using System.Windows.Media.Imaging;

namespace HeroesMatchData.Core.ViewModels.Statistics
{
    public class StatsHeroesViewModel : HmdViewModel
    {
        private string _selectedSeasonOption;
        private string _selectedHero;
        private BitmapImage _selectedHeroPortrait;

        private List<string> AllNonCustomMaps;
        private List<string> AllMaps;

        private HeroesGameModeQuickMatchViewModel _heroesGameModeQuickMatchViewModel;
        private HeroesGameModeUnrankedDraftViewModel _heroesGameModeUnrankedDraftViewModel;
        private HeroesGameModeHeroLeagueViewModel _heroesGameModeHeroLeagueViewModel;
        private HeroesGameModeTeamLeagueViewModel _heroesGameModeTeamLeagueViewModel;
        private HeroesGameModeCustomGameViewModel _heroesGameModeCustomGameViewModel;
        private HeroesGameModeBrawlViewModel _heroesGameModeBrawlViewModel;

        public StatsHeroesViewModel(IInternalService internalService)
            : base(internalService)
        {
            HeroesGameModeQuickMatchViewModel = new HeroesGameModeQuickMatchViewModel(internalService);
            HeroesGameModeUnrankedDraftViewModel = new HeroesGameModeUnrankedDraftViewModel(internalService);
            HeroesGameModeHeroLeagueViewModel = new HeroesGameModeHeroLeagueViewModel(internalService);
            HeroesGameModeTeamLeagueViewModel = new HeroesGameModeTeamLeagueViewModel(internalService);
            HeroesGameModeCustomGameViewModel = new HeroesGameModeCustomGameViewModel(internalService);
            HeroesGameModeBrawlViewModel = new HeroesGameModeBrawlViewModel(internalService);

            AllNonCustomMaps = HeroesIcons.MapBackgrounds().GetMapsListExceptCustomOnly();
            AllMaps = HeroesIcons.MapBackgrounds().GetMapsList();

            SeasonList.Add("Lifetime");
            SeasonList.AddRange(HeroesHelpers.Seasons.GetSeasonList());
            SelectedSeasonOption = SeasonList[0];

            HeroesList.Add("- Select Hero -");
            HeroesList.AddRange(HeroesIcons.Heroes().GetListOfHeroes());
            SelectedHero = HeroesList[0];
        }

        public List<string> SeasonList { get; private set; } = new List<string>();
        public List<string> HeroesList { get; private set; } = new List<string>();

        public string SelectedSeasonOption
        {
            get => _selectedSeasonOption;
            set
            {
                _selectedSeasonOption = value;
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

        public RelayCommand LoadHeroStatsCommand => new RelayCommand(LoadHeroStats);

        public HeroesGameModeQuickMatchViewModel HeroesGameModeQuickMatchViewModel { get => _heroesGameModeQuickMatchViewModel; set => _heroesGameModeQuickMatchViewModel = value; }
        public HeroesGameModeUnrankedDraftViewModel HeroesGameModeUnrankedDraftViewModel { get => _heroesGameModeUnrankedDraftViewModel; set => _heroesGameModeUnrankedDraftViewModel = value; }
        public HeroesGameModeHeroLeagueViewModel HeroesGameModeHeroLeagueViewModel { get => _heroesGameModeHeroLeagueViewModel; set => _heroesGameModeHeroLeagueViewModel = value; }
        public HeroesGameModeTeamLeagueViewModel HeroesGameModeTeamLeagueViewModel { get => _heroesGameModeTeamLeagueViewModel; set => _heroesGameModeTeamLeagueViewModel = value; }
        public HeroesGameModeCustomGameViewModel HeroesGameModeCustomGameViewModel { get => _heroesGameModeCustomGameViewModel; set => _heroesGameModeCustomGameViewModel = value; }
        public HeroesGameModeBrawlViewModel HeroesGameModeBrawlViewModel { get => _heroesGameModeBrawlViewModel; set => _heroesGameModeBrawlViewModel = value; }

        private void LoadHeroStats()
        {
            if (SelectedHero == "- Select Hero -" || string.IsNullOrEmpty(SelectedHero) || string.IsNullOrEmpty(SelectedSeasonOption))
                return;

            Season selectedSeason = HeroesHelpers.EnumParser.ConvertSeasonStringToEnum(SelectedSeasonOption);
            SelectedHeroPortrait = HeroesIcons.Heroes().GetHeroLoadingPortrait(SelectedHero);

            HeroesGameModeQuickMatchViewModel.SetData(AllNonCustomMaps, SelectedHero, selectedSeason, GameMode.QuickMatch);
            HeroesGameModeUnrankedDraftViewModel.SetData(AllNonCustomMaps, SelectedHero, selectedSeason, GameMode.UnrankedDraft);
            HeroesGameModeHeroLeagueViewModel.SetData(AllNonCustomMaps, SelectedHero, selectedSeason, GameMode.HeroLeague);
            HeroesGameModeTeamLeagueViewModel.SetData(AllNonCustomMaps, SelectedHero, selectedSeason, GameMode.TeamLeague);
            HeroesGameModeCustomGameViewModel.SetData(AllNonCustomMaps, SelectedHero, selectedSeason, GameMode.Custom);
            HeroesGameModeBrawlViewModel.SetData(AllNonCustomMaps, SelectedHero, selectedSeason, GameMode.Brawl);
        }
    }
}
