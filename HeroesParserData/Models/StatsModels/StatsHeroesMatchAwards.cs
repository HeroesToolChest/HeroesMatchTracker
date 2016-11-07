using System.Windows.Media.Imaging;

namespace HeroesParserData.Models.StatsModels
{
    public class StatsHeroesMatchAwards
    {
        public BitmapImage AwardImage { get; set; }
        public string AwardName { get; set; }
        public string AwardDescription { get; set; }
        public int QuickMatch { get; set; }
        public int UnrankedDraft { get; set; }
        public int HeroLeague { get; set; }
        public int TeamLeague { get; set; }
        public int Total { get; set; }
    }
}
