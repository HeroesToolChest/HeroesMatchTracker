using Heroes.ReplayParser;
using HeroesParserData.DataQueries;
using HeroesParserData.Models.StatsModels;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows;
using HeroesParserData.Messages;
using System;

namespace HeroesParserData.ViewModels.Stats
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
            GameModeListVisibility = false;
        }

        protected override async Task RefreshStats()
        {
            StatsHeroesGameModes = new ObservableCollection<StatsHeroesGameModes>();
            var heroesList = HeroesInfo.GetListOfHeroes();

            foreach (var heroName in heroesList)
            {               
                int level = QueryHeroLevel(heroName);

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

                var heroImage = HeroesInfo.GetHeroLeaderboardPortrait(heroName);
                heroImage.Freeze();

                StatsHeroesGameModes statsHeroesGameModes = new StatsHeroesGameModes
                {
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
                    TotalWinPercentage = totalTotal != 0 ? totalWinPercentage : (int?)null,
                };

                await Application.Current.Dispatcher.InvokeAsync(delegate
                {
                    statsHeroesGameModes.LeaderboardPortrait = heroImage;
                    StatsHeroesGameModes.Add(statsHeroesGameModes);
                });
            }
        }

        protected override async Task ReceiveMessage(StatisticsTabMessage action)
        {
            if (action.StatisticsTab == StatisticsTab.GameModes)
            {
                await PerformCommand();
            }
        }
    }
}
