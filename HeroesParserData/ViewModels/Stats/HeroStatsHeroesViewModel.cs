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
        private string _quickMatchTitle;
        private string _unrankedDraftTitle;
        private string _heroLeagueTitle;
        private string _teamLeagueTitle;
        private string _customGameTitle;
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
        public string QuickMatchTitle
        {
            get { return _quickMatchTitle; }
            set
            {
                _quickMatchTitle = value;
                RaisePropertyChangedEvent(nameof(QuickMatchTitle));
            }
        }

        public string UnrankedDraftTitle
        {
            get { return _unrankedDraftTitle; }
            set
            {
                _unrankedDraftTitle = value;
                RaisePropertyChangedEvent(nameof(UnrankedDraftTitle));
            }
        }

        public string HeroLeagueTitle
        {
            get { return _heroLeagueTitle; }
            set
            {
                _heroLeagueTitle = value;
                RaisePropertyChangedEvent(nameof(HeroLeagueTitle));
            }
        }

        public string TeamLeagueTitle
        {
            get { return _teamLeagueTitle; }
            set
            {
                _teamLeagueTitle = value;
                RaisePropertyChangedEvent(nameof(TeamLeagueTitle));
            }
        }

        public string CustomGameTitle
        {
            get { return _customGameTitle; }
            set
            {
                _customGameTitle = value;
                RaisePropertyChangedEvent(nameof(CustomGameTitle));
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
            QuickMatchTitle = "Quick Match";
            UnrankedDraftTitle = "Unranked Draft";
            HeroLeagueTitle = "Hero League";
            TeamLeagueTitle = "Team League";
            CustomGameTitle = "Custom Game";

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
                list.Add(SetMapMatchStats(maps, GameMode.QuickMatch, StatsHeroesQuickMatchDataCollection, StatsHeroesQuickMatchDataTotalCollection));
                list.Add(SetMapMatchStats(maps, GameMode.UnrankedDraft, StatsHeroesUnrankedDraftDataCollection, StatsHeroesUnrankedDraftDataTotalCollection));
                list.Add(SetMapMatchStats(maps, GameMode.HeroLeague, StatsHeroesHeroLeagueDataCollection, StatsHeroesHeroLeagueDataTotalCollection));
                list.Add(SetMapMatchStats(maps, GameMode.TeamLeague, StatsHeroesTeamLeagueDataCollection, StatsHeroesTeamLeagueDataTotalCollection));
                list.Add(SetMapMatchStats(customGameModeMaps, GameMode.Custom, StatsHeroesCustomGameDataCollection, StatsHeroesCustomGameDataTotalCollection));
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

        private async Task SetMapMatchStats(List<string> maps, GameMode gameMode, ObservableCollection<StatsHeroesMapMatch> collection, ObservableCollection<StatsHeroesMapMatch> totalCollection)
        {
            foreach (var map in maps)
            {
                int wins = Query.HeroStatsGameMode.GetWinsOrLossesForHero(SelectedHero, GetSeasonSelected, gameMode, map, true);
                int losses = Query.HeroStatsGameMode.GetWinsOrLossesForHero(SelectedHero, GetSeasonSelected, gameMode, map, false);
                int total = wins + losses;
                int winPercentage = Utilities.CalculateWinPercentage(wins, total);

                int kills = Query.PlayerStatistics.ReadTotalStatTypeForMapCharacter(StatType.kills, GetSeasonSelected, gameMode, map, SelectedHero);
                int assists = Query.PlayerStatistics.ReadTotalStatTypeForMapCharacter(StatType.assists, GetSeasonSelected, gameMode, map, SelectedHero);
                int deaths = Query.PlayerStatistics.ReadTotalStatTypeForMapCharacter(StatType.deaths, GetSeasonSelected, gameMode, map, SelectedHero);

                double siegeDamage = Query.PlayerStatistics.ReadTotalScoreResult(PlayerScoreResultTypes.SiegeDamage, GetSeasonSelected, gameMode, map, SelectedHero);
                double heroDamage = Query.PlayerStatistics.ReadTotalScoreResult(PlayerScoreResultTypes.HeroDamage, GetSeasonSelected, gameMode, map, SelectedHero);
                double experience = Query.PlayerStatistics.ReadTotalScoreResult(PlayerScoreResultTypes.ExperienceContribution, GetSeasonSelected, gameMode, map, SelectedHero);
                int mercsCaptured = (int)Query.PlayerStatistics.ReadTotalScoreResult(PlayerScoreResultTypes.MercCampCaptures, GetSeasonSelected, gameMode, map, SelectedHero);

                double role = 0;
                if (HeroesInfo.GetHeroRole(SelectedHero) == HeroRole.Warrior)
                    role = Query.PlayerStatistics.ReadTotalScoreResult(PlayerScoreResultTypes.DamageTaken, GetSeasonSelected, gameMode, map, SelectedHero);
                else if (HeroesInfo.GetHeroRole(SelectedHero) == HeroRole.Support || HeroesInfo.IsNonSupportHeroWithHealingStat(SelectedHero))
                    role = Query.PlayerStatistics.ReadTotalScoreResult(PlayerScoreResultTypes.Healing, GetSeasonSelected, gameMode, map, SelectedHero);

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
                };

                await Application.Current.Dispatcher.InvokeAsync(delegate
                {
                    mapMatch.MapImage = mapImage;
                    collection.Add(mapMatch);
                });
            }

            // get totals
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
            };

            await Application.Current.Dispatcher.InvokeAsync(delegate
            {
                totalCollection.Add(totalMatch);
            });

            string gameModeStats = $"     {totalWins} W / {totalLosses} L / {totalTotal} T / {totalWinPercentage}%";
            switch (gameMode)
            {
                case GameMode.QuickMatch:
                    QuickMatchTitle = $"Quick Match {gameModeStats}";
                    break;
                case GameMode.UnrankedDraft:
                    UnrankedDraftTitle = $"Unranked Draft {gameModeStats}";
                    break;
                case GameMode.HeroLeague:
                    HeroLeagueTitle = $"Hero League {gameModeStats}";
                    break;
                case GameMode.TeamLeague:
                    TeamLeagueTitle = $"Team League {gameModeStats}";
                    break;
                case GameMode.Custom:
                    CustomGameTitle = $"Custom Game {gameModeStats}";
                    break;
            }
        }

        private async Task SetMatchAwards()
        {
            foreach (var award in HeroesInfo.GetMatchAwardsList())
            {
                int quickmatchAwards = Query.PlayerStatistics.ReadTotalMatchAwards(award, GetSeasonSelected, GameMode.QuickMatch, SelectedHero, SelectedAwardMap);
                int unrankedDraftAwards = Query.PlayerStatistics.ReadTotalMatchAwards(award, GetSeasonSelected, GameMode.UnrankedDraft, SelectedHero, SelectedAwardMap);
                int heroLeagueAwards = Query.PlayerStatistics.ReadTotalMatchAwards(award, GetSeasonSelected, GameMode.HeroLeague, SelectedHero, SelectedAwardMap);
                int teamLeagueAwards = Query.PlayerStatistics.ReadTotalMatchAwards(award, GetSeasonSelected, GameMode.TeamLeague, SelectedHero, SelectedAwardMap);
                int rowTotal = quickmatchAwards + unrankedDraftAwards + heroLeagueAwards + teamLeagueAwards;

                if (award == "MVP")
                    TotalMVPCount = quickmatchAwards + unrankedDraftAwards + heroLeagueAwards + teamLeagueAwards;

                string awardName;
                var awardImage = HeroesInfo.GetMVPScoreScreenAward(award.ToString(), MVPScoreScreenColor.Blue, out awardName);
                awardImage.Freeze();

                StatsHeroesMatchAwards matchAwards = new StatsHeroesMatchAwards
                {
                    AwardName = awardName,
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

                StatsHeroesTalentPicks talentPicks = new StatsHeroesTalentPicks
                {
                    TalentName = HeroesInfo.GetTrueTalentName(talent),
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

            QuickMatchTitle = "Quick Match";
            UnrankedDraftTitle = "Unranked Draft";
            HeroLeagueTitle = "Hero League";
            TeamLeagueTitle = "Team League";
            CustomGameTitle = "Custom Game";

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
