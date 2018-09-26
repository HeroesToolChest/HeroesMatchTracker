using System;

namespace Heroes.Helpers
{
    public static partial class HeroesHelpers
    {
        public static class Builds
        {
            /// <summary>
            /// Item1 is the beginning build (inclusive) and Item2 is the end (exclusive).
            /// </summary>
            /// <param name="season">The selected season.</param>
            /// <returns></returns>
            public static Tuple<int?, int?> GetReplayBuildsFromSeason(Season season)
            {
                switch (season)
                {
                    case Season.Year2018Season4:
                        return new Tuple<int?, int?>(68740, 99999);
                    case Season.Year2018Season3:
                        return new Tuple<int?, int?>(66488, 68740);
                    case Season.Year2018Season2:
                        return new Tuple<int?, int?>(62833, 66488);
                    case Season.Year2018Season1:
                        return new Tuple<int?, int?>(60399, 62833);
                    case Season.Year2017Season3:
                        return new Tuple<int?, int?>(57062, 60399);
                    case Season.Year2017Season2:
                        return new Tuple<int?, int?>(54339, 57062);
                    case Season.Year2017Season1:
                        return new Tuple<int?, int?>(51375, 54339);
                    case Season.Year2016Season3:
                        return new Tuple<int?, int?>(48760, 51375);
                    case Season.Year2016Season2:
                        return new Tuple<int?, int?>(45949, 48760);
                    case Season.Year2016Season1:
                        return new Tuple<int?, int?>(43571, 45949);
                    case Season.Preseason:
                        return new Tuple<int?, int?>(0, 43571);
                    case Season.Lifetime:
                        return new Tuple<int?, int?>(0, 99999);
                    default:
                        return null;
                }
            }
        }
    }
}
