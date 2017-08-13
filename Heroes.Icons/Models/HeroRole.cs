using System;

namespace Heroes.Icons
{
    [Flags]
    public enum HeroRole
    {
        Unknown = 0,
        Multiclass = 1 << 0,
        Warrior = 1 << 1,
        Assassin = 1 << 2,
        Support = 1 << 3,
        Specialist = 1 << 4,
    }
}
