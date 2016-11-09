using Heroes.ReplayParser;
using HeroesIcons;
using HeroesParserData.DataQueries;
using HeroesParserData.Messages;
using HeroesParserData.Models.StatsModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Imaging;

namespace HeroesParserData.ViewModels.Stats
{
    public class HeroStatsHeroesViewModel : HeroStatsContext
    {
        #region private properties
        private int? TotalMVPCount;

        private string _selectedHero;
        private List<string> _heroesList;
        private BitmapImage _characterDraftPortrait;
        private string _characterName;
        private string _characterRole;
        private string _totalWinrate;
        private string _selectedAwardMap;
        private string _selectedTalentMap;
        private string _selectedTalentGameMode;
        private int? _characterLevel;
        private int? _totalWins;
        private int? _totalLosses;
        private int? _totalGames;
        private int? _totalKills;
        private int? _totalDeaths;
        private int? _totalAwards;
        private int? _totalMVPs;
        private bool _isTotalWinsProcessing;
        private bool _isTotalLossesProcessing;
        private bool _isTotalGamesProcessing;
        private bool _isTotalWinrateProcessing;
        private bool _isTotalKillsProcessing;
        private bool _isTotalDeathsProcessing;
        private bool _isTotalAwardsProcessing;
        private bool _isTotalMVPsProcessing;

        private ObservableCollection<StatsHeroesMapMatch> _statsHeroesQuickMatchDataCollection = new ObservableCollection<StatsHeroesMapMatch>();
        private ObservableCollection<StatsHeroesMapMatch> _statsHeroesUnrankedDraftDataCollection = new ObservableCollection<StatsHeroesMapMatch>();
        private ObservableCollection<StatsHeroesMapMatch> _statsHeroesHeroLeagueDataCollection = new ObservableCollection<StatsHeroesMapMatch>();
        private ObservableCollection<StatsHeroesMapMatch> _statsHeroesTeamLeagueDataCollection = new ObservableCollection<StatsHeroesMapMatch>();
        private ObservableCollection<StatsHeroesMapMatch> _statsHeroesCustomGameDataCollection = new ObservableCollection<StatsHeroesMapMatch>();

        private ObservableCollection<StatsHeroesMapMatch> _statsHeroesQuickMatchDataTotalCollection = new ObservableCollection<StatsHeroesMapMatch>();
        private ObservableCollection<StatsHeroesMapMatch> _statsHeroesUnrankedDraftDataTotalCollection = new ObservableCollection<StatsHeroesMapMatch>();
        private ObservableCollection<StatsHeroesMapMatch> _statsHeroesHeroLeagueDataTotalCollection = new ObservableCollection<StatsHeroesMapMatch>();
        private ObservableCollection<StatsHeroesMapMatch> _statsHeroesTeamLeagueDataTotalCollection = new ObservableCollection<StatsHeroesMapMatch>();
        private ObservableCollection<StatsHeroesMapMatch> _statsHeroesCustomGameDataTotalCollection = new ObservableCollection<StatsHeroesMapMatch>();

        private ObservableCollection<StatsHeroesMapMatch> _statsHeroesQuickMatchDataAverageCollection = new ObservableCollection<StatsHeroesMapMatch>();
        private ObservableCollection<StatsHeroesMapMatch> _statsHeroesUnrankedDraftDataAverageCollection = new ObservableCollection<StatsHeroesMapMatch>();
        private ObservableCollection<StatsHeroesMapMatch> _statsHeroesHeroLeagueDataAverageCollection = new ObservableCollection<StatsHeroesMapMatch>();
        private ObservableCollection<StatsHeroesMapMatch> _statsHeroesTeamLeagueDataAverageCollection = new ObservableCollection<StatsHeroesMapMatch>();
        private ObservableCollection<StatsHeroesMapMatch> _statsHeroesCustomGameDataAverageCollection = new ObservableCollection<StatsHeroesMapMatch>();

        private ObservableCollection<StatsHeroesMapMatch> _statsHeroesQuickMatchDataAverageTotalCollection = new ObservableCollection<StatsHeroesMapMatch>();
        private ObservableCollection<StatsHeroesMapMatch> _statsHeroesUnrankedDraftDataAverageTotalCollection = new ObservableCollection<StatsHeroesMapMatch>();
        private ObservableCollection<StatsHeroesMapMatch> _statsHeroesHeroLeagueDataAverageTotalCollection = new ObservableCollection<StatsHeroesMapMatch>();
        private ObservableCollection<StatsHeroesMapMatch> _statsHeroesTeamLeagueDataAverageTotalCollection = new ObservableCollection<StatsHeroesMapMatch>();
        private ObservableCollection<StatsHeroesMapMatch> _statsHeroesCustomGameDataAverageTotalCollection = new ObservableCollection<StatsHeroesMapMatch>();

        private ObservableCollection<StatsHeroesMatchAwards> _matchAwardDataCollection = new ObservableCollection<StatsHeroesMatchAwards>();
        private ObservableCollection<StatsHeroesMatchAwards> _matchAwardDataTotalCollection = new ObservableCollection<StatsHeroesMatchAwards>();

        private ObservableCollection<StatsHeroesTalentPicks> _talentsPickLevel1DataCollection = new ObservableCollection<StatsHeroesTalentPicks>();
        private ObservableCollection<StatsHeroesTalentPicks> _talentsPickLevel4DataCollection = new ObservableCollection<StatsHeroesTalentPicks>();
        private ObservableCollection<StatsHeroesTalentPicks> _talentsPickLevel7DataCollection = new ObservableCollection<StatsHeroesTalentPicks>();
        private ObservableCollection<StatsHeroesTalentPicks> _talentsPickLevel10DataCollection = new ObservableCollection<StatsHeroesTalentPicks>();
        private ObservableCollection<StatsHeroesTalentPicks> _talentsPickLevel13DataCollection = new ObservableCollection<StatsHeroesTalentPicks>();
        private ObservableCollection<StatsHeroesTalentPicks> _talentsPickLevel16DataCollection = new ObservableCollection<StatsHeroesTalentPicks>();
        private ObservableCollection<StatsHeroesTalentPicks> _talentsPickLevel20DataCollection = new ObservableCollection<StatsHeroesTalentPicks>();
        #endregion private properties

