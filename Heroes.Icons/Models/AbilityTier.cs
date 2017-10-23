using System;
using System.ComponentModel;

namespace Heroes.Icons.Models
{
    [Flags]
    public enum AbilityTier
    {
        [Description("Basic Ability 1")]
        BasicAbility1 = 1 << 0,
        [Description("Basic Ability 2")]
        BasicAbility2 = 1 << 1,
        [Description("Basic Ability 3")]
        BasicAbility3 = 1 << 2,
        [Description("Heroic Ability")]
        HeroicAbility = 1 << 3,
        [Description("Trait Ability")]
        TraitAbility = 1 << 4,
        [Description("Mount Ability")]
        MountAbility = 1 << 5,
        [Description("Activable Ability")]
        ActivableAbility = 1 << 6,
    }
}
