using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace HeroesParserData.Models
{
    public class MatchInfo
    {
        public string PlayerName { get; set; }
        public int BattleNetTag { get; set; }
        public int BattleNetId { get; set; }
        public string CharacterName { get; set; }
        public int CharacterLevel { get; set; }
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
    }
}
