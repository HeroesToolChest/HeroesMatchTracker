using System;

namespace HeroesMatchTracker.Core
{
    public static class Utilities
    {
        public static double CalculateWinPercentage(int wins, int total)
        {
            return total != 0 ? Math.Round((wins / (double)total) * 100, 1) : 0;
        }

        /// <summary>
        /// Returns the win value as a double (less than 1)
        /// </summary>
        /// <param name="wins">Wins</param>
        /// <param name="total">Total</param>
        /// <returns></returns>
        public static double CalculateWinValue(int wins, int total)
        {
            return total != 0 ? wins / (double)total : 0;
        }
    }
}
