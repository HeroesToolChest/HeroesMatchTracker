using System;
using System.Collections.Generic;

namespace Heroes.Helpers
{
    public enum OverviewHeroStatOption
    {
        HighestWinRate,
        MostWins,
        MostDeaths,
        MostKills,
        MostAssists,
    }

    public static partial class HeroesHelpers
    {
        public static class OverviewHeroStatOptions
        {
            public static List<string> GetOverviewHeroStatOptionList()
            {
                List<string> list = new List<string>();

                foreach (OverviewHeroStatOption option in Enum.GetValues(typeof(OverviewHeroStatOption)))
                {
                    list.Add(option.GetFriendlyName());
                }

                return list;
            }
        }
    }
}