        #region public properties
        public List<string> HeroesList
        {
            get { return _heroesList; }
            set
            {
                _heroesList = value;
                RaisePropertyChangedEvent(nameof(HeroesList));
            }
        }

        public string SelectedHero
        {
            get { return _selectedHero; }
            set
            {
                _selectedHero = value;
                RaisePropertyChangedEvent(nameof(SelectedHero));
            }
        }

        public BitmapImage CharacterDraftPortrait
        {
            get { return _characterDraftPortrait; }
            set
            {
                _characterDraftPortrait = value;
                RaisePropertyChangedEvent(nameof(CharacterDraftPortrait));
            }
        }

        public string CharacterName
        {
            get { return _characterName; }
            set
            {
                _characterName = value;
                RaisePropertyChangedEvent(nameof(CharacterName));
            }
        }

        public string CharacterRole
        {
            get { return _characterRole; }
            set
            {
                _characterRole = value;
                RaisePropertyChangedEvent(nameof(CharacterRole));
            }
        }

        public int? CharacterLevel
        {
            get { return _characterLevel; }
            set
            {
                _characterLevel = value;
                RaisePropertyChangedEvent(nameof(CharacterLevel));
            }
        }

        public ObservableCollection<StatsHeroesMapMatch> StatsHeroesQuickMatchDataCollection
        {
            get { return _statsHeroesQuickMatchDataCollection; }
            set
            {
                _statsHeroesQuickMatchDataCollection = value;
                RaisePropertyChangedEvent(nameof(StatsHeroesQuickMatchDataCollection));
            }
        }

        public ObservableCollection<StatsHeroesMapMatch> StatsHeroesUnrankedDraftDataCollection
        {
            get { return _statsHeroesUnrankedDraftDataCollection; }
            set
            {
                _statsHeroesUnrankedDraftDataCollection = value;
                RaisePropertyChangedEvent(nameof(StatsHeroesUnrankedDraftDataCollection));
            }
        }

        public ObservableCollection<StatsHeroesMapMatch> StatsHeroesHeroLeagueDataCollection
        {
            get { return _statsHeroesHeroLeagueDataCollection; }
            set
            {
                _statsHeroesHeroLeagueDataCollection = value;
                RaisePropertyChangedEvent(nameof(StatsHeroesHeroLeagueDataCollection));
            }
        }

        public ObservableCollection<StatsHeroesMapMatch> StatsHeroesTeamLeagueDataCollection
        {
            get { return _statsHeroesTeamLeagueDataCollection; }
            set
            {
                _statsHeroesTeamLeagueDataCollection = value;
                RaisePropertyChangedEvent(nameof(StatsHeroesTeamLeagueDataCollection));
            }
        }

        public ObservableCollection<StatsHeroesMapMatch> StatsHeroesCustomGameDataCollection
        {
            get { return _statsHeroesCustomGameDataCollection; }
            set
            {
                _statsHeroesCustomGameDataCollection = value;
                RaisePropertyChangedEvent(nameof(StatsHeroesCustomGameDataCollection));
            }
        }

        public ObservableCollection<StatsHeroesMapMatch> StatsHeroesQuickMatchDataTotalCollection
        {
            get { return _statsHeroesQuickMatchDataTotalCollection; }
            set
            {
                _statsHeroesQuickMatchDataTotalCollection = value;
                RaisePropertyChangedEvent(nameof(StatsHeroesQuickMatchDataTotalCollection));
            }
        }

        public ObservableCollection<StatsHeroesMapMatch> StatsHeroesUnrankedDraftDataTotalCollection
        {
            get { return _statsHeroesUnrankedDraftDataTotalCollection; }
            set
            {
                _statsHeroesUnrankedDraftDataTotalCollection = value;
                RaisePropertyChangedEvent(nameof(StatsHeroesUnrankedDraftDataTotalCollection));
            }
        }

        public ObservableCollection<StatsHeroesMapMatch> StatsHeroesHeroLeagueDataTotalCollection
        {
            get { return _statsHeroesHeroLeagueDataTotalCollection; }
            set
            {
                _statsHeroesHeroLeagueDataTotalCollection = value;
                RaisePropertyChangedEvent(nameof(StatsHeroesHeroLeagueDataTotalCollection));
            }
        }

        public ObservableCollection<StatsHeroesMapMatch> StatsHeroesTeamLeagueDataTotalCollection
        {
            get { return _statsHeroesTeamLeagueDataTotalCollection; }
            set
            {
                _statsHeroesTeamLeagueDataTotalCollection = value;
                RaisePropertyChangedEvent(nameof(StatsHeroesTeamLeagueDataTotalCollection));
            }
        }

        public ObservableCollection<StatsHeroesMapMatch> StatsHeroesCustomGameDataTotalCollection
        {
            get { return _statsHeroesCustomGameDataTotalCollection; }
            set
            {
                _statsHeroesCustomGameDataTotalCollection = value;
                RaisePropertyChangedEvent(nameof(StatsHeroesCustomGameDataTotalCollection));
            }
        }

        public ObservableCollection<StatsHeroesMatchAwards> MatchAwardDataCollection
        {
            get { return _matchAwardDataCollection; }
            set
            {
                _matchAwardDataCollection = value;
                RaisePropertyChangedEvent(nameof(MatchAwardDataCollection));
            }
        }

        public ObservableCollection<StatsHeroesMatchAwards> MatchAwardDataTotalCollection
        {
            get { return _matchAwardDataTotalCollection; }
            set
            {
                _matchAwardDataTotalCollection = value;
                RaisePropertyChangedEvent(nameof(MatchAwardDataTotalCollection));
            }
        }

