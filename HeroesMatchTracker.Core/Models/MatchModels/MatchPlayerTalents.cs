using HeroesMatchTracker.Core.Services;
using HeroesMatchTracker.Data.Models.Replays;
using System.Collections.Generic;
using System.Windows.Media.Imaging;

namespace HeroesMatchTracker.Core.Models.MatchModels
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

            var talentIcon1 = HeroesIcons.HeroBuilds().GetTalentIcon(playerTalentList.TalentName1);
            talentIcon1.Freeze();
            var talentIcon4 = HeroesIcons.HeroBuilds().GetTalentIcon(playerTalentList.TalentName4);
            talentIcon4.Freeze();
            var talentIcon7 = HeroesIcons.HeroBuilds().GetTalentIcon(playerTalentList.TalentName7);
            talentIcon7.Freeze();
            var talentIcon10 = HeroesIcons.HeroBuilds().GetTalentIcon(playerTalentList.TalentName10);
            talentIcon10.Freeze();
            var talentIcon13 = HeroesIcons.HeroBuilds().GetTalentIcon(playerTalentList.TalentName13);
            talentIcon13.Freeze();
            var talentIcon16 = HeroesIcons.HeroBuilds().GetTalentIcon(playerTalentList.TalentName16);
            talentIcon16.Freeze();
            var talentIcon20 = HeroesIcons.HeroBuilds().GetTalentIcon(playerTalentList.TalentName20);
            talentIcon20.Freeze();

            talents.Add(talentIcon1);
            talents.Add(talentIcon4);
            talents.Add(talentIcon7);
            talents.Add(talentIcon10);
            talents.Add(talentIcon13);
            talents.Add(talentIcon16);
            talents.Add(talentIcon20);

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
