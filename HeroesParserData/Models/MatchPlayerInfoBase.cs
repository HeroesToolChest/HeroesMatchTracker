using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace HeroesParserData.Models
{
    public class MatchPlayerInfoBase
    {
        public BitmapImage LeaderboardPortrait { get; set; }
        public string PlayerName { get; set; }
        public string PlayerTag { get; set; }
        public string CharacterName { get; set; }
        public string CharacterLevel { get; set; }
        public Color RowBackColor { get; set; }
        public Color PortraitBackColor { get; set; }
        public int PlayerNumber { get; set; }
    }
}
