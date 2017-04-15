using Heroes.Helpers;
using Heroes.Icons;
using Heroes.ReplayParser;
using HeroesMatchData.Core.Models.StatisticsModels;
using HeroesMatchData.Core.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace HeroesMatchData.Core.ViewModels.Statistics
{
    public class StatsHeroesDataViewModel : HmdViewModel
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

        public async Task SetData(string selectedHero, Season selectedSeason, GameMode selectedGameModes, List<string> selectedMaps)
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
            foreach (var map in MapList)
            {
                int wins = Database.ReplaysDb().Statistics.ReadTotalGameResults(heroName, season, gameMode, true, map);
                int losses = Database.ReplaysDb().Statistics.ReadTotalGameResults(heroName, season, gameMode, false, map);
                int total = wins + losses;
                int winPercentage = Utilities.CalculateWinPercentage(wins, total);

                var scoreResultsList = Database.ReplaysDb().Statistics.ReadScoreResult(heroName, season, gameMode, map);

                int kills = (int)scoreResultsList.Sum(x => x.SoloKills);
                int assists = (int)scoreResultsList.Sum(x => x.Assists);
                int deaths = (int)scoreResultsList.Sum(x => x.Deaths);

                double siegeDamage = (double)scoreResultsList.Sum(x => x.SiegeDamage);
                double heroDamage = (double)scoreResultsList.Sum(x => x.HeroDamage);
                double experience = (double)scoreResultsList.Sum(x => x.ExperienceContribution);
                int mercsCaptured = (int)scoreResultsList.Sum(x => x.MercCampCaptures);

                TimeSpan gameTime = Database.ReplaysDb().Statistics.ReadTotalMapGameTime(heroName, season, gameMode, map);

                double role = 0;
                if (HeroesIcons.Heroes().GetHeroRoleList(heroName)[0] == HeroRole.Warrior)
                    role = (double)scoreResultsList.Sum(x => x.DamageTaken);
                else if (HeroesIcons.Heroes().GetHeroRoleList(heroName)[0] == HeroRole.Support || HeroesIcons.IsNonSupportHeroWithHealingStat(heroName))
                    role = (double)scoreResultsList.Sum(x => x.Healing);

                var mapImage = HeroesIcons.MapBackgrounds().GetMapBackground(map, false);
                mapImage.Freeze();

                StatsHeroesGameModes statsHeroesGameModes = new StatsHeroesGameModes
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
                    GameTime = gameTime,
                };

                statsHeroesGameModes.MapImage = mapImage;
                Application.Current.Dispatcher.Invoke(() => StatsHeroesDataCollection.Add(statsHeroesGameModes));
            }

            SetStatTotals();
            SetAverages();

            return Task.CompletedTask;
        }

        private Task SetStatTotals()
        {
            int totalWins = StatsHeroesDataCollection.Sum(x => x.Wins);
            int totalLosses = StatsHeroesDataCollection.Sum(x => x.Losses);
            int totalTotal = StatsHeroesDataCollection.Sum(x => x.TotalGames);
            int totalWinPercentage = Utilities.CalculateWinPercentage(totalWins, totalTotal);

            int totalKills = StatsHeroesDataCollection.Sum(x => x.Kills);
            int totalAssists = StatsHeroesDataCollection.Sum(x => x.Assists);
            int totalDeaths = StatsHeroesDataCollection.Sum(x => x.Deaths);

            double totalSiegeDamage = StatsHeroesDataCollection.Sum(x => x.SiegeDamage);
            double totalHeroDamage = StatsHeroesDataCollection.Sum(x => x.HeroDamage);
            double totalrole = StatsHeroesDataCollection.Sum(x => x.Role);
            double totalExperience = StatsHeroesDataCollection.Sum(x => x.Experience);
            int totalMercsCaptured = StatsHeroesDataCollection.Sum(x => x.MercsCaptured);
            TimeSpan totalGameTime = TimeSpan.FromSeconds(StatsHeroesDataCollection.Sum(x => x.GameTime.TotalSeconds));

            StatsHeroesGameModes totalMatch = new StatsHeroesGameModes
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
                int totalGames = map.TotalGames > 0 ? map.TotalGames : 1;

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

                StatsHeroesGameModes averageMatch = new StatsHeroesGameModes
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
                    GameTime = gameTimeAverage,
                };

                averageMatch.MapImage = mapImage;
                Application.Current.Dispatcher.Invoke(() => StatsHeroesDataAverageCollection.Add(averageMatch));
            }

            return Task.CompletedTask;
        }

        private Task SetAverageTotals()
        {
            var dataTotal = StatsHeroesDataTotalCollection[0];

            int totalAverageTotal = dataTotal.TotalGames > 0 ? dataTotal.TotalGames : 1;

            StatsHeroesGameModes totalAverageMatch = new StatsHeroesGameModes
            {
                MapName = "Total Average",
                TotalGames = dataTotal.TotalGames,
                Kills = dataTotal.Kills / totalAverageTotal,
                Assists = dataTotal.Assists / totalAverageTotal,
                Deaths = dataTotal.Deaths / totalAverageTotal,
                SiegeDamage = dataTotal.SiegeDamage / totalAverageTotal,
                HeroDamage = dataTotal.HeroDamage / totalAverageTotal,
                Role = dataTotal.Role / totalAverageTotal,
                Experience = dataTotal.Experience / totalAverageTotal,
                MercsCaptured = dataTotal.MercsCaptured / totalAverageTotal,
                GameTime = TimeSpan.FromSeconds(Math.Round(dataTotal.GameTime.TotalSeconds / totalAverageTotal, 0)),
            };

            Application.Current.Dispatcher.Invoke(() => StatsHeroesDataAverageTotalCollection.Add(totalAverageMatch));

            return Task.CompletedTask;
        }

        private Task SetTalentPicks(string heroName, Season season, GameMode gameMode, TalentTier tier, List<string> selectedMaps, ObservableCollection<StatsHeroesTalents> collection)
        {
            var tierTalents = HeroesIcons.HeroBuilds().GetTierTalentsForHero(heroName, tier);
            foreach (var talent in tierTalents)
            {
                var talentImage = HeroesIcons.HeroBuilds().GetTalentIcon(talent);
                talentImage.Freeze();

                int talentWin = Database.ReplaysDb().Statistics.ReadTalentsCountForHero(heroName, season, gameMode, selectedMaps, talent, tier, true);
                int talentLoss = Database.ReplaysDb().Statistics.ReadTalentsCountForHero(heroName, season, gameMode, selectedMaps, talent, tier, false);
                int talentTotal = talentWin + talentLoss;
                int talentWinPercentage = Utilities.CalculateWinPercentage(talentWin, talentTotal);
                TalentTooltip talentTooltip = HeroesIcons.HeroBuilds().GetTalentTooltips(talent);

                StatsHeroesTalents talentPicks = new StatsHeroesTalents
                {
                    TalentName = HeroesIcons.HeroBuilds().GetTrueTalentName(talent),
                    TalentShortTooltip = talentTooltip.Short,
                    TalentFullTooltip = talentTooltip.Full,
                    Wins = talentWin,
                    Losses = talentLoss,
                    Total = talentTotal,
                    Winrate = talentWinPercentage,
                };

                talentPicks.TalentImage = talentImage;
                Application.Current.Dispatcher.Invoke(() => collection.Add(talentPicks));
            }

            return Task.CompletedTask;
        }

        private Task SetAwardsAwarded(string heroName, Season season, List<string> selectedMaps)
        {
            var awardsList = HeroesIcons.MatchAwards().GetMatchAwardsList();
            foreach (var award in awardsList)
            {
                int quickmatchAwards = Database.ReplaysDb().Statistics.ReadMatchAwardCountForHero(heroName, season, GameMode.QuickMatch, selectedMaps, award);
                int unrankedDraftAwards = Database.ReplaysDb().Statistics.ReadMatchAwardCountForHero(heroName, season, GameMode.UnrankedDraft, selectedMaps, award);
                int heroLeagueAwards = Database.ReplaysDb().Statistics.ReadMatchAwardCountForHero(heroName, season, GameMode.HeroLeague, selectedMaps, award);
                int teamLeagueAwards = Database.ReplaysDb().Statistics.ReadMatchAwardCountForHero(heroName, season, GameMode.TeamLeague, selectedMaps, award);

                int rowTotal = quickmatchAwards + unrankedDraftAwards + heroLeagueAwards + teamLeagueAwards;

                //if (award == "MVP")
                //    TotalMVPCount = quickmatchAwards + unrankedDraftAwards + heroLeagueAwards + teamLeagueAwards;

                var awardImage = HeroesIcons.MatchAwards().GetMVPScoreScreenAward(award.ToString(), MVPScoreScreenColor.Blue, out string awardName);
                awardImage.Freeze();

                StatsHeroesAwards matchAwards = new StatsHeroesAwards
                {
                    AwardName = awardName,
                    AwardDescription = HeroesIcons.MatchAwards().GetMatchAwardDescription(award.ToString()),
                    QuickMatch = quickmatchAwards,
                    UnrankedDraft = unrankedDraftAwards,
                    HeroLeague = heroLeagueAwards,
                    TeamLeague = teamLeagueAwards,
                    Total = rowTotal,
                };

                matchAwards.AwardImage = awardImage;
                Application.Current.Dispatcher.Invoke(() => StatsHeroesAwardsCollection.Add(matchAwards));
            }

            // get totals
            int totalQuickMatch = StatsHeroesAwardsCollection.Sum(x => x.QuickMatch);
            int totalUnrankedDraft = StatsHeroesAwardsCollection.Sum(x => x.UnrankedDraft);
            int totalHeroLeague = StatsHeroesAwardsCollection.Sum(x => x.HeroLeague);
            int totalTeamLeague = StatsHeroesAwardsCollection.Sum(x => x.TeamLeague);
            int totalTotal = StatsHeroesAwardsCollection.Sum(x => x.Total);

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
