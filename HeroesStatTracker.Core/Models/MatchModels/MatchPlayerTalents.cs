using Heroes.Icons;
using HeroesStatTracker.Data;
using HeroesStatTracker.Data.Models.Replays;
using System.Collections.Generic;
using System.Windows.Media.Imaging;

namespace HeroesStatTracker.Core.Models.MatchModels
{
    public class MatchPlayerTalents : MatchPlayerBase
    {
        public MatchPlayerTalents(MatchPlayerBase matchPlayerBase)
            : base(matchPlayerBase)
        { }

        public MatchPlayerTalents(IDatabaseService database, IHeroesIconsService heroesIcons, ReplayMatchPlayer player)
            : base(database, heroesIcons, player)
        { }

        public List<BitmapImage> Talents { get; set; }
        public List<string> TalentNames { get; set; }
        public List<string> TalentShortTooltips { get; set; }
        public List<string> TalentFullTooltips { get; set; }

        public void SetTalents(List<ReplayMatchPlayerTalent> playerTalentList, int playerNum)
        {
            List<BitmapImage> talents = new List<BitmapImage>();
            List<string> talentNames = new List<string>();
            List<string> talentShortTooltips = new List<string>();
            List<string> talentFullTooltips = new List<string>();

            talents.Add(HeroesIcons.HeroBuilds().GetTalentIcon(playerTalentList[playerNum].TalentName1));
            talents.Add(HeroesIcons.HeroBuilds().GetTalentIcon(playerTalentList[playerNum].TalentName4));
            talents.Add(HeroesIcons.HeroBuilds().GetTalentIcon(playerTalentList[playerNum].TalentName7));
            talents.Add(HeroesIcons.HeroBuilds().GetTalentIcon(playerTalentList[playerNum].TalentName10));
            talents.Add(HeroesIcons.HeroBuilds().GetTalentIcon(playerTalentList[playerNum].TalentName13));
            talents.Add(HeroesIcons.HeroBuilds().GetTalentIcon(playerTalentList[playerNum].TalentName16));
            talents.Add(HeroesIcons.HeroBuilds().GetTalentIcon(playerTalentList[playerNum].TalentName20));

            talentNames.Add(HeroesIcons.HeroBuilds().GetTrueTalentName(playerTalentList[playerNum].TalentName1));
            talentNames.Add(HeroesIcons.HeroBuilds().GetTrueTalentName(playerTalentList[playerNum].TalentName4));
            talentNames.Add(HeroesIcons.HeroBuilds().GetTrueTalentName(playerTalentList[playerNum].TalentName7));
            talentNames.Add(HeroesIcons.HeroBuilds().GetTrueTalentName(playerTalentList[playerNum].TalentName10));
            talentNames.Add(HeroesIcons.HeroBuilds().GetTrueTalentName(playerTalentList[playerNum].TalentName13));
            talentNames.Add(HeroesIcons.HeroBuilds().GetTrueTalentName(playerTalentList[playerNum].TalentName16));
            talentNames.Add(HeroesIcons.HeroBuilds().GetTrueTalentName(playerTalentList[playerNum].TalentName20));

            var talent1 = HeroesIcons.HeroBuilds().GetTalentTooltips(playerTalentList[playerNum].TalentName1);
            var talent4 = HeroesIcons.HeroBuilds().GetTalentTooltips(playerTalentList[playerNum].TalentName4);
            var talent7 = HeroesIcons.HeroBuilds().GetTalentTooltips(playerTalentList[playerNum].TalentName7);
            var talent10 = HeroesIcons.HeroBuilds().GetTalentTooltips(playerTalentList[playerNum].TalentName10);
            var talent13 = HeroesIcons.HeroBuilds().GetTalentTooltips(playerTalentList[playerNum].TalentName13);
            var talent16 = HeroesIcons.HeroBuilds().GetTalentTooltips(playerTalentList[playerNum].TalentName16);
            var talent20 = HeroesIcons.HeroBuilds().GetTalentTooltips(playerTalentList[playerNum].TalentName20);

            talentShortTooltips.Add($"<c val=\"FFFFFF\">{talentNames[0]}:</c> {talent1.Short}");
            talentShortTooltips.Add($"<c val=\"FFFFFF\">{talentNames[1]}:</c> {talent4.Short}");
            talentShortTooltips.Add($"<c val=\"FFFFFF\">{talentNames[2]}:</c> {talent7.Short}");
            talentShortTooltips.Add($"<c val=\"FFFFFF\">{talentNames[3]}:</c> {talent10.Short}");
            talentShortTooltips.Add($"<c val=\"FFFFFF\">{talentNames[4]}:</c> {talent13.Short}");
            talentShortTooltips.Add($"<c val=\"FFFFFF\">{talentNames[5]}:</c> {talent16.Short}");
            talentShortTooltips.Add($"<c val=\"FFFFFF\">{talentNames[6]}:</c> {talent20.Short}");

            talentFullTooltips.Add(talent1.Full);
            talentFullTooltips.Add(talent4.Full);
            talentFullTooltips.Add(talent7.Full);
            talentFullTooltips.Add(talent10.Full);
            talentFullTooltips.Add(talent13.Full);
            talentFullTooltips.Add(talent16.Full);
            talentFullTooltips.Add(talent20.Full);

            Talents = talents;
            TalentNames = talentNames;
            TalentShortTooltips = talentShortTooltips;
            TalentFullTooltips = talentFullTooltips;
        }

        public override void Dispose()
        {
            Talents.ForEach((talentImage) => talentImage = null);
            Talents.Clear();

            base.Dispose();
        }
    }
}
