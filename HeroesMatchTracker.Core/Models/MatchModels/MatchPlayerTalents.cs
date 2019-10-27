using Heroes.Icons;
using Heroes.Models.AbilityTalents;
using HeroesMatchTracker.Core.Services;
using HeroesMatchTracker.Data.Models.Replays;
using System.Collections.Generic;
using System.IO;

namespace HeroesMatchTracker.Core.Models.MatchModels
{
    public class MatchPlayerTalents : MatchPlayerBase
    {
        public MatchPlayerTalents(MatchPlayerBase matchPlayerBase)
            : base(matchPlayerBase)
        { }

        public MatchPlayerTalents(IInternalService internalService, IWebsiteService website, ReplayMatchPlayer player, int build)
            : base(internalService, website, player, build)
        { }

        public List<Stream> Talents { get; private set; } = new List<Stream>();
        public List<string> TalentNames { get; private set; } = new List<string>();
        public List<string> TalentShortTooltips { get; private set; } = new List<string>();
        public List<string> TalentFullTooltips { get; private set; } = new List<string>();
        public List<string> TalentSubInfo { get; private set; } = new List<string>();

        public void SetTalents(ReplayMatchPlayerTalent playerTalentList)
        {
            Talent talent1 = HeroesIcons.HeroesData(Build).HeroData(playerTalentList.Character).GetTalent(playerTalentList.TalentName1);
            Talent talent4 = HeroesIcons.HeroesData(Build).HeroData(playerTalentList.Character).GetTalent(playerTalentList.TalentName4);
            Talent talent7 = HeroesIcons.HeroesData(Build).HeroData(playerTalentList.Character).GetTalent(playerTalentList.TalentName7);
            Talent talent10 = HeroesIcons.HeroesData(Build).HeroData(playerTalentList.Character).GetTalent(playerTalentList.TalentName10);
            Talent talent13 = HeroesIcons.HeroesData(Build).HeroData(playerTalentList.Character).GetTalent(playerTalentList.TalentName13);
            Talent talent16 = HeroesIcons.HeroesData(Build).HeroData(playerTalentList.Character).GetTalent(playerTalentList.TalentName16);
            Talent talent20 = HeroesIcons.HeroesData(Build).HeroData(playerTalentList.Character).GetTalent(playerTalentList.TalentName20);

            Talents.Add(talent1.AbilityTalentImage());
            Talents.Add(talent4.AbilityTalentImage());
            Talents.Add(talent7.AbilityTalentImage());
            Talents.Add(talent10.AbilityTalentImage());
            Talents.Add(talent13.AbilityTalentImage());
            Talents.Add(talent16.AbilityTalentImage());
            Talents.Add(talent20.AbilityTalentImage());

            TalentNames.Add(talent1.Name);
            TalentNames.Add(talent4.Name);
            TalentNames.Add(talent7.Name);
            TalentNames.Add(talent10.Name);
            TalentNames.Add(talent13.Name);
            TalentNames.Add(talent16.Name);
            TalentNames.Add(talent20.Name);

            TalentShortTooltips.Add($"<c val=\"FFFFFF\">{talent1.Name}:</c> {talent1.Tooltip?.ShortTooltip?.ColoredText}");
            TalentShortTooltips.Add($"<c val=\"FFFFFF\">{talent4.Name}:</c> {talent4.Tooltip?.ShortTooltip?.ColoredText}");
            TalentShortTooltips.Add($"<c val=\"FFFFFF\">{talent7.Name}:</c> {talent7.Tooltip?.ShortTooltip?.ColoredText}");
            TalentShortTooltips.Add($"<c val=\"FFFFFF\">{talent10.Name}:</c> {talent10.Tooltip?.ShortTooltip?.ColoredText}");
            TalentShortTooltips.Add($"<c val=\"FFFFFF\">{talent13.Name}:</c> {talent13.Tooltip?.ShortTooltip?.ColoredText}");
            TalentShortTooltips.Add($"<c val=\"FFFFFF\">{talent16.Name}:</c> {talent16.Tooltip?.ShortTooltip?.ColoredText}");
            TalentShortTooltips.Add($"<c val=\"FFFFFF\">{talent20.Name}:</c> {talent20.Tooltip?.ShortTooltip?.ColoredText}");

            TalentFullTooltips.Add(talent1.Tooltip?.FullTooltip?.ColoredText);
            TalentFullTooltips.Add(talent4.Tooltip?.FullTooltip?.ColoredText);
            TalentFullTooltips.Add(talent7.Tooltip?.FullTooltip?.ColoredText);
            TalentFullTooltips.Add(talent10.Tooltip?.FullTooltip?.ColoredText);
            TalentFullTooltips.Add(talent13.Tooltip?.FullTooltip?.ColoredText);
            TalentFullTooltips.Add(talent16.Tooltip?.FullTooltip?.ColoredText);
            TalentFullTooltips.Add(talent20.Tooltip?.FullTooltip?.ColoredText);

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
            Talents.ForEach((talentImage) => talentImage?.Dispose());
            Talents.Clear();
            TalentNames.Clear();
            TalentShortTooltips.Clear();
            TalentFullTooltips.Clear();
            TalentSubInfo.Clear();

            base.Dispose();
        }
    }
}