        public int? TotalWins
        {
            get { return _totalWins; }
            set
            {
                _totalWins = value;
                RaisePropertyChangedEvent(nameof(TotalWins));
            }
        }

        public int? TotalLosses
        {
            get { return _totalLosses; }
            set
            {
                _totalLosses = value;
                RaisePropertyChangedEvent(nameof(TotalLosses));
            }
        }

        public int? TotalGames
        {
            get { return _totalGames; }
            set
            {
                _totalGames = value;
                RaisePropertyChangedEvent(nameof(TotalGames));
            }
        }

        public string TotalWinrate
        {
            get { return _totalWinrate; }
            set
            {
                _totalWinrate = value;
                RaisePropertyChangedEvent(nameof(TotalWinrate));
            }
        }

        public int? TotalKills
        {
            get { return _totalKills; }
            set
            {
                _totalKills = value;
                RaisePropertyChangedEvent(nameof(TotalKills));
            }
        }

        public int? TotalDeaths
        {
            get { return _totalDeaths; }
            set
            {
                _totalDeaths = value;
                RaisePropertyChangedEvent(nameof(TotalDeaths));
            }
        }

        public int? TotalMVPs
        {
            get { return _totalMVPs; }
            set
            {
                _totalMVPs = value;
                RaisePropertyChangedEvent(nameof(TotalMVPs));
            }
        }

        public int? TotalAwards
        {
            get { return _totalAwards; }
            set
            {
                _totalAwards = value;
                RaisePropertyChangedEvent(nameof(TotalAwards));
            }
        }

        public bool IsTotalWinsProcessing
        {
            get {  return _isTotalWinsProcessing; }
            set
            {
                _isTotalWinsProcessing = value;
                RaisePropertyChangedEvent(nameof(IsTotalWinsProcessing));
            }
        }

        public bool IsTotalLossesProcessing
        {
            get { return _isTotalLossesProcessing; }
            set
            {
                _isTotalLossesProcessing = value;
                RaisePropertyChangedEvent(nameof(IsTotalLossesProcessing));
            }
        }

        public bool IsTotalGamesProcessing
        {
            get { return _isTotalGamesProcessing; }
            set
            {
                _isTotalGamesProcessing = value;
                RaisePropertyChangedEvent(nameof(IsTotalGamesProcessing));
            }
        }

        public bool IsTotalWinrateProcessing
        {
            get { return _isTotalWinrateProcessing; }
            set
            {
                _isTotalWinrateProcessing = value;
                RaisePropertyChangedEvent(nameof(IsTotalWinrateProcessing));
            }
        }

        public bool IsTotalKillsProcessing
        {
            get { return _isTotalKillsProcessing; }
            set
            {
                _isTotalKillsProcessing = value;
                RaisePropertyChangedEvent(nameof(IsTotalKillsProcessing));
            }
        }

        public bool IsTotalDeathsProcessing
        {
            get { return _isTotalDeathsProcessing; }
            set
            {
                _isTotalDeathsProcessing = value;
                RaisePropertyChangedEvent(nameof(IsTotalDeathsProcessing));
            }
        }

        public bool IsTotalAwardsProcessing
        {
            get { return _isTotalAwardsProcessing; }
            set
            {
                _isTotalAwardsProcessing = value;
                RaisePropertyChangedEvent(nameof(IsTotalAwardsProcessing));
            }
        }

        public bool IsTotalMVPsProcessing
        {
            get { return _isTotalMVPsProcessing; }
            set
            {
                _isTotalMVPsProcessing = value;
                RaisePropertyChangedEvent(nameof(IsTotalMVPsProcessing));
            }
        }

        public string SelectedAwardMap
        {
            get { return _selectedAwardMap; }
            set
            {
                _selectedAwardMap = value;
                RaisePropertyChangedEvent(nameof(SelectedAwardMap));
            }
        }

        public string SelectedTalentMap
        {
            get { return _selectedTalentMap; }
            set
            {
                _selectedTalentMap = value;
                RaisePropertyChangedEvent(nameof(SelectedTalentMap));
            }
        }

        public string SelectedTalentGameMode
        {
            get { return _selectedTalentGameMode; }
            set
            {
                _selectedTalentGameMode = value;
                RaisePropertyChangedEvent(nameof(SelectedTalentGameMode));
            }
        }

        public ObservableCollection<StatsHeroesTalentPicks> TalentsPickLevel1DataCollection
        {
            get { return _talentsPickLevel1DataCollection; }
            set
            {
                _talentsPickLevel1DataCollection = value;
                RaisePropertyChangedEvent(nameof(TalentsPickLevel1DataCollection));
            }
        }

        public ObservableCollection<StatsHeroesTalentPicks> TalentsPickLevel4DataCollection
        {
            get { return _talentsPickLevel4DataCollection; }
            set
            {
                _talentsPickLevel4DataCollection = value;
                RaisePropertyChangedEvent(nameof(TalentsPickLevel4DataCollection));
            }
        }

        public ObservableCollection<StatsHeroesTalentPicks> TalentsPickLevel7DataCollection
        {
            get { return _talentsPickLevel7DataCollection; }
            set
            {
                _talentsPickLevel7DataCollection = value;
                RaisePropertyChangedEvent(nameof(TalentsPickLevel7DataCollection));
            }
        }

        public ObservableCollection<StatsHeroesTalentPicks> TalentsPickLevel10DataCollection
        {
            get { return _talentsPickLevel10DataCollection; }
            set
            {
                _talentsPickLevel10DataCollection = value;
                RaisePropertyChangedEvent(nameof(TalentsPickLevel10DataCollection));
            }
        }

        public ObservableCollection<StatsHeroesTalentPicks> TalentsPickLevel13DataCollection
        {
            get { return _talentsPickLevel13DataCollection; }
            set
            {
                _talentsPickLevel13DataCollection = value;
                RaisePropertyChangedEvent(nameof(TalentsPickLevel13DataCollection));
            }
        }

