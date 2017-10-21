using System;
using System.ComponentModel;

namespace Heroes.Icons.Models
{
    [Flags]
    public enum TalentTier
    {
        Old = 0,
        [Description("Level 1")]
        Level1 = 1 << 0,
        [Description("Level 4")]
        Level4 = 1 << 1,
        [Description("Level 7")]
        Level7 = 1 << 2,
        [Description("Level 10")]
        Level10 = 1 << 3,
        [Description("Level 13")]
        Level13 = 1 << 4,
        [Description("Level 16")]
        Level16 = 1 << 5,
        [Description("Level 20")]
        Level20 = 1 << 6,
    }
}
