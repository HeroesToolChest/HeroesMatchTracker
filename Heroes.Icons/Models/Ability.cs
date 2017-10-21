using Heroes.Helpers;

namespace Heroes.Icons.Models
{
    public class Ability : TalentBase
    {
        public AbilityTier Tier { get; set; }

        public override string ToString() => $"{Tier.GetFriendlyName()} | {ReferenceName}";
    }
}
