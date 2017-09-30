using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace Heroes.Helpers
{
    public enum OverviewHeroStatOption
    {
        [Description("Highest Winrate")]
        HighestWinrate,
        [Description("Most Wins")]
        MostWins,
        [Description("Most Deaths")]
        MostDeaths,
        [Description("Most Kills")]
        MostKills,
        [Description("Most Assists")]
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
