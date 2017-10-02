using System;

namespace Heroes.Icons.Models
{
    [Flags]
    public enum MVPScreenColor
    {
        Blue = 0,
        Red = 1 << 0,
        Gold = 1 << 1,
    }

    [Flags]
    public enum MVPScoreScreenColor
    {
        Blue = 0,
        Red = 1 << 0,
    }
}
