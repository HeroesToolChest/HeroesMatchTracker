using System;

namespace Heroes.Icons
{
    [Flags]
    public enum TalentTier : uint
    {
        Old = 0,
        Level1 = 1 << 0,
        Level4 = 1 << 1,
        Level7 = 1 << 2,
        Level10 = 1 << 3,
        Level13 = 1 << 4,
        Level16 = 1 << 5,
        Level20 = 1 << 6,
    }
}