        public ObservableCollection<StatsHeroesTalentPicks> TalentsPickLevel16DataCollection
        {
            get { return _talentsPickLevel16DataCollection; }
            set
            {
                _talentsPickLevel16DataCollection = value;
                RaisePropertyChangedEvent(nameof(TalentsPickLevel16DataCollection));
            }
        }

        public ObservableCollection<StatsHeroesTalentPicks> TalentsPickLevel20DataCollection
        {
            get { return _talentsPickLevel20DataCollection; }
            set
            {
                _talentsPickLevel20DataCollection = value;
                RaisePropertyChangedEvent(nameof(TalentsPickLevel20DataCollection));
            }
        }

        public ObservableCollection<StatsHeroesMapMatch> StatsHeroesQuickMatchDataAverageCollection
        {
            get { return _statsHeroesQuickMatchDataAverageCollection; }
            set
            {
                _statsHeroesQuickMatchDataAverageCollection = value;
                RaisePropertyChangedEvent(nameof(TalentsPickLevel20DataCollection));
            }
        }

        public ObservableCollection<StatsHeroesMapMatch> StatsHeroesUnrankedDraftDataAverageCollection
        {
            get { return _statsHeroesUnrankedDraftDataAverageCollection; }
            set
            {
                _statsHeroesUnrankedDraftDataAverageCollection = value;
                RaisePropertyChangedEvent(nameof(StatsHeroesUnrankedDraftDataAverageCollection));
            }
        }

        public ObservableCollection<StatsHeroesMapMatch> StatsHeroesHeroLeagueDataAverageCollection
        {
            get { return _statsHeroesHeroLeagueDataAverageCollection; }
            set
            {
                _statsHeroesHeroLeagueDataAverageCollection = value;
                RaisePropertyChangedEvent(nameof(StatsHeroesHeroLeagueDataAverageCollection));
            }
        }

        public ObservableCollection<StatsHeroesMapMatch> StatsHeroesTeamLeagueDataAverageCollection
        {
            get { return _statsHeroesTeamLeagueDataAverageCollection; }
            set
            {
                _statsHeroesTeamLeagueDataAverageCollection = value;
                RaisePropertyChangedEvent(nameof(StatsHeroesTeamLeagueDataAverageCollection));
            }
        }

        public ObservableCollection<StatsHeroesMapMatch> StatsHeroesCustomGameDataAverageCollection
        {
            get { return _statsHeroesCustomGameDataAverageCollection; }
            set
            {
                _statsHeroesCustomGameDataAverageCollection = value;
                RaisePropertyChangedEvent(nameof(StatsHeroesCustomGameDataAverageCollection));
            }
        }

        public ObservableCollection<StatsHeroesMapMatch> StatsHeroesQuickMatchDataAverageTotalCollection
        {
            get { return _statsHeroesQuickMatchDataAverageTotalCollection; }
            set
            {
                _statsHeroesQuickMatchDataAverageTotalCollection = value;
                RaisePropertyChangedEvent(nameof(StatsHeroesQuickMatchDataAverageTotalCollection));
            }
        }

        public ObservableCollection<StatsHeroesMapMatch> StatsHeroesUnrankedDraftDataAverageTotalCollection
        {
            get { return _statsHeroesUnrankedDraftDataAverageTotalCollection; }
            set
            {
                _statsHeroesUnrankedDraftDataAverageTotalCollection = value;
                RaisePropertyChangedEvent(nameof(StatsHeroesUnrankedDraftDataAverageTotalCollection));
            }
        }

        public ObservableCollection<StatsHeroesMapMatch> StatsHeroesHeroLeagueDataAverageTotalCollection
        {
            get { return _statsHeroesHeroLeagueDataAverageTotalCollection; }
            set
            {
                _statsHeroesHeroLeagueDataAverageTotalCollection = value;
                RaisePropertyChangedEvent(nameof(StatsHeroesHeroLeagueDataAverageTotalCollection));
            }
        }

        public ObservableCollection<StatsHeroesMapMatch> StatsHeroesTeamLeagueDataAverageTotalCollection
        {
            get { return _statsHeroesTeamLeagueDataAverageTotalCollection; }
            set
            {
                _statsHeroesTeamLeagueDataAverageTotalCollection = value;
                RaisePropertyChangedEvent(nameof(StatsHeroesTeamLeagueDataAverageTotalCollection));
            }
        }

        public ObservableCollection<StatsHeroesMapMatch> StatsHeroesCustomGameDataAverageTotalCollection
        {
            get { return _statsHeroesCustomGameDataAverageTotalCollection; }
            set
            {
                _statsHeroesCustomGameDataAverageTotalCollection = value;
                RaisePropertyChangedEvent(nameof(StatsHeroesCustomGameDataAverageTotalCollection));
            }
        }

        public ICommand RefreshAwardsCommand
        {
            get { return new DelegateCommand(async () => await PerformRefreshAwardsOnlyCommand()); }
        }

        public ICommand RefreshTalentsCommand
        {
            get { return new DelegateCommand(async () => await PerformRefreshTalentsOnlyCommand()); }
        }

        #endregion public properties

        /// <summary>
        /// Constructor
        /// </summary>
        public HeroStatsHeroesViewModel()
            :base()
        {
            HeroesList = HeroesInfo.GetListOfHeroes();

            SelectedAwardMap = MapList[0];
            SelectedTalentMap = MapList[0];
            SelectedTalentGameMode = GameModeList[0];
        }

        protected override async Task ReceiveMessage(StatisticsTabMessage action)
        {
            if (action.StatisticsTab == StatisticsTab.Heroes && !string.IsNullOrEmpty(SelectedHero))
            {
                await PerformCommand();
            }
        }

