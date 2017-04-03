using System;

namespace HeroesMatchData.Core
{
    public static class Utilities
    {
        public static int CalculateWinPercentage(int wins, double total)
        {
            return total != 0 ? (int)(Math.Round(wins / total, 2) * 100) : 0;
        }
    }
}
