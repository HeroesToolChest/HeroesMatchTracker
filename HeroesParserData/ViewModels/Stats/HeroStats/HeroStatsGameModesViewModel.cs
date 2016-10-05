using Heroes.ReplayParser;
using HeroesParserData.DataQueries;
using HeroesParserData.Models.StatsModels;
using HeroesParserData.Properties;
using NLog;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace HeroesParserData.ViewModels.Stats.HeroStats
{
    public class HeroStatsGameModesViewModel : HeroStatsContext
    {
        private ObservableCollection<StatsHeroesGameModes> _statsHeroesGameModes = new ObservableCollection<StatsHeroesGameModes>();

        public ObservableCollection<StatsHeroesGameModes> StatsHeroesGameModes
        {
            get { return _statsHeroesGameModes; }
            set
            {
                _statsHeroesGameModes = value;
                RaisePropertyChangedEvent(nameof(StatsHeroesGameModes));
            }
        }

        public HeroStatsGameModesViewModel()
            :base()
        {

        }

        protected override void RefreshStats()
        {
            StatsHeroesGameModes = new ObservableCollection<StatsHeroesGameModes>();
            var heroesList = HeroesInfo.GetListOfHeroes();

            foreach (var heroName in heroesList)
            {               
                int level = QueryHeroLevels(heroName);

                int quickMatchWins = Query.HeroStatsGameMode.GetWinsOrLossesForHero(heroName, GetSeasonSelected, GameMode.QuickMatch, true);
                int quickMatchLosses = Query.HeroStatsGameMode.GetWinsOrLossesForHero(heroName, GetSeasonSelected, GameMode.QuickMatch, false);
                int quickMatchTotal = quickMatchWins + quickMatchLosses;
                int quickMatchWinPercentage = Utilities.CalculateWinPercentage(quickMatchWins, quickMatchTotal);

                int unrankedDraftWins = Query.HeroStatsGameMode.GetWinsOrLossesForHero(heroName, GetSeasonSelected, GameMode.UnrankedDraft, true);
                int unrankedDraftLosses = Query.HeroStatsGameMode.GetWinsOrLossesForHero(heroName, GetSeasonSelected, GameMode.UnrankedDraft, false);
                int unrankedDraftTotal = unrankedDraftWins + unrankedDraftLosses;
                int unrankedDraftWinPercentage = Utilities.CalculateWinPercentage(unrankedDraftWins, unrankedDraftTotal);

                int heroLeagueWins = Query.HeroStatsGameMode.GetWinsOrLossesForHero(heroName, GetSeasonSelected, GameMode.HeroLeague, true);
                int heroLeagueLosses = Query.HeroStatsGameMode.GetWinsOrLossesForHero(heroName, GetSeasonSelected, GameMode.HeroLeague, false);
                int heroLeagueTotal = heroLeagueWins + heroLeagueLosses;
                int heroLeagueWinPercentage = Utilities.CalculateWinPercentage(heroLeagueWins, heroLeagueTotal);

                int teamLeagueWins = Query.HeroStatsGameMode.GetWinsOrLossesForHero(heroName, GetSeasonSelected, GameMode.TeamLeague, true);
                int teamLeagueLosses = Query.HeroStatsGameMode.GetWinsOrLossesForHero(heroName, GetSeasonSelected, GameMode.TeamLeague, false);
                int teamLeagueTotal = teamLeagueWins + teamLeagueLosses;
                int teamLeagueWinPercentage = Utilities.CalculateWinPercentage(teamLeagueWins, teamLeagueTotal);

                int totalWins = quickMatchWins + unrankedDraftWins + heroLeagueWins + teamLeagueWins;
                int totalLosses = quickMatchLosses + unrankedDraftLosses + heroLeagueLosses + teamLeagueLosses;
                int totalTotal = totalWins + totalLosses;
                int totalWinPercentage = Utilities.CalculateWinPercentage(totalWins, totalTotal);
                StatsHeroesGameModes statsHeroesGameModes = new StatsHeroesGameModes
                {
                    LeaderboardPortrait = HeroesInfo.GetHeroLeaderboardPortrait(heroName),
                    CharacterName = heroName,
                    CharacterLevel = level,
                    QuickMatchWins = quickMatchWins,
                    QuickMatchLosses = quickMatchLosses,
                    QuickMatchGames = quickMatchTotal,
                    QuickMatchWinPercentage = quickMatchTotal != 0? quickMatchWinPercentage : (int?)null,
                    UnrankedDraftWins = unrankedDraftWins,
                    UnrankedDraftLosses = unrankedDraftLosses,
                    UnrankedDraftGames = unrankedDraftTotal,
                    UnrankedDraftWinPercentage = unrankedDraftTotal != 0 ? unrankedDraftWinPercentage : (int?)null,
                    HeroLeagueWins = heroLeagueWins,
                    HeroLeagueLosses = heroLeagueLosses,
                    HeroLeagueGames = heroLeagueTotal,
                    HeroLeagueWinPercentage = heroLeagueTotal != 0 ? heroLeagueWinPercentage : (int?)null,
                    TeamLeagueWins = heroLeagueWins,
                    TeamLeagueLosses = teamLeagueLosses,
                    TeamLeagueGames = teamLeagueTotal,
                    TeamLeagueWinPercentage = teamLeagueTotal != 0 ? teamLeagueWinPercentage : (int?)null,
                    TotalWins = totalWins,
                    TotalLosses = totalLosses,
                    TotalGames = totalTotal,
                    TotalWinPercentage = totalWinPercentage,
                };

                StatsHeroesGameModes.Add(statsHeroesGameModes);
            }
        }
    }
}
