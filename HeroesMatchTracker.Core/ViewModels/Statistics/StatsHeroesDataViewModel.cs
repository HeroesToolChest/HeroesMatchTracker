using Heroes.Helpers;
using Heroes.Icons.Models;
using HeroesMatchTracker.Core.Models.StatisticsModels;
using HeroesMatchTracker.Core.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace HeroesMatchTracker.Core.ViewModels.Statistics
{
    public class StatsHeroesDataViewModel : HmtViewModel
    {
        private ObservableCollection<StatsHeroesGameModes> _statsHeroesDataCollection = new ObservableCollection<StatsHeroesGameModes>();
        private ObservableCollection<StatsHeroesGameModes> _statsHeroesDataTotalCollection = new ObservableCollection<StatsHeroesGameModes>();
        private ObservableCollection<StatsHeroesGameModes> _statsHeroesDataAverageCollection = new ObservableCollection<StatsHeroesGameModes>();
        private ObservableCollection<StatsHeroesGameModes> _statsHeroesDataAverageTotalCollection = new ObservableCollection<StatsHeroesGameModes>();
        private ObservableCollection<StatsHeroesTalents> _statsHeroesTalents1Collection = new ObservableCollection<StatsHeroesTalents>();
        private ObservableCollection<StatsHeroesTalents> _statsHeroesTalents4Collection = new ObservableCollection<StatsHeroesTalents>();
        private ObservableCollection<StatsHeroesTalents> _statsHeroesTalents7Collection = new ObservableCollection<StatsHeroesTalents>();
        private ObservableCollection<StatsHeroesTalents> _statsHeroesTalents10Collection = new ObservableCollection<StatsHeroesTalents>();
        private ObservableCollection<StatsHeroesTalents> _statsHeroesTalents13Collection = new ObservableCollection<StatsHeroesTalents>();
        private ObservableCollection<StatsHeroesTalents> _statsHeroesTalents16Collection = new ObservableCollection<StatsHeroesTalents>();
        private ObservableCollection<StatsHeroesTalents> _statsHeroesTalents20Collection = new ObservableCollection<StatsHeroesTalents>();
        private ObservableCollection<StatsHeroesAwards> _statsHeroesAwardsCollection = new ObservableCollection<StatsHeroesAwards>();
        private ObservableCollection<StatsHeroesAwards> _statsHeroesAwardsTotalCollection = new ObservableCollection<StatsHeroesAwards>();

        private List<string> MapList;

        public StatsHeroesDataViewModel(IInternalService internalService, List<string> mapList)
            : base(internalService)
        {
            MapList = mapList;
        }

        public bool QueryTotalsAndAverages { get; set; } = true;
        public bool QueryTalents { get; set; } = true;
        public bool QueryAwards { get; set; } = true;
        public int MVPCount { get; private set; }

        public ObservableCollection<StatsHeroesGameModes> StatsHeroesDataCollection
        {
            get => _statsHeroesDataCollection;
            set
            {
                _statsHeroesDataCollection = value;
                RaisePropertyChanged();
            }
        }

        public ObservableCollection<StatsHeroesGameModes> StatsHeroesDataTotalCollection
        {
            get => _statsHeroesDataTotalCollection;
            set
            {
                _statsHeroesDataTotalCollection = value;
                RaisePropertyChanged();
            }
        }

        public ObservableCollection<StatsHeroesGameModes> StatsHeroesDataAverageCollection
        {
            get => _statsHeroesDataAverageCollection;
            set
            {
                _statsHeroesDataAverageCollection = value;
                RaisePropertyChanged();
            }
        }

        public ObservableCollection<StatsHeroesGameModes> StatsHeroesDataAverageTotalCollection
        {
            get => _statsHeroesDataAverageTotalCollection;
            set
            {
                _statsHeroesDataAverageTotalCollection = value;
                RaisePropertyChanged();
            }
        }

        public ObservableCollection<StatsHeroesTalents> StatsHeroesTalents1Collection
        {
            get => _statsHeroesTalents1Collection;
            set
            {
                _statsHeroesTalents1Collection = value;
                RaisePropertyChanged();
            }
        }

        public ObservableCollection<StatsHeroesTalents> StatsHeroesTalents4Collection
        {
            get => _statsHeroesTalents4Collection;
            set
            {
                _statsHeroesTalents4Collection = value;
                RaisePropertyChanged();
            }
        }

        public ObservableCollection<StatsHeroesTalents> StatsHeroesTalents7Collection
        {
            get => _statsHeroesTalents7Collection;
            set
            {
                _statsHeroesTalents7Collection = value;
                RaisePropertyChanged();
            }
        }

        public ObservableCollection<StatsHeroesTalents> StatsHeroesTalents10Collection
        {
            get => _statsHeroesTalents10Collection;
            set
            {
                _statsHeroesTalents10Collection = value;
                RaisePropertyChanged();
            }
        }

        public ObservableCollection<StatsHeroesTalents> StatsHeroesTalents13Collection
        {
            get => _statsHeroesTalents13Collection;
            set
            {
                _statsHeroesTalents13Collection = value;
                RaisePropertyChanged();
            }
        }

        public ObservableCollection<StatsHeroesTalents> StatsHeroesTalents16Collection
        {
            get => _statsHeroesTalents16Collection;
            set
            {
                _statsHeroesTalents16Collection = value;
                RaisePropertyChanged();
            }
        }

        public ObservableCollection<StatsHeroesTalents> StatsHeroesTalents20Collection
        {
            get => _statsHeroesTalents20Collection;
            set
            {
                _statsHeroesTalents20Collection = value;
                RaisePropertyChanged();
            }
        }

        public ObservableCollection<StatsHeroesAwards> StatsHeroesAwardsCollection
        {
            get => _statsHeroesAwardsCollection;
            set
            {
                _statsHeroesAwardsCollection = value;
                RaisePropertyChanged();
            }
        }

        public ObservableCollection<StatsHeroesAwards> StatsHeroesAwardsTotalCollection
        {
            get => _statsHeroesAwardsTotalCollection;
            set
            {
                _statsHeroesAwardsTotalCollection = value;
                RaisePropertyChanged();
            }
        }

        public async Task SetDataAsync(string selectedHero, Season selectedSeason, GameMode selectedGameModes, List<string> selectedMaps)
        {
            await Application.Current.Dispatcher.InvokeAsync(() => ClearData());

            List<Task> list = new List<Task>()
            {
                SetStats(selectedHero, selectedSeason, selectedGameModes),
                SetAwardsAwarded(selectedHero, selectedSeason, selectedMaps),
                SetTalentPicks(selectedHero, selectedSeason, selectedGameModes, TalentTier.Level1, selectedMaps, StatsHeroesTalents1Collection),
                SetTalentPicks(selectedHero, selectedSeason, selectedGameModes, TalentTier.Level4, selectedMaps, StatsHeroesTalents4Collection),
                SetTalentPicks(selectedHero, selectedSeason, selectedGameModes, TalentTier.Level7, selectedMaps, StatsHeroesTalents7Collection),
                SetTalentPicks(selectedHero, selectedSeason, selectedGameModes, TalentTier.Level10, selectedMaps, StatsHeroesTalents10Collection),
                SetTalentPicks(selectedHero, selectedSeason, selectedGameModes, TalentTier.Level13, selectedMaps, StatsHeroesTalents13Collection),
                SetTalentPicks(selectedHero, selectedSeason, selectedGameModes, TalentTier.Level16, selectedMaps, StatsHeroesTalents16Collection),
                SetTalentPicks(selectedHero, selectedSeason, selectedGameModes, TalentTier.Level20, selectedMaps, StatsHeroesTalents20Collection),
            };

            await Task.WhenAll(list.ToArray());
        }

        private Task SetStats(string heroName, Season season, GameMode gameMode)
        {
            if (QueryTotalsAndAverages == false)
                return Task.CompletedTask;

            foreach (var map in MapList)
            {
                int wins = Database.ReplaysDb().Statistics.ReadTotalGameResults(heroName, season, gameMode, true, map);
                int losses = Database.ReplaysDb().Statistics.ReadTotalGameResults(heroName, season, gameMode, false, map);
                int total = wins + losses;
                double winPercentage = Utilities.CalculateWinValue(wins, total);

                var scoreResultsList = Database.ReplaysDb().Statistics.ReadScoreResult(heroName, season, gameMode, map);

                int kills = (int)scoreResultsList.Sum(x => x.SoloKills);
                int assists = (int)scoreResultsList.Sum(x => x.Assists);
                int deaths = (int)scoreResultsList.Sum(x => x.Deaths);

                double siegeDamage = (double)scoreResultsList.Sum(x => x.SiegeDamage);
                double heroDamage = (double)scoreResultsList.Sum(x => x.HeroDamage);
                double experience = (double)scoreResultsList.Sum(x => x.ExperienceContribution);
                int mercsCaptured = (int)scoreResultsList.Sum(x => x.MercCampCaptures);

                TimeSpan gameTime = Database.ReplaysDb().Statistics.ReadTotalMapGameTime(heroName, season, gameMode, map);

                double? healingShielding = (double)scoreResultsList.Sum(x => x.Healing);
                double? damageTaken = (double)scoreResultsList.Sum(x => x.DamageTaken);

                StatsHeroesGameModes statsHeroesGameModes = new StatsHeroesGameModes();

                if (total > 0)
                {
                    statsHeroesGameModes.Wins = wins;
                    statsHeroesGameModes.Losses = losses;
                    statsHeroesGameModes.TotalGames = total;
                    statsHeroesGameModes.WinPercentage = winPercentage;
                    statsHeroesGameModes.Kills = kills;
                    statsHeroesGameModes.Assists = assists;
                    statsHeroesGameModes.Deaths = deaths;
                    statsHeroesGameModes.SiegeDamage = siegeDamage;
                    statsHeroesGameModes.HeroDamage = heroDamage;
                    statsHeroesGameModes.HealingShielding = healingShielding;
                    statsHeroesGameModes.DamageTaken = damageTaken;
                    statsHeroesGameModes.Experience = experience;
                    statsHeroesGameModes.MercsCaptured = mercsCaptured;
                    statsHeroesGameModes.GameTime = gameTime;
                    statsHeroesGameModes.MapName = map;
                    statsHeroesGameModes.MapImage = HeroesIcons.MapBackgrounds().GetMapBackground(map);
                }

                Application.Current.Dispatcher.Invoke(() => StatsHeroesDataCollection.Add(statsHeroesGameModes));
            }

            SetStatTotals();
            SetAverages();

            return Task.CompletedTask;
        }

        private Task SetStatTotals()
        {
            int totalWins = StatsHeroesDataCollection.Sum(x => x.Wins ?? 0);
            int totalLosses = StatsHeroesDataCollection.Sum(x => x.Losses ?? 0);
            int totalTotal = StatsHeroesDataCollection.Sum(x => x.TotalGames ?? 0);
            double totalWinPercentage = Utilities.CalculateWinValue(totalWins, totalTotal);

            int totalKills = StatsHeroesDataCollection.Sum(x => x.Kills ?? 0);
            int totalAssists = StatsHeroesDataCollection.Sum(x => x.Assists ?? 0);
            int totalDeaths = StatsHeroesDataCollection.Sum(x => x.Deaths ?? 0);

            double totalSiegeDamage = StatsHeroesDataCollection.Sum(x => x.SiegeDamage ?? 0);
            double totalHeroDamage = StatsHeroesDataCollection.Sum(x => x.HeroDamage ?? 0);
            double totalHealingShielding = StatsHeroesDataCollection.Sum(x => x.HealingShielding ?? 0);
            double totalDamageTaken = StatsHeroesDataCollection.Sum(x => x.DamageTaken ?? 0);
            double totalExperience = StatsHeroesDataCollection.Sum(x => x.Experience ?? 0);
            int totalMercsCaptured = StatsHeroesDataCollection.Sum(x => x.MercsCaptured ?? 0);
            TimeSpan totalGameTime = TimeSpan.FromSeconds(StatsHeroesDataCollection.Sum(x => x.GameTime.HasValue ? x.GameTime.Value.TotalSeconds : 0));

            StatsHeroesGameModes totalMatch = new StatsHeroesGameModes
            {
                MapName = "Total",
                Wins = totalWins,
                Losses = totalLosses,
                TotalGames = totalTotal,
                WinPercentage = totalWinPercentage,
                Kills = totalKills,
                Assists = totalAssists,
                Deaths = totalDeaths,
                SiegeDamage = totalSiegeDamage,
                HeroDamage = totalHeroDamage,
                HealingShielding = totalHealingShielding,
                DamageTaken = totalDamageTaken,
                Experience = totalExperience,
                MercsCaptured = totalMercsCaptured,
                GameTime = totalGameTime,
            };

            Application.Current.Dispatcher.Invoke(() => StatsHeroesDataTotalCollection.Add(totalMatch));
            SetAverageTotals();

            return Task.CompletedTask;
        }

        private Task SetAverages()
        {
            foreach (var map in StatsHeroesDataCollection)
            {
                int? totalGames = map.TotalGames > 0 ? map.TotalGames : 1;

                int? killsAverage = map.Kills / totalGames;
                int? assistsAverage = map.Assists / totalGames;
                int? deathsAverage = map.Deaths / totalGames;
                double? siegeDamageAverage = map.SiegeDamage / totalGames;
                double? heroDamageAverage = map.HeroDamage / totalGames;
                double? healingShieldingAverage = map.HealingShielding / totalGames;
                double? damageTakenAverage = map.DamageTaken / totalGames;
                double? experienceAverage = map.Experience / totalGames;
                int? mercsCapturedAverage = map.MercsCaptured / totalGames;
                TimeSpan? gameTimeAverage = null;
                if (map.GameTime.HasValue)
                {
                    gameTimeAverage = TimeSpan.FromSeconds(Math.Round(map.GameTime.Value.TotalSeconds / totalGames.Value, 0));
                }

                StatsHeroesGameModes averageMatch = new StatsHeroesGameModes
                {
                    MapName = map.MapName,
                    TotalGames = map.TotalGames,
                    Kills = killsAverage,
                    Assists = assistsAverage,
                    Deaths = deathsAverage,
                    SiegeDamage = siegeDamageAverage,
                    HeroDamage = heroDamageAverage,
                    HealingShielding = healingShieldingAverage,
                    DamageTaken = damageTakenAverage,
                    Experience = experienceAverage,
                    MercsCaptured = mercsCapturedAverage,
                    GameTime = gameTimeAverage,
                    MapImage = map.MapImage,
            };

                Application.Current.Dispatcher.Invoke(() => StatsHeroesDataAverageCollection.Add(averageMatch));
            }

            return Task.CompletedTask;
        }

        private Task SetAverageTotals()
        {
            var dataTotal = StatsHeroesDataTotalCollection[0];

            int? totalAverageTotal = dataTotal.TotalGames > 0 ? dataTotal.TotalGames : 1;

            StatsHeroesGameModes totalAverageMatch = new StatsHeroesGameModes
            {
                MapName = "Total Average",
                TotalGames = dataTotal.TotalGames,
                Kills = dataTotal.Kills / totalAverageTotal,
                Assists = dataTotal.Assists / totalAverageTotal,
                Deaths = dataTotal.Deaths / totalAverageTotal,
                SiegeDamage = dataTotal.SiegeDamage / totalAverageTotal,
                HeroDamage = dataTotal.HeroDamage / totalAverageTotal,
                HealingShielding = dataTotal.HealingShielding / totalAverageTotal,
                DamageTaken = dataTotal.DamageTaken / totalAverageTotal,
                Experience = dataTotal.Experience / totalAverageTotal,
                MercsCaptured = dataTotal.MercsCaptured / totalAverageTotal,
                GameTime = dataTotal.GameTime.HasValue ? TimeSpan.FromSeconds(Math.Round(dataTotal.GameTime.Value.TotalSeconds / totalAverageTotal.Value, 0)) : (TimeSpan?)null,
            };

            Application.Current.Dispatcher.Invoke(() => StatsHeroesDataAverageTotalCollection.Add(totalAverageMatch));

            return Task.CompletedTask;
        }

        private Task SetTalentPicks(string heroName, Season season, GameMode gameMode, TalentTier tier, List<string> selectedMaps, ObservableCollection<StatsHeroesTalents> collection)
        {
            if (QueryTalents == false)
                return Task.CompletedTask;

            var tierTalents = HeroesIcons.HeroBuilds().GetHeroInfo(heroName).GetTierTalents(tier);

            if (tierTalents == null)
                return Task.CompletedTask;

            foreach (var talent in tierTalents)
            {
                int talentWin = Database.ReplaysDb().Statistics.ReadTalentsCountForHero(heroName, season, gameMode, selectedMaps, talent, true);
                int talentLoss = Database.ReplaysDb().Statistics.ReadTalentsCountForHero(heroName, season, gameMode, selectedMaps, talent, false);
                int talentTotal = talentWin + talentLoss;
                double talentWinPercentage = Utilities.CalculateWinValue(talentWin, talentTotal);

                StatsHeroesTalents talentPick = new StatsHeroesTalents
                {
                    TalentName = talent.Name,
                    TalentImage = talent.GetTalentIcon(),
                    TalentSubInfo = talent.Tooltip.GetTalentSubInfo(),
                    TalentShortTooltip = talent.Tooltip.Short,
                    TalentFullTooltip = talent.Tooltip.Full,
                    Wins = talentTotal > 0 ? talentWin : (int?)null,
                    Losses = talentTotal > 0 ? talentLoss : (int?)null,
                    Total = talentTotal > 0 ? talentTotal : (int?)null,
                    Winrate = talentTotal > 0 ? talentWinPercentage : (double?)null,
                };

                Application.Current.Dispatcher.Invoke(() => collection.Add(talentPick));
            }

            return Task.CompletedTask;
        }

        private Task SetAwardsAwarded(string heroName, Season season, List<string> selectedMaps)
        {
            if (QueryAwards == false)
                return Task.CompletedTask;

            var awardsList = HeroesIcons.MatchAwards().GetMatchAwardsList();
            foreach (var award in awardsList)
            {
                int quickmatchAwards = Database.ReplaysDb().Statistics.ReadMatchAwardCountForHero(heroName, season, GameMode.QuickMatch, selectedMaps, award);
                int unrankedDraftAwards = Database.ReplaysDb().Statistics.ReadMatchAwardCountForHero(heroName, season, GameMode.UnrankedDraft, selectedMaps, award);
                int heroLeagueAwards = Database.ReplaysDb().Statistics.ReadMatchAwardCountForHero(heroName, season, GameMode.HeroLeague, selectedMaps, award);
                int teamLeagueAwards = Database.ReplaysDb().Statistics.ReadMatchAwardCountForHero(heroName, season, GameMode.TeamLeague, selectedMaps, award);

                int rowTotal = quickmatchAwards + unrankedDraftAwards + heroLeagueAwards + teamLeagueAwards;

                if (award == "MVP")
                    MVPCount = quickmatchAwards + unrankedDraftAwards + heroLeagueAwards + teamLeagueAwards;

                var awardImage = HeroesIcons.MatchAwards().GetMVPScoreScreenAward(award.ToString(), MVPScoreScreenColor.Blue, out string awardName);

                StatsHeroesAwards matchAwards = new StatsHeroesAwards
                {
                    AwardName = awardName,
                    AwardDescription = HeroesIcons.MatchAwards().GetMatchAwardDescription(award.ToString()),
                    QuickMatch = rowTotal > 0 ? quickmatchAwards : (int?)null,
                    UnrankedDraft = rowTotal > 0 ? unrankedDraftAwards : (int?)null,
                    HeroLeague = rowTotal > 0 ? heroLeagueAwards : (int?)null,
                    TeamLeague = rowTotal > 0 ? teamLeagueAwards : (int?)null,
                    Total = rowTotal > 0 ? rowTotal : (int?)null,
                    AwardImage = awardImage,
            };

                Application.Current.Dispatcher.Invoke(() => StatsHeroesAwardsCollection.Add(matchAwards));
            }

            // get totals
            int totalQuickMatch = StatsHeroesAwardsCollection.Sum(x => x.QuickMatch ?? 0);
            int totalUnrankedDraft = StatsHeroesAwardsCollection.Sum(x => x.UnrankedDraft ?? 0);
            int totalHeroLeague = StatsHeroesAwardsCollection.Sum(x => x.HeroLeague ?? 0);
            int totalTeamLeague = StatsHeroesAwardsCollection.Sum(x => x.TeamLeague ?? 0);
            int totalTotal = StatsHeroesAwardsCollection.Sum(x => x.Total ?? 0);

            StatsHeroesAwards totalAwards = new StatsHeroesAwards
            {
                AwardName = "Total",
                QuickMatch = totalQuickMatch,
                UnrankedDraft = totalUnrankedDraft,
                HeroLeague = totalHeroLeague,
                TeamLeague = totalTeamLeague,
                Total = totalTotal,
            };

            Application.Current.Dispatcher.Invoke(() => StatsHeroesAwardsTotalCollection.Add(totalAwards));

            return Task.CompletedTask;
        }

        private void ClearData()
        {
            foreach (var map in StatsHeroesDataCollection)
                map.MapImage = null;
            foreach (var map in StatsHeroesDataTotalCollection)
                map.MapImage = null;
            foreach (var map in StatsHeroesDataAverageCollection)
                map.MapImage = null;
            foreach (var map in StatsHeroesDataAverageTotalCollection)
                map.MapImage = null;
            foreach (var talent in StatsHeroesTalents1Collection)
                talent.TalentImage = null;
            foreach (var talent in StatsHeroesTalents4Collection)
                talent.TalentImage = null;
            foreach (var talent in StatsHeroesTalents7Collection)
                talent.TalentImage = null;
            foreach (var talent in StatsHeroesTalents10Collection)
                talent.TalentImage = null;
            foreach (var talent in StatsHeroesTalents13Collection)
                talent.TalentImage = null;
            foreach (var talent in StatsHeroesTalents16Collection)
                talent.TalentImage = null;
            foreach (var talent in StatsHeroesTalents20Collection)
                talent.TalentImage = null;
            foreach (var award in StatsHeroesAwardsCollection)
                award.AwardImage = null;
            foreach (var award in StatsHeroesAwardsTotalCollection)
                award.AwardImage = null;

            StatsHeroesDataCollection.Clear();
            StatsHeroesDataTotalCollection.Clear();
            StatsHeroesDataAverageCollection.Clear();
            StatsHeroesDataAverageTotalCollection.Clear();
            StatsHeroesTalents1Collection.Clear();
            StatsHeroesTalents4Collection.Clear();
            StatsHeroesTalents7Collection.Clear();
            StatsHeroesTalents10Collection.Clear();
            StatsHeroesTalents13Collection.Clear();
            StatsHeroesTalents16Collection.Clear();
            StatsHeroesTalents20Collection.Clear();
            StatsHeroesAwardsCollection.Clear();
            StatsHeroesAwardsTotalCollection.Clear();
        }
    }
}
