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
                List<string> list = new List<string>
                {
                    "Highest Win Rate As",
                    "Most Wins as",
                    "Most Deaths as",
                    "Most Kills as",
                    "Most Assists as",
                };

                return list;
            }
        }
    }
}
