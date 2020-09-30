using System.IO;

namespace HeroesMatchTracker.Core.Models.StatisticsModels
{
    public class StatsHeroesAwards
    {
        public Stream AwardImage { get; set; }
        public string AwardName { get; set; }
        public string AwardDescription { get; set; }
        public int? QuickMatch { get; set; }
        public int? UnrankedDraft { get; set; }
        public int? StormLeague { get; set; }
        public int? HeroLeague { get; set; }
        public int? TeamLeague { get; set; }
        public int? Brawl { get; set; }
        public int? ARAM { get; set; }
        public int? Total { get; set; }
    }
}
