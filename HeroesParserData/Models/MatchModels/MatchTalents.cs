using System.Windows.Media.Imaging;

namespace HeroesParserData.Models.MatchModels
{
    public class MatchTalents : MatchPlayerInfoBase
    {
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

        public MatchTalents(MatchPlayerInfoBase matchPlayerInfoBase)
        {
            LeaderboardPortrait = matchPlayerInfoBase.LeaderboardPortrait;
            PlayerName = matchPlayerInfoBase.PlayerName;
            PlayerTag = matchPlayerInfoBase.PlayerTag;
            CharacterName = matchPlayerInfoBase.CharacterName;
            CharacterLevel = matchPlayerInfoBase.CharacterLevel;
            PlayerSilenced = matchPlayerInfoBase.PlayerSilenced;
            MvpAward = matchPlayerInfoBase.MvpAward;
            MvpAwardName = matchPlayerInfoBase.MvpAwardName;
        }
    }
}
