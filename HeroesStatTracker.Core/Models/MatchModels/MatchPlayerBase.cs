using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace HeroesStatTracker.Core.Models.MatchModels
{
    public class MatchPlayerBase
    {
        public BitmapImage LeaderboardPortrait { get; set; }
        public BitmapImage MvpAward { get; set; }
        public BitmapImage PartyIcon { get; set; }
        public string PlayerName { get; set; }
        public string CharacterName { get; set; }
        public string CharacterTooltip { get; set; }
        public string CharacterLevel { get; set; }
        public string MvpAwardName { get; set; }
        public Color RowBackColor { get; set; }
        public Color PortraitBackColor { get; set; }
        public int PlayerNumber { get; set; }
        public bool Silenced { get; set; }
    }
}
