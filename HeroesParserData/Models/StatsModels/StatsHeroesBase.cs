using System.Windows.Media.Imaging;

namespace HeroesParserData.Models.StatsModels
{
    public class StatsHeroesBase
    {
        public BitmapImage LeaderboardPortrait { get; set; }
        public string CharacterName { get; set; }
        public int CharacterLevel { get; set; }
    }
}
