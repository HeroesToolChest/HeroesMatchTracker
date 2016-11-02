using System.Windows.Media.Imaging;

namespace HeroesParserData.Models.StatsModels
{
    public class StatsHeroesTalentPicks
    {
        public BitmapImage TalentImage { get; set; }
        public string TalentName { get; set; }
        public string TalentShortDescription { get; set; }
        public string TalentFullDescription { get; set; }
        public int Wins { get; set; }
        public int Losses { get; set; }
        public int Total { get; set; }
        public int Winrate { get; set; }
    }
}
