using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace HeroesParserData.Models
{
    public class MatchTalents
    {
        public BitmapImage LeaderboardPortrait { get; set; }
        public string PlayerName { get; set; }
        public string PlayerTag { get; set; }
        public string CharacterName { get; set; }
        public string CharacterLevel { get; set; }
        public BitmapImage Talent1 { get; set; }        
        public BitmapImage Talent4 { get; set; }       
        public BitmapImage Talent7 { get; set; }        
        public BitmapImage Talent10 { get; set; }    
        public BitmapImage Talent13 { get; set; }     
        public BitmapImage Talent16 { get; set; }       
        public BitmapImage Talent20 { get; set; }       
        public string TalentName1 { get; set; }
        public string TalentName4 { get; set; }
        public string TalentName7 { get; set; }
        public string TalentName10 { get; set; }
        public string TalentName13 { get; set; }
        public string TalentName16 { get; set; }
        public string TalentName20 { get; set; }
        public Color TalentsBackColor { get; set; }
        public Color TalentsPortraitBackColor { get; set; }
    }
}