        protected override async Task RefreshStats()
        {
            if (string.IsNullOrWhiteSpace(SelectedHero))
                return;

            try
            {
                await Application.Current.Dispatcher.InvokeAsync(delegate
                {
                    CharacterDraftPortrait = HeroesInfo.GetHeroPortrait(SelectedHero);
                    CharacterName = SelectedHero;
                    CharacterRole = HeroesInfo.GetHeroRole(SelectedHero).ToString();
                    CharacterLevel = QueryHeroLevel(SelectedHero);

                    ClearStats();
                });

                var maps = HeroesInfo.GetMapsListExceptCustomOnly();
                var customGameModeMaps = HeroesInfo.GetMapsList();

                List<Task> list = new List<Task>();
                list.Add(SetMapMatchStats(maps, GameMode.QuickMatch, StatsHeroesQuickMatchDataCollection, StatsHeroesQuickMatchDataTotalCollection, StatsHeroesQuickMatchDataAverageCollection, StatsHeroesQuickMatchDataAverageTotalCollection));
                list.Add(SetMapMatchStats(maps, GameMode.UnrankedDraft, StatsHeroesUnrankedDraftDataCollection, StatsHeroesUnrankedDraftDataTotalCollection, StatsHeroesUnrankedDraftDataAverageCollection, StatsHeroesUnrankedDraftDataAverageTotalCollection));
                list.Add(SetMapMatchStats(maps, GameMode.HeroLeague, StatsHeroesHeroLeagueDataCollection, StatsHeroesHeroLeagueDataTotalCollection, StatsHeroesHeroLeagueDataAverageCollection, StatsHeroesHeroLeagueDataAverageTotalCollection));
                list.Add(SetMapMatchStats(maps, GameMode.TeamLeague, StatsHeroesTeamLeagueDataCollection, StatsHeroesTeamLeagueDataTotalCollection, StatsHeroesTeamLeagueDataAverageCollection, StatsHeroesTeamLeagueDataAverageTotalCollection));
                list.Add(SetMapMatchStats(customGameModeMaps, GameMode.Custom, StatsHeroesCustomGameDataCollection, StatsHeroesCustomGameDataTotalCollection, StatsHeroesCustomGameDataAverageCollection, StatsHeroesCustomGameDataAverageTotalCollection));
                list.Add(SetMatchAwards());
                list.Add(SetAllTalentPicks());

                await Task.WhenAll(list.ToArray());

                TotalWins = StatsHeroesQuickMatchDataTotalCollection[0].Wins + StatsHeroesUnrankedDraftDataTotalCollection[0].Wins +
                            StatsHeroesHeroLeagueDataTotalCollection[0].Wins + StatsHeroesTeamLeagueDataTotalCollection[0].Wins;
                TotalLosses = StatsHeroesQuickMatchDataTotalCollection[0].Losses + StatsHeroesUnrankedDraftDataTotalCollection[0].Losses +
                              StatsHeroesHeroLeagueDataTotalCollection[0].Losses + StatsHeroesTeamLeagueDataTotalCollection[0].Losses;
                TotalGames = StatsHeroesQuickMatchDataTotalCollection[0].TotalGames + StatsHeroesUnrankedDraftDataTotalCollection[0].TotalGames +
                             StatsHeroesHeroLeagueDataTotalCollection[0].TotalGames + StatsHeroesTeamLeagueDataTotalCollection[0].TotalGames;
                TotalKills = StatsHeroesQuickMatchDataTotalCollection[0].Kills + StatsHeroesUnrankedDraftDataTotalCollection[0].Kills +
                             StatsHeroesHeroLeagueDataTotalCollection[0].Kills + StatsHeroesTeamLeagueDataTotalCollection[0].Kills;
                TotalDeaths = StatsHeroesQuickMatchDataTotalCollection[0].Deaths + StatsHeroesUnrankedDraftDataTotalCollection[0].Deaths +
                              StatsHeroesHeroLeagueDataTotalCollection[0].Deaths + StatsHeroesTeamLeagueDataTotalCollection[0].Deaths;

                TotalWinrate = $"{Utilities.CalculateWinPercentage((int)TotalWins, (double)TotalGames)}%";

                TotalAwards = MatchAwardDataTotalCollection[0].QuickMatch + MatchAwardDataTotalCollection[0].UnrankedDraft +
                              MatchAwardDataTotalCollection[0].HeroLeague + MatchAwardDataTotalCollection[0].TeamLeague;
                TotalMVPs = TotalMVPCount;

                ProgressRingsActive(false);
            }
            catch
            {
                throw;
            }
        }

