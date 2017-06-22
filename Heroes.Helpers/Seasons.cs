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
        Year2017Season2 = 1 << 5,
    }

    public static partial class HeroesHelpers
    {
        public static class Seasons
        {
            public const string LifeTime = "LifeTime";
            public const string Preseason = "Preseason";
            public const string Year2016Season1 = "2016 Season 1";
            public const string Year2016Season2 = "2016 Season 2";
            public const string Year2016Season3 = "2016 Season 3";
            public const string Year2017Season1 = "2017 Season 1";
            public const string Year2017Season2 = "2017 Season 2";

            public static List<string> GetSeasonList()
            {
                List<string> list = new List<string>
                {
                    Preseason,
                    Year2016Season1,
                    Year2016Season2,
                    Year2016Season3,
                    Year2017Season1,
                    Year2017Season2,
                };

                return list;
            }

            public static string GetStringFromSeason(Season season)
            {
                switch (season)
                {
                    case Season.Lifetime:
                        return LifeTime;
                    case Season.Preseason:
                        return Preseason;
                    case Season.Year2016Season1:
                        return Year2016Season1;
                    case Season.Year2016Season2:
                        return Year2016Season2;
                    case Season.Year2016Season3:
                        return Year2016Season3;
                    case Season.Year2017Season1:
                        return Year2017Season1;
                    case Season.Year2017Season2:
                        return Year2017Season2;
                    default:
                        throw new ArgumentException($"paramter {season} not found", nameof(season));
                }
            }
        }
    }
}
