using HeroesMatchData.Core.Services;
using HeroesMatchData.Data.Models.Replays;
using System.Collections.Generic;
using System.Windows.Media.Imaging;

namespace HeroesMatchData.Core.Models.MatchModels
{
    public class MatchPlayerTalents : MatchPlayerBase
    {
        public MatchPlayerTalents(MatchPlayerBase matchPlayerBase)
            : base(matchPlayerBase)
        { }

        public MatchPlayerTalents(IInternalService internalService, IWebsiteService website, ReplayMatchPlayer player)
            : base(internalService, website, player)
        { }

        public List<BitmapImage> Talents { get; private set; }
        public List<string> TalentNames { get; private set; }
        public List<string> TalentShortTooltips { get; private set; }
        public List<string> TalentFullTooltips { get; private set; }

        public void SetTalents(ReplayMatchPlayerTalent playerTalentList)
        {
            List<BitmapImage> talents = new List<BitmapImage>();
            List<string> talentNames = new List<string>();
            List<string> talentShortTooltips = new List<string>();
            List<string> talentFullTooltips = new List<string>();

            talents.Add(HeroesIcons.HeroBuilds().GetTalentIcon(playerTalentList.TalentName1));
            talents.Add(HeroesIcons.HeroBuilds().GetTalentIcon(playerTalentList.TalentName4));
            talents.Add(HeroesIcons.HeroBuilds().GetTalentIcon(playerTalentList.TalentName7));
            talents.Add(HeroesIcons.HeroBuilds().GetTalentIcon(playerTalentList.TalentName10));
            talents.Add(HeroesIcons.HeroBuilds().GetTalentIcon(playerTalentList.TalentName13));
            talents.Add(HeroesIcons.HeroBuilds().GetTalentIcon(playerTalentList.TalentName16));
            talents.Add(HeroesIcons.HeroBuilds().GetTalentIcon(playerTalentList.TalentName20));

            talentNames.Add(HeroesIcons.HeroBuilds().GetTrueTalentName(playerTalentList.TalentName1));
            talentNames.Add(HeroesIcons.HeroBuilds().GetTrueTalentName(playerTalentList.TalentName4));
            talentNames.Add(HeroesIcons.HeroBuilds().GetTrueTalentName(playerTalentList.TalentName7));
            talentNames.Add(HeroesIcons.HeroBuilds().GetTrueTalentName(playerTalentList.TalentName10));
            talentNames.Add(HeroesIcons.HeroBuilds().GetTrueTalentName(playerTalentList.TalentName13));
            talentNames.Add(HeroesIcons.HeroBuilds().GetTrueTalentName(playerTalentList.TalentName16));
            talentNames.Add(HeroesIcons.HeroBuilds().GetTrueTalentName(playerTalentList.TalentName20));

            var talent1 = HeroesIcons.HeroBuilds().GetTalentTooltips(playerTalentList.TalentName1);
            var talent4 = HeroesIcons.HeroBuilds().GetTalentTooltips(playerTalentList.TalentName4);
            var talent7 = HeroesIcons.HeroBuilds().GetTalentTooltips(playerTalentList.TalentName7);
            var talent10 = HeroesIcons.HeroBuilds().GetTalentTooltips(playerTalentList.TalentName10);
            var talent13 = HeroesIcons.HeroBuilds().GetTalentTooltips(playerTalentList.TalentName13);
            var talent16 = HeroesIcons.HeroBuilds().GetTalentTooltips(playerTalentList.TalentName16);
            var talent20 = HeroesIcons.HeroBuilds().GetTalentTooltips(playerTalentList.TalentName20);

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
