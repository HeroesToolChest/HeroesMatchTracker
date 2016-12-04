using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace HeroesParserData.Models.MatchModels
{
    public class MatchPlayerInfoBase
    {
        public BitmapImage LeaderboardPortrait { get; set; }
        public BitmapImage MvpAward { get; set; }
        public BitmapImage PartyIcon { get; set; }
        public string PlayerName { get; set; }
        public string CharacterName { get; set; }
        public string CharacterLevel { get; set; }
        public string MvpAwardName { get; set; }
        public Color RowBackColor { get; set; }
        public Color PortraitBackColor { get; set; }
        public int PlayerNumber { get; set; }
        public bool PlayerSilenced { get; set; }
        public PlayerSearchContextMenu PlayerSearchContextMenu { get; set; } = new PlayerSearchContextMenu();
    }
}
