using System;
using System.Collections.Generic;

namespace Heroes.Helpers
{
    [Flags]
    public enum Season
    {
        Lifetime = 0,
        Preseason = 1 << 0,
        Year2016Season1 = 1 << 1,
        Year2016Season2 = 1 << 2,
        Year2016Season3 = 1 << 3,
        Year2017Season1 = 1 << 4,
    }

    public static partial class HeroesHelpers
    {
        public static class Seasons
        {
            public static List<string> GetSeasonList()
            {
                List<string> list = new List<string>
                {
                    "Preseason",
                    "2016 Season 1",
                    "2016 Season 2",
                    "2016 Season 3",
                    "2017 Season 1",
                };

                return list;
            }
        }
    }
}
