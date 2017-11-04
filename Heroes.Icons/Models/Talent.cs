using Heroes.Helpers;

namespace Heroes.Icons.Models
{
    public class Talent : TalentBase
    {
        public Talent() { }

        public Talent(TalentBase talentBase)
        {
            Name = talentBase.Name;
            ReferenceName = talentBase.ReferenceName;
            IsIconGeneric = talentBase.IsIconGeneric;
            IsGeneric = talentBase.IsGeneric;
            TooltipDescriptionName = talentBase.TooltipDescriptionName;
            Icon = talentBase.Icon;
            Tooltip = talentBase.Tooltip;
        }

        public TalentTier Tier { get; set; }

        public override string ToString() => $"{Tier.GetFriendlyName()} | {ReferenceName}";
    }
}
