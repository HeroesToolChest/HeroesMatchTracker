using System;

namespace Heroes.Icons
{
    [Flags]
    public enum PartyIconColor : uint
    {
        Purple = 0,
        Yellow = 1 << 1,
        Brown = 1 << 2,
        Teal = 1 << 3,
    }
}
