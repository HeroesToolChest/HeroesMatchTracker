namespace HeroesMatchTracker.Core.Models.StatisticsModels
{
    public class StatsOverviewMaps
    {
        public string MapName { get; set; }
        public int Wins { get; set; }
        public int Losses { get; set; }
        public decimal Winrate { get; set; }
    }
}