        private async Task SetMapMatchStats(List<string> maps, GameMode gameMode, ObservableCollection<StatsHeroesMapMatch> collection, ObservableCollection<StatsHeroesMapMatch> totalCollection, 
            ObservableCollection<StatsHeroesMapMatch> averageCollection, ObservableCollection<StatsHeroesMapMatch> averageTotalCollection)
        {
            foreach (var map in maps)
            {
                int wins = Query.HeroStatsGameMode.GetWinsOrLossesForHero(SelectedHero, GetSeasonSelected, gameMode, map, true);
                int losses = Query.HeroStatsGameMode.GetWinsOrLossesForHero(SelectedHero, GetSeasonSelected, gameMode, map, false);
                int total = wins + losses;
                int winPercentage = Utilities.CalculateWinPercentage(wins, total);

                var scoreResultsList = Query.PlayerStatistics.ReadScoreResult(GetSeasonSelected, gameMode, map, SelectedHero);

                int kills = (int)scoreResultsList.Sum(x => x.SoloKills);
                int assists = (int)scoreResultsList.Sum(x => x.Assists);
                int deaths = (int)scoreResultsList.Sum(x => x.Deaths);

                double siegeDamage = (double)scoreResultsList.Sum(x => x.SiegeDamage);
                double heroDamage = (double)scoreResultsList.Sum(x => x.HeroDamage);
                double experience = (double)scoreResultsList.Sum(x => x.ExperienceContribution);
                int mercsCaptured = (int)scoreResultsList.Sum(x => x.MercCampCaptures);

                TimeSpan gameTime = Query.PlayerStatistics.ReadMapGameTime(SelectedHero, GetSeasonSelected, gameMode, map);

                double role = 0;
                if (HeroesInfo.GetHeroRole(SelectedHero) == HeroRole.Warrior)
                    role = (double)scoreResultsList.Sum(x => x.DamageTaken);
                else if (HeroesInfo.GetHeroRole(SelectedHero) == HeroRole.Support || HeroesInfo.IsNonSupportHeroWithHealingStat(SelectedHero))
                    role = (double)scoreResultsList.Sum(x => x.Healing);

                var mapImage = HeroesInfo.GetMapBackground(map, true);
                mapImage.Freeze();

                StatsHeroesMapMatch mapMatch = new StatsHeroesMapMatch
                {
                    MapName = map,
                    Wins = wins,
                    Losses = losses,
                    TotalGames = total,
                    WinPercentage = total != 0 ? winPercentage : (int?)null,
                    Kills = kills,
                    Assists = assists,
                    Deaths = deaths,
                    SiegeDamage = siegeDamage,
                    HeroDamage = heroDamage,
                    Role = role,
                    Experience = experience,
                    MercsCaptured = mercsCaptured,
                    GameTime = gameTime
                };

                await Application.Current.Dispatcher.InvokeAsync(delegate
                {
                    mapMatch.MapImage = mapImage;
                    collection.Add(mapMatch);
                });
            }

            //===============================
            // get totals
            //===============================
            int totalWins = collection.Sum(x => x.Wins);
            int totalLosses = collection.Sum(x => x.Losses);
            int totalTotal = collection.Sum(x => x.TotalGames);
            int totalWinPercentage = Utilities.CalculateWinPercentage(totalWins, totalTotal);

            int totalKills = collection.Sum(x => x.Kills);
            int totalAssists = collection.Sum(x => x.Assists);
            int totalDeaths = collection.Sum(x => x.Deaths);

            double totalSiegeDamage = collection.Sum(x => x.SiegeDamage);
            double totalHeroDamage = collection.Sum(x => x.HeroDamage);
            double totalrole = collection.Sum(x => x.Role);
            double totalExperience = collection.Sum(x => x.Experience);
            int totalMercsCaptured = collection.Sum(x => x.MercsCaptured);
            TimeSpan totalGameTime = TimeSpan.FromSeconds(collection.Sum(x => x.GameTime.TotalSeconds));

            StatsHeroesMapMatch totalMatch = new StatsHeroesMapMatch
            {
                MapName = "Total",
                Wins = totalWins,
                Losses = totalLosses,
                TotalGames = totalTotal,
                WinPercentage = totalTotal != 0 ? totalWinPercentage : (int?)null,
                Kills = totalKills,
                Assists = totalAssists,
                Deaths = totalDeaths,
                SiegeDamage = totalSiegeDamage,
                HeroDamage = totalHeroDamage,
                Role = totalrole,
                Experience = totalExperience,
                MercsCaptured = totalMercsCaptured,
                GameTime = totalGameTime
            };

            await Application.Current.Dispatcher.InvokeAsync(delegate
            {
                totalCollection.Add(totalMatch);
            });

            //===============================
            // get averages
            //===============================
            foreach (var map in collection)
            {
                int totalGames = map.TotalGames > 0? map.TotalGames : 1;

                int killsAverage = map.Kills / totalGames;
                int assistsAverage = map.Assists / totalGames;
                int deathsAverage = map.Deaths / totalGames;
                double siegeDamageAverage = map.SiegeDamage / totalGames;
                double heroDamageAverage = map.HeroDamage / totalGames;
                double roleAverage = map.Role / totalGames;
                double experienceAverage = map.Experience / totalGames;
                int mercsCapturedAverage = map.MercsCaptured / totalGames;
                TimeSpan gameTimeAverage = TimeSpan.FromSeconds(Math.Round(map.GameTime.TotalSeconds / totalGames, 0));

                var mapImage = map.MapImage;
                mapImage.Freeze();

                StatsHeroesMapMatch averageMatch = new StatsHeroesMapMatch
                {
                    MapName = map.MapName,
                    TotalGames = map.TotalGames,
                    Kills = killsAverage,
                    Assists = assistsAverage,
                    Deaths = deathsAverage,
                    SiegeDamage = siegeDamageAverage,
                    HeroDamage = heroDamageAverage,
                    Role = roleAverage,
                    Experience = experienceAverage,
                    MercsCaptured = mercsCapturedAverage,
                    GameTime = gameTimeAverage
                };

                await Application.Current.Dispatcher.InvokeAsync(delegate
                {
                    averageMatch.MapImage = mapImage;
                    averageCollection.Add(averageMatch);
                });
            }

            //===============================
            // get total average
            //===============================
            int totalAverageTotal = totalTotal > 0 ? totalTotal : 1;

            StatsHeroesMapMatch totalAverageMatch = new StatsHeroesMapMatch
            {
                MapName = "Total Average",
                TotalGames = totalTotal,
                Kills = totalKills / totalAverageTotal,
                Assists = totalAssists / totalAverageTotal,
                Deaths = totalDeaths / totalAverageTotal,
                SiegeDamage = totalSiegeDamage / totalAverageTotal,
                HeroDamage = totalHeroDamage / totalAverageTotal,
                Role = totalrole / totalAverageTotal,
                Experience = totalExperience / totalAverageTotal,
                MercsCaptured = totalMercsCaptured / totalAverageTotal,
                GameTime = TimeSpan.FromSeconds(Math.Round(totalGameTime.TotalSeconds / totalAverageTotal, 0))
            };

            await Application.Current.Dispatcher.InvokeAsync(delegate
            {
                averageTotalCollection.Add(totalAverageMatch);
            });
        }

