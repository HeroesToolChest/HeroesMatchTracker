using System;
using System.ComponentModel;

namespace Heroes.Icons.Models
{
    [Flags]
    public enum AbilityTier
    {
        [Description("Basic Ability")]
        Basic,
        [Description("Heroic Ability")]
        Heroic,
        Trait,
        [Description("Mount Ability")]
        Mount,
        [Description("Activable Ability")]
        Activable,
    }
}
