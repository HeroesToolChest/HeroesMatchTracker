using System.Windows.Media.Imaging;

namespace HeroesMatchTracker.Core.Models.StatisticsModels
{
    public class StatsHeroesTalents
    {
        public BitmapImage TalentImage { get; set; }
        public string TalentName { get; set; }
        public string TalentSubInfo { get; set; }
        public string TalentShortTooltip { get; set; }
        public string TalentFullTooltip { get; set; }
        public int? Wins { get; set; }
        public int? Losses { get; set; }
        public int? Total { get; set; }
        public double? Winrate { get; set; }
    }
}
