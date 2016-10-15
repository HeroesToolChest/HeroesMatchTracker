using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HeroesParserData.Messages;
using HeroesParserData.Models.StatsModels;
using System.Windows.Media.Imaging;
using System.Windows;
using System.Collections.ObjectModel;
using HeroesParserData.DataQueries;
using Heroes.ReplayParser;
using System.Windows.Data;

namespace HeroesParserData.ViewModels.Stats
{
    public class HeroStatsHeroesViewModel : HeroStatsContext
    {
        private string _selectedHero;
        private List<string> _heroesList;
        private BitmapImage _characterDraftPortrait;
        private string _characterName;
        private string _characterRole;
        private int _characterLevel;

        private ObservableCollection<StatsHeroesMapMatch> _statsHeroesQuickMatchDataCollection = new ObservableCollection<StatsHeroesMapMatch>();
        private ObservableCollection<StatsHeroesMapMatch> _statsHeroesUnrankedDraftDataCollection = new ObservableCollection<StatsHeroesMapMatch>();
        private ObservableCollection<StatsHeroesMapMatch> _statsHeroesHeroLeagueDataCollection = new ObservableCollection<StatsHeroesMapMatch>();
        private ObservableCollection<StatsHeroesMapMatch> _statsHeroesTeamLeagueDataCollection = new ObservableCollection<StatsHeroesMapMatch>();

        private ObservableCollection<StatsHeroesMapMatch> _statsHeroesQuickMatchDataTotalCollection = new ObservableCollection<StatsHeroesMapMatch>();
        private ObservableCollection<StatsHeroesMapMatch> _statsHeroesUnrankedDraftDataTotalCollection = new ObservableCollection<StatsHeroesMapMatch>();
        private ObservableCollection<StatsHeroesMapMatch> _statsHeroesHeroLeagueDataTotalCollection = new ObservableCollection<StatsHeroesMapMatch>();
        private ObservableCollection<StatsHeroesMapMatch> _statsHeroesTeamLeagueDataTotalCollection = new ObservableCollection<StatsHeroesMapMatch>();

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

        public int CharacterLevel
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

        public HeroStatsHeroesViewModel()
            :base()
        {
            HeroesList = HeroesInfo.GetListOfHeroes();
        }

        protected override async Task ReceiveMessage(StatisticsTabMessage action)
        {

        }

        protected override async Task RefreshStats()
        {
            if (string.IsNullOrWhiteSpace(SelectedHero))
                return;

            await Application.Current.Dispatcher.InvokeAsync(delegate
            {
                CharacterDraftPortrait = HeroesInfo.GetHeroPortrait(SelectedHero);
                CharacterName = SelectedHero;
                CharacterRole = HeroesInfo.GetHeroRole(SelectedHero).ToString();
                CharacterLevel = QueryHeroLevel(SelectedHero);

                StatsHeroesQuickMatchDataCollection.Clear();
                StatsHeroesUnrankedDraftDataCollection.Clear();
                StatsHeroesHeroLeagueDataCollection.Clear();
                StatsHeroesTeamLeagueDataCollection.Clear();

                StatsHeroesQuickMatchDataTotalCollection.Clear();
                StatsHeroesUnrankedDraftDataTotalCollection.Clear();
                StatsHeroesHeroLeagueDataTotalCollection.Clear();
                StatsHeroesTeamLeagueDataTotalCollection.Clear();
            });

            var maps = Maps.GetMaps();

            List<Task> list = new List<Task>();
            list.Add(CalculateSetMapMatchStats(maps, GameMode.QuickMatch, StatsHeroesQuickMatchDataCollection, StatsHeroesQuickMatchDataTotalCollection));
            list.Add(CalculateSetMapMatchStats(maps, GameMode.UnrankedDraft, StatsHeroesUnrankedDraftDataCollection, StatsHeroesUnrankedDraftDataTotalCollection));
            list.Add(CalculateSetMapMatchStats(maps, GameMode.HeroLeague, StatsHeroesHeroLeagueDataCollection, StatsHeroesHeroLeagueDataTotalCollection));
            list.Add(CalculateSetMapMatchStats(maps, GameMode.TeamLeague, StatsHeroesTeamLeagueDataCollection, StatsHeroesTeamLeagueDataTotalCollection));

            await Task.WhenAll(list.ToArray());
        }

        private async Task CalculateSetMapMatchStats(List<string> maps, GameMode gameMode, ObservableCollection<StatsHeroesMapMatch> collection, ObservableCollection<StatsHeroesMapMatch> totalCollection)
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
    }
}
