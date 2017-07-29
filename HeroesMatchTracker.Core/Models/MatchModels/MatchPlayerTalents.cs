using Heroes.Icons;
using HeroesMatchTracker.Core.Services;
using HeroesMatchTracker.Data.Models.Replays;
using System;
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

        public List<BitmapImage> Talents { get; private set; } = new List<BitmapImage>();
        public List<string> TalentNames { get; private set; } = new List<string>();
        public List<string> TalentShortTooltips { get; private set; } = new List<string>();
        public List<string> TalentFullTooltips { get; private set; } = new List<string>();
        public List<string> TalentSubInfo { get; private set; } = new List<string>();

        public void SetTalents(ReplayMatchPlayerTalent playerTalentList)
        {
            var talentIcon1 = HeroesIcons.HeroBuilds().GetTalentIcon(playerTalentList.TalentName1);
            var talentIcon4 = HeroesIcons.HeroBuilds().GetTalentIcon(playerTalentList.TalentName4);
            var talentIcon7 = HeroesIcons.HeroBuilds().GetTalentIcon(playerTalentList.TalentName7);
            var talentIcon10 = HeroesIcons.HeroBuilds().GetTalentIcon(playerTalentList.TalentName10);
            var talentIcon13 = HeroesIcons.HeroBuilds().GetTalentIcon(playerTalentList.TalentName13);
            var talentIcon16 = HeroesIcons.HeroBuilds().GetTalentIcon(playerTalentList.TalentName16);
            var talentIcon20 = HeroesIcons.HeroBuilds().GetTalentIcon(playerTalentList.TalentName20);

            Talents.Add(talentIcon1);
            Talents.Add(talentIcon4);
            Talents.Add(talentIcon7);
            Talents.Add(talentIcon10);
            Talents.Add(talentIcon13);
            Talents.Add(talentIcon16);
            Talents.Add(talentIcon20);

            TalentNames.Add(HeroesIcons.HeroBuilds().GetTrueTalentName(playerTalentList.TalentName1));
            TalentNames.Add(HeroesIcons.HeroBuilds().GetTrueTalentName(playerTalentList.TalentName4));
            TalentNames.Add(HeroesIcons.HeroBuilds().GetTrueTalentName(playerTalentList.TalentName7));
            TalentNames.Add(HeroesIcons.HeroBuilds().GetTrueTalentName(playerTalentList.TalentName10));
            TalentNames.Add(HeroesIcons.HeroBuilds().GetTrueTalentName(playerTalentList.TalentName13));
            TalentNames.Add(HeroesIcons.HeroBuilds().GetTrueTalentName(playerTalentList.TalentName16));
            TalentNames.Add(HeroesIcons.HeroBuilds().GetTrueTalentName(playerTalentList.TalentName20));

            TalentTooltip talent1 = HeroesIcons.HeroBuilds().GetTalentTooltips(playerTalentList.TalentName1);
            TalentTooltip talent4 = HeroesIcons.HeroBuilds().GetTalentTooltips(playerTalentList.TalentName4);
            TalentTooltip talent7 = HeroesIcons.HeroBuilds().GetTalentTooltips(playerTalentList.TalentName7);
            TalentTooltip talent10 = HeroesIcons.HeroBuilds().GetTalentTooltips(playerTalentList.TalentName10);
            TalentTooltip talent13 = HeroesIcons.HeroBuilds().GetTalentTooltips(playerTalentList.TalentName13);
            TalentTooltip talent16 = HeroesIcons.HeroBuilds().GetTalentTooltips(playerTalentList.TalentName16);
            TalentTooltip talent20 = HeroesIcons.HeroBuilds().GetTalentTooltips(playerTalentList.TalentName20);

            TalentShortTooltips.Add($"<c val=\"FFFFFF\">{TalentNames[0]}:</c> {talent1.Short}");
            TalentShortTooltips.Add($"<c val=\"FFFFFF\">{TalentNames[1]}:</c> {talent4.Short}");
            TalentShortTooltips.Add($"<c val=\"FFFFFF\">{TalentNames[2]}:</c> {talent7.Short}");
            TalentShortTooltips.Add($"<c val=\"FFFFFF\">{TalentNames[3]}:</c> {talent10.Short}");
            TalentShortTooltips.Add($"<c val=\"FFFFFF\">{TalentNames[4]}:</c> {talent13.Short}");
            TalentShortTooltips.Add($"<c val=\"FFFFFF\">{TalentNames[5]}:</c> {talent16.Short}");
            TalentShortTooltips.Add($"<c val=\"FFFFFF\">{TalentNames[6]}:</c> {talent20.Short}");

            TalentFullTooltips.Add(talent1.Full);
            TalentFullTooltips.Add(talent4.Full);
            TalentFullTooltips.Add(talent7.Full);
            TalentFullTooltips.Add(talent10.Full);
            TalentFullTooltips.Add(talent13.Full);
            TalentFullTooltips.Add(talent16.Full);
            TalentFullTooltips.Add(talent20.Full);

            TalentSubInfo.Add(talent1.GetTalentSubInfo());
            TalentSubInfo.Add(talent4.GetTalentSubInfo());
            TalentSubInfo.Add(talent7.GetTalentSubInfo());
            TalentSubInfo.Add(talent10.GetTalentSubInfo());
            TalentSubInfo.Add(talent13.GetTalentSubInfo());
            TalentSubInfo.Add(talent16.GetTalentSubInfo());
            TalentSubInfo.Add(talent20.GetTalentSubInfo());
        }

        public override void Dispose()
        {
            Talents.ForEach((talentImage) => talentImage = null);
            Talents.Clear();
            TalentNames.Clear();
            TalentShortTooltips.Clear();
            TalentFullTooltips.Clear();
            TalentSubInfo.Clear();

            base.Dispose();
        }
    }
}
