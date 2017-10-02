using Heroes.Icons.Models;
using HeroesMatchTracker.Core.Services;
using HeroesMatchTracker.Data.Models.Replays;
using System;
using System.Collections.Generic;

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

        public List<Uri> Talents { get; private set; } = new List<Uri>();
        public List<string> TalentNames { get; private set; } = new List<string>();
        public List<string> TalentShortTooltips { get; private set; } = new List<string>();
        public List<string> TalentFullTooltips { get; private set; } = new List<string>();
        public List<string> TalentSubInfo { get; private set; } = new List<string>();

        public void SetTalents(ReplayMatchPlayerTalent playerTalentList)
        {
            var talent1 = HeroesIcons.HeroBuilds().GetHeroTalent(playerTalentList.Character, TalentTier.Level1, playerTalentList.TalentName1);
            var talent4 = HeroesIcons.HeroBuilds().GetHeroTalent(playerTalentList.Character, TalentTier.Level4, playerTalentList.TalentName4);
            var talent7 = HeroesIcons.HeroBuilds().GetHeroTalent(playerTalentList.Character, TalentTier.Level7, playerTalentList.TalentName7);
            var talent10 = HeroesIcons.HeroBuilds().GetHeroTalent(playerTalentList.Character, TalentTier.Level10, playerTalentList.TalentName10);
            var talent13 = HeroesIcons.HeroBuilds().GetHeroTalent(playerTalentList.Character, TalentTier.Level13, playerTalentList.TalentName13);
            var talent16 = HeroesIcons.HeroBuilds().GetHeroTalent(playerTalentList.Character, TalentTier.Level16, playerTalentList.TalentName16);
            var talent20 = HeroesIcons.HeroBuilds().GetHeroTalent(playerTalentList.Character, TalentTier.Level20, playerTalentList.TalentName20);

            Talents.Add(talent1.Icon);
            Talents.Add(talent4.Icon);
            Talents.Add(talent7.Icon);
            Talents.Add(talent10.Icon);
            Talents.Add(talent13.Icon);
            Talents.Add(talent16.Icon);
            Talents.Add(talent20.Icon);

            TalentNames.Add(talent1.Name);
            TalentNames.Add(talent4.Name);
            TalentNames.Add(talent7.Name);
            TalentNames.Add(talent10.Name);
            TalentNames.Add(talent13.Name);
            TalentNames.Add(talent16.Name);
            TalentNames.Add(talent20.Name);

            TalentShortTooltips.Add($"<c val=\"FFFFFF\">{talent1.Name}:</c> {talent1.Tooltip.Short}");
            TalentShortTooltips.Add($"<c val=\"FFFFFF\">{talent4.Name}:</c> {talent1.Tooltip.Short}");
            TalentShortTooltips.Add($"<c val=\"FFFFFF\">{talent7.Name}:</c> {talent1.Tooltip.Short}");
            TalentShortTooltips.Add($"<c val=\"FFFFFF\">{talent10.Name}:</c> {talent1.Tooltip.Short}");
            TalentShortTooltips.Add($"<c val=\"FFFFFF\">{talent13.Name}:</c> {talent1.Tooltip.Short}");
            TalentShortTooltips.Add($"<c val=\"FFFFFF\">{talent16.Name}:</c> {talent1.Tooltip.Short}");
            TalentShortTooltips.Add($"<c val=\"FFFFFF\">{talent20.Name}:</c> {talent1.Tooltip.Short}");

            TalentFullTooltips.Add(talent1.Tooltip.Full);
            TalentFullTooltips.Add(talent4.Tooltip.Full);
            TalentFullTooltips.Add(talent7.Tooltip.Full);
            TalentFullTooltips.Add(talent10.Tooltip.Full);
            TalentFullTooltips.Add(talent13.Tooltip.Full);
            TalentFullTooltips.Add(talent16.Tooltip.Full);
            TalentFullTooltips.Add(talent20.Tooltip.Full);

            TalentSubInfo.Add(talent1.Tooltip.GetTalentSubInfo());
            TalentSubInfo.Add(talent4.Tooltip.GetTalentSubInfo());
            TalentSubInfo.Add(talent7.Tooltip.GetTalentSubInfo());
            TalentSubInfo.Add(talent10.Tooltip.GetTalentSubInfo());
            TalentSubInfo.Add(talent13.Tooltip.GetTalentSubInfo());
            TalentSubInfo.Add(talent16.Tooltip.GetTalentSubInfo());
            TalentSubInfo.Add(talent20.Tooltip.GetTalentSubInfo());
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
