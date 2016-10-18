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

        private ObservableCollection<StatsHeroesMapMatch> _statsHeroesQuickMatchDataTotalCollection = new ObservableCollection<StatsHeroesMapMatch>();
        private ObservableCollection<StatsHeroesMapMatch> _statsHeroesUnrankedDraftDataTotalCollection = new ObservableCollection<StatsHeroesMapMatch>();
        private ObservableCollection<StatsHeroesMapMatch> _statsHeroesHeroLeagueDataTotalCollection = new ObservableCollection<StatsHeroesMapMatch>();
        private ObservableCollection<StatsHeroesMapMatch> _statsHeroesTeamLeagueDataTotalCollection = new ObservableCollection<StatsHeroesMapMatch>();

        private ObservableCollection<StatsHeroesMatchAwards> _matchAwardDataCollection = new ObservableCollection<StatsHeroesMatchAwards>();
        private ObservableCollection<StatsHeroesMatchAwards> _matchAwardDataTotalCollection = new ObservableCollection<StatsHeroesMatchAwards>();
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
        #endregion public properties

        public HeroStatsHeroesViewModel()
            :base()
        {
            HeroesList = HeroesInfo.GetListOfHeroes();
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

                var maps = Maps.GetMaps();

                List<Task> list = new List<Task>();
                list.Add(SetMapMatchStats(maps, GameMode.QuickMatch, StatsHeroesQuickMatchDataCollection, StatsHeroesQuickMatchDataTotalCollection));
                list.Add(SetMapMatchStats(maps, GameMode.UnrankedDraft, StatsHeroesUnrankedDraftDataCollection, StatsHeroesUnrankedDraftDataTotalCollection));
                list.Add(SetMapMatchStats(maps, GameMode.HeroLeague, StatsHeroesHeroLeagueDataCollection, StatsHeroesHeroLeagueDataTotalCollection));
                list.Add(SetMapMatchStats(maps, GameMode.TeamLeague, StatsHeroesTeamLeagueDataCollection, StatsHeroesTeamLeagueDataTotalCollection));
                list.Add(SetMapMatchAwards());

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
                if (HeroesInfo.GetHeroRole(SelectedHero) == HeroesIcons.HeroRole.Warrior)
                    role = Query.PlayerStatistics.ReadTotalScoreResult(PlayerScoreResultTypes.DamageTaken, GetSeasonSelected, gameMode, map, SelectedHero);
                else if (HeroesInfo.GetHeroRole(SelectedHero) == HeroesIcons.HeroRole.Support || SelectedHero == "Medivh")
                    role = Query.PlayerStatistics.ReadTotalScoreResult(PlayerScoreResultTypes.Healing, GetSeasonSelected, gameMode, map, SelectedHero);

                var mapImage = HeroesInfo.GetMapBackground(Maps.GetEnumMapName(map), true);
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
        }

        private async Task SetMapMatchAwards()
        {
            foreach (MVPAwardType award in Enum.GetValues(typeof(MVPAwardType)))
            {
                if (award == MVPAwardType.Unknown)
                    continue;

                int quickmatchAwards = Query.PlayerStatistics.ReadTotalMatchAwards(award, GetSeasonSelected, GameMode.QuickMatch, SelectedHero);
                int unrankedDraftAwards = Query.PlayerStatistics.ReadTotalMatchAwards(award, GetSeasonSelected, GameMode.UnrankedDraft, SelectedHero);
                int heroLeagueAwards = Query.PlayerStatistics.ReadTotalMatchAwards(award, GetSeasonSelected, GameMode.HeroLeague, SelectedHero);
                int teamLeagueAwards = Query.PlayerStatistics.ReadTotalMatchAwards(award, GetSeasonSelected, GameMode.TeamLeague, SelectedHero);

                if (award == MVPAwardType.MVP)
                    TotalMVPCount = quickmatchAwards + unrankedDraftAwards + heroLeagueAwards + teamLeagueAwards;

                string awardName;
                var awardImage = HeroesInfo.GetMVPScoreScreenAward(award, MVPScoreScreenColor.Blue, out awardName);
                awardImage.Freeze();

                StatsHeroesMatchAwards matchAwards = new StatsHeroesMatchAwards
                {
                    AwardName = awardName,
                    QuickMatch = quickmatchAwards,
                    UnrankedDraft = unrankedDraftAwards,
                    HeroLeague = heroLeagueAwards,
                    TeamLeague = teamLeagueAwards
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

            StatsHeroesMatchAwards totalAwards = new StatsHeroesMatchAwards
            {
                AwardName = "Total",
                QuickMatch = totalQuickMatch,
                UnrankedDraft = totalUnrankedDraft,
                HeroLeague = totalHeroLeague,
                TeamLeague = totalTeamLeague
            };

            await Application.Current.Dispatcher.InvokeAsync(delegate
            {
                MatchAwardDataTotalCollection.Add(totalAwards);
            });
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
            foreach (var award in MatchAwardDataCollection)
                award.AwardImage = null;

            StatsHeroesQuickMatchDataCollection.Clear();
            StatsHeroesUnrankedDraftDataCollection.Clear();
            StatsHeroesHeroLeagueDataCollection.Clear();
            StatsHeroesTeamLeagueDataCollection.Clear();

            StatsHeroesQuickMatchDataTotalCollection.Clear();
            StatsHeroesUnrankedDraftDataTotalCollection.Clear();
            StatsHeroesHeroLeagueDataTotalCollection.Clear();
            StatsHeroesTeamLeagueDataTotalCollection.Clear();

            MatchAwardDataCollection.Clear();
            MatchAwardDataTotalCollection.Clear();
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
