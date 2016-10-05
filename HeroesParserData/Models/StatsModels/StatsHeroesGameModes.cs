namespace HeroesParserData.Models.StatsModels
{
    public class StatsHeroesGameModes : StatsHeroesBase
    {
        public int QuickMatchWins { get; set; }
        public int QuickMatchLosses { get; set; }
        public int QuickMatchGames { get; set; }
        public int? QuickMatchWinPercentage { get; set; }
        public int UnrankedDraftWins { get; set; }
        public int UnrankedDraftLosses { get; set; }
        public int UnrankedDraftGames { get; set; }
        public int? UnrankedDraftWinPercentage { get; set; }
        public int HeroLeagueWins { get; set; }
        public int HeroLeagueLosses { get; set; }
        public int HeroLeagueGames { get; set; }
        public int? HeroLeagueWinPercentage { get; set; }
        public int TeamLeagueWins { get; set; }
        public int TeamLeagueLosses { get; set; }
        public int TeamLeagueGames { get; set; }
        public int? TeamLeagueWinPercentage { get; set; }
        public int TotalWins { get; set; }
        public int TotalLosses { get; set; }
        public int TotalGames { get; set; }
        public int TotalWinPercentage { get; set; }
    }
}
