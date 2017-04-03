using Heroes.Helpers;
using Heroes.Icons;
using Heroes.ReplayParser;
using HeroesMatchData.Core.Models.StatisticsModels;
using HeroesMatchData.Core.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace HeroesMatchData.Core.ViewModels.Statistics
{
    public class HeroesGameModeViewModel : HmdViewModel
    {
        private ObservableCollection<StatsHeroesGameModes> _statsHeroesDataCollection = new ObservableCollection<StatsHeroesGameModes>();
        private ObservableCollection<StatsHeroesGameModes> _statsHeroesDataTotalCollection = new ObservableCollection<StatsHeroesGameModes>();
        private ObservableCollection<StatsHeroesGameModes> _statsHeroesDataAverageCollection = new ObservableCollection<StatsHeroesGameModes>();
        private ObservableCollection<StatsHeroesGameModes> _statsHeroesDataAverageTotalCollection = new ObservableCollection<StatsHeroesGameModes>();

        public HeroesGameModeViewModel(IInternalService internalService)
            : base(internalService)
        {
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

        public void SetData(List<string> mapList, string heroName, Season season, GameMode gameMode)
        {
            ClearData();

            SetStats(mapList, heroName, season, gameMode);
            SetStatTotals();
            SetAverages();
            SetAverageTotals();
        }

        private void SetStats(List<string> mapList, string heroName, Season season, GameMode gameMode)
        {
            foreach (var map in mapList)
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
                StatsHeroesDataCollection.Add(statsHeroesGameModes);
            }
        }

        private void SetStatTotals()
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

            StatsHeroesDataTotalCollection.Add(totalMatch);
        }

        private void SetAverages()
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
                StatsHeroesDataAverageCollection.Add(averageMatch);
            }
        }

        private void SetAverageTotals()
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

            StatsHeroesDataAverageTotalCollection.Add(totalAverageMatch);
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

            StatsHeroesDataCollection.Clear();
            StatsHeroesDataTotalCollection.Clear();
            StatsHeroesDataAverageCollection.Clear();
            StatsHeroesDataAverageTotalCollection.Clear();
        }
    }
}
