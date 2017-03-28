using System;
using System.Windows.Media.Imaging;

namespace HeroesMatchData.Core.Models.StatisticsModels
{
    public class StatsHeroesGamesModes
    {
        public BitmapImage MapImage { get; set; }
        public string MapName { get; set; }
        public int Wins { get; set; }
        public int Losses { get; set; }
        public int TotalGames { get; set; }
        public int? WinPercentage { get; set; }
        public int Kills { get; set; }
        public int Assists { get; set; }
        public int Deaths { get; set; }
        public double SiegeDamage { get; set; }
        public double HeroDamage { get; set; }
        public double Experience { get; set; }
        public double Role { get; set; }
        public int MercsCaptured { get; set; }
        public TimeSpan GameTime { get; set; }
    }
}
