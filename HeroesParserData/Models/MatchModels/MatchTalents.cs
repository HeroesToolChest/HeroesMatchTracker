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
        public string TalentShortDescription1 { get; set; }
        public string TalentShortDescription4 { get; set; }
        public string TalentShortDescription7 { get; set; }
        public string TalentShortDescription10 { get; set; }
        public string TalentShortDescription13 { get; set; }
        public string TalentShortDescription16 { get; set; }
        public string TalentShortDescription20 { get; set; }
        public string TalentFullDescription1 { get; set; }
        public string TalentFullDescription4 { get; set; }
        public string TalentFullDescription7 { get; set; }
        public string TalentFullDescription10 { get; set; }
        public string TalentFullDescription13 { get; set; }
        public string TalentFullDescription16 { get; set; }
        public string TalentFullDescription20 { get; set; }


        public MatchTalents(MatchPlayerInfoBase matchPlayerInfoBase)
        {
            LeaderboardPortrait = matchPlayerInfoBase.LeaderboardPortrait;
            PartyIcon = matchPlayerInfoBase.PartyIcon;
            PlayerName = matchPlayerInfoBase.PlayerName;
            PlayerTag = matchPlayerInfoBase.PlayerTag;
            CharacterName = matchPlayerInfoBase.CharacterName;
            CharacterLevel = matchPlayerInfoBase.CharacterLevel;
            PlayerSilenced = matchPlayerInfoBase.PlayerSilenced;
            MvpAward = matchPlayerInfoBase.MvpAward;
            MvpAwardName = matchPlayerInfoBase.MvpAwardName;
            RowBackColor = matchPlayerInfoBase.RowBackColor;
        }
    }
}