        private async Task SetMatchAwards()
        {
            foreach (var award in HeroesInfo.GetMatchAwardsList())
            {
                int quickmatchAwards = Query.PlayerStatistics.ReadMatchAwards(award, GetSeasonSelected, GameMode.QuickMatch, SelectedHero, SelectedAwardMap);
                int unrankedDraftAwards = Query.PlayerStatistics.ReadMatchAwards(award, GetSeasonSelected, GameMode.UnrankedDraft, SelectedHero, SelectedAwardMap);
                int heroLeagueAwards = Query.PlayerStatistics.ReadMatchAwards(award, GetSeasonSelected, GameMode.HeroLeague, SelectedHero, SelectedAwardMap);
                int teamLeagueAwards = Query.PlayerStatistics.ReadMatchAwards(award, GetSeasonSelected, GameMode.TeamLeague, SelectedHero, SelectedAwardMap);

                int rowTotal = quickmatchAwards + unrankedDraftAwards + heroLeagueAwards + teamLeagueAwards;

                if (award == "MVP")
                    TotalMVPCount = quickmatchAwards + unrankedDraftAwards + heroLeagueAwards + teamLeagueAwards;

                string awardName;
                var awardImage = HeroesInfo.GetMVPScoreScreenAward(award.ToString(), MVPScoreScreenColor.Blue, out awardName);
                awardImage.Freeze();

                StatsHeroesMatchAwards matchAwards = new StatsHeroesMatchAwards
                {
                    AwardName = awardName,
                    AwardDescription = HeroesInfo.GetMatchAwardDescription(award.ToString()),
                    QuickMatch = quickmatchAwards,
                    UnrankedDraft = unrankedDraftAwards,
                    HeroLeague = heroLeagueAwards,
                    TeamLeague = teamLeagueAwards,
                    Total = rowTotal,
                };

                await Application.Current.Dispatcher.InvokeAsync(delegate
                {
                    matchAwards.AwardImage = awardImage;
                    MatchAwardDataCollection.Add(matchAwards);
                });
            }

            // get totals
            int totalQuickMatch = MatchAwardDataCollection.Sum(x => x.QuickMatch);
            int totalUnrankedDraft = MatchAwardDataCollection.Sum(x => x.UnrankedDraft);
            int totalHeroLeague = MatchAwardDataCollection.Sum(x => x.HeroLeague);
            int totalTeamLeague = MatchAwardDataCollection.Sum(x => x.TeamLeague);
            int totalTotal = MatchAwardDataCollection.Sum(x => x.Total);

            StatsHeroesMatchAwards totalAwards = new StatsHeroesMatchAwards
            {
                AwardName = "Total",
                QuickMatch = totalQuickMatch,
                UnrankedDraft = totalUnrankedDraft,
                HeroLeague = totalHeroLeague,
                TeamLeague = totalTeamLeague, 
                Total = totalTotal,
            };

            await Application.Current.Dispatcher.InvokeAsync(delegate
            {
                MatchAwardDataTotalCollection.Add(totalAwards);
            });
        }

        private async Task SetAllTalentPicks()
        {
            if (string.IsNullOrEmpty(SelectedHero))
                return;

            var allTalentTierForHero = HeroesInfo.GetTalentsForHero(SelectedHero);

            GameMode gameMode = Utilities.GetGameModeFromString(SelectedTalentGameMode);
            await SetTalentPicks(allTalentTierForHero, TalentTier.Level1, gameMode, TalentsPickLevel1DataCollection);
            await SetTalentPicks(allTalentTierForHero, TalentTier.Level4, gameMode, TalentsPickLevel4DataCollection);
            await SetTalentPicks(allTalentTierForHero, TalentTier.Level7, gameMode, TalentsPickLevel7DataCollection);
            await SetTalentPicks(allTalentTierForHero, TalentTier.Level10, gameMode, TalentsPickLevel10DataCollection);
            await SetTalentPicks(allTalentTierForHero, TalentTier.Level13, gameMode, TalentsPickLevel13DataCollection);
            await SetTalentPicks(allTalentTierForHero, TalentTier.Level16, gameMode, TalentsPickLevel16DataCollection);
            await SetTalentPicks(allTalentTierForHero, TalentTier.Level20, gameMode, TalentsPickLevel20DataCollection);
        }

        private async Task SetTalentPicks(Dictionary<TalentTier, List<string>> allTalentTiers, TalentTier tier, GameMode gameMode, ObservableCollection<StatsHeroesTalentPicks> collection)
        {
            var tierTalents = allTalentTiers[tier];

            foreach (var talent in tierTalents)
            {
                var talentImage = HeroesInfo.GetTalentIcon(talent);
                talentImage.Freeze();

                int talentWin = Query.PlayerStatistics.ReadCharacterTalentsIsWinner(talent, tier, GetSeasonSelected, gameMode, SelectedHero, SelectedTalentMap, true);
                int talentLoss = Query.PlayerStatistics.ReadCharacterTalentsIsWinner(talent, tier, GetSeasonSelected, gameMode, SelectedHero, SelectedTalentMap, false);
                int talentTotal = talentWin + talentLoss;
                int talentWinPercentage = Utilities.CalculateWinPercentage(talentWin, talentTotal);
                TalentDescription talentDescription = HeroesInfo.GetTalentDescriptions(talent);

                StatsHeroesTalentPicks talentPicks = new StatsHeroesTalentPicks
                {
                    TalentName = HeroesInfo.GetTrueTalentName(talent),
                    TalentShortDescription = talentDescription.Short,
                    TalentFullDescription = talentDescription.Full,
                    Wins = talentWin,
                    Losses = talentLoss,
                    Total = talentTotal,
                    Winrate = talentWinPercentage,
                };

                await Application.Current.Dispatcher.InvokeAsync(delegate
                {
                    talentPicks.TalentImage = talentImage;
                    collection.Add(talentPicks);
                });
            }
        }

        private async Task PerformRefreshAwardsOnlyCommand()
        {
            IsComboBoxEnabled = false;
            await Task.Run(async () =>
            {
                foreach (var award in MatchAwardDataCollection)
                    award.AwardImage = null;

                await Application.Current.Dispatcher.InvokeAsync(delegate
                {
                    MatchAwardDataCollection.Clear();
                    MatchAwardDataTotalCollection.Clear();
                });

                await SetMatchAwards();
            });
            IsComboBoxEnabled = true;
        }

