using System;

namespace Heroes.Icons
{
    [Flags]
    public enum HeroRole
    {
        Unknown = -1,
        Multiclass = 0,
        Warrior = 1 << 0,
        Assassin = 1 << 1,
        Support = 1 << 2,
        Specialist = 1 << 3,
    }
}
