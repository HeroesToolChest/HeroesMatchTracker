using Heroes.Helpers;

namespace Heroes.Icons.Models
{
    public class Ability : TalentBase
    {
        public Ability() { }

        public Ability(TalentBase talentBase)
        {
            Name = talentBase.Name;
            ReferenceName = talentBase.ReferenceName;
            IsIconGeneric = talentBase.IsIconGeneric;
            IsGeneric = talentBase.IsGeneric;
            TooltipDescriptionName = talentBase.TooltipDescriptionName;
            Icon = talentBase.Icon;
            Tooltip = talentBase.Tooltip;
        }

        public AbilityTier Tier { get; set; }

        /// <summary>
        /// Gets the ability parent that is associated with this ability
        /// </summary>
        public string ParentLink { get; set; }

        public override string ToString() => $"{Tier.GetFriendlyName()} | {ReferenceName}";
    }
}