        private async Task PerformRefreshTalentsOnlyCommand()
        {
            IsComboBoxEnabled = false;
            await Task.Run(async () =>
            {
                foreach (var talent in TalentsPickLevel1DataCollection)
                    talent.TalentImage = null;
                foreach (var talent in TalentsPickLevel4DataCollection)
                    talent.TalentImage = null;
                foreach (var talent in TalentsPickLevel7DataCollection)
                    talent.TalentImage = null;
                foreach (var talent in TalentsPickLevel10DataCollection)
                    talent.TalentImage = null;
                foreach (var talent in TalentsPickLevel13DataCollection)
                    talent.TalentImage = null;
                foreach (var talent in TalentsPickLevel16DataCollection)
                    talent.TalentImage = null;
                foreach (var talent in TalentsPickLevel20DataCollection)
                    talent.TalentImage = null;

                await Application.Current.Dispatcher.InvokeAsync(delegate
                {
                    TalentsPickLevel1DataCollection.Clear();
                    TalentsPickLevel4DataCollection.Clear();
                    TalentsPickLevel7DataCollection.Clear();
                    TalentsPickLevel10DataCollection.Clear();
                    TalentsPickLevel13DataCollection.Clear();
                    TalentsPickLevel16DataCollection.Clear();
                    TalentsPickLevel20DataCollection.Clear();
                });

                await SetAllTalentPicks();
            });
            IsComboBoxEnabled = true;
        }

        private void ClearStats()
        {
            TotalMVPCount = null;

            TotalWins = null;
            TotalLosses = null;
            TotalGames = null;
            TotalKills = null;
            TotalDeaths = null;
            TotalWinrate = string.Empty;
            TotalAwards = null;
            TotalMVPs = null;

            ProgressRingsActive(true);

            foreach (var map in StatsHeroesQuickMatchDataCollection)
                map.MapImage = null;
            foreach (var map in StatsHeroesUnrankedDraftDataCollection)
                map.MapImage = null;
            foreach (var map in StatsHeroesHeroLeagueDataCollection)
                map.MapImage = null;
            foreach (var map in StatsHeroesTeamLeagueDataCollection)
                map.MapImage = null;
            foreach (var map in StatsHeroesCustomGameDataCollection)
                map.MapImage = null;

            foreach (var map in StatsHeroesQuickMatchDataAverageCollection)
                map.MapImage = null;
            foreach (var map in StatsHeroesUnrankedDraftDataAverageCollection)
                map.MapImage = null;
            foreach (var map in StatsHeroesHeroLeagueDataAverageCollection)
                map.MapImage = null;
            foreach (var map in StatsHeroesTeamLeagueDataAverageCollection)
                map.MapImage = null;
            foreach (var map in StatsHeroesCustomGameDataAverageCollection)
                map.MapImage = null;

            foreach (var award in MatchAwardDataCollection)
                award.AwardImage = null;
            foreach (var talent in TalentsPickLevel1DataCollection)
                talent.TalentImage = null;
            foreach (var talent in TalentsPickLevel4DataCollection)
                talent.TalentImage = null;
            foreach (var talent in TalentsPickLevel7DataCollection)
                talent.TalentImage = null;
            foreach (var talent in TalentsPickLevel10DataCollection)
                talent.TalentImage = null;
            foreach (var talent in TalentsPickLevel13DataCollection)
                talent.TalentImage = null;
            foreach (var talent in TalentsPickLevel16DataCollection)
                talent.TalentImage = null;
            foreach (var talent in TalentsPickLevel20DataCollection)
                talent.TalentImage = null;

            StatsHeroesQuickMatchDataCollection.Clear();
            StatsHeroesUnrankedDraftDataCollection.Clear();
            StatsHeroesHeroLeagueDataCollection.Clear();
            StatsHeroesTeamLeagueDataCollection.Clear();
            StatsHeroesCustomGameDataCollection.Clear();

            StatsHeroesQuickMatchDataTotalCollection.Clear();
            StatsHeroesUnrankedDraftDataTotalCollection.Clear();
            StatsHeroesHeroLeagueDataTotalCollection.Clear();
            StatsHeroesTeamLeagueDataTotalCollection.Clear();
            StatsHeroesCustomGameDataTotalCollection.Clear();

            StatsHeroesQuickMatchDataAverageCollection.Clear();
            StatsHeroesUnrankedDraftDataAverageCollection.Clear();
            StatsHeroesHeroLeagueDataAverageCollection.Clear();
            StatsHeroesTeamLeagueDataAverageCollection.Clear();
            StatsHeroesCustomGameDataAverageCollection.Clear();

            StatsHeroesQuickMatchDataAverageTotalCollection.Clear();
            StatsHeroesUnrankedDraftDataAverageTotalCollection.Clear();
            StatsHeroesHeroLeagueDataAverageTotalCollection.Clear();
            StatsHeroesTeamLeagueDataAverageTotalCollection.Clear();
            StatsHeroesCustomGameDataAverageTotalCollection.Clear();

            MatchAwardDataCollection.Clear();
            MatchAwardDataTotalCollection.Clear();

            TalentsPickLevel1DataCollection.Clear();
            TalentsPickLevel4DataCollection.Clear();
            TalentsPickLevel7DataCollection.Clear();
            TalentsPickLevel10DataCollection.Clear();
            TalentsPickLevel13DataCollection.Clear();
            TalentsPickLevel16DataCollection.Clear();
            TalentsPickLevel20DataCollection.Clear();
        }

        private void ProgressRingsActive(bool isActive)
        {
            IsTotalWinsProcessing = isActive;
            IsTotalLossesProcessing = isActive;
            IsTotalGamesProcessing = isActive;
            IsTotalWinrateProcessing = isActive;
            IsTotalKillsProcessing = isActive;
            IsTotalDeathsProcessing = isActive;
            IsTotalAwardsProcessing = isActive;
            IsTotalMVPsProcessing = isActive;
        }
    }
}
