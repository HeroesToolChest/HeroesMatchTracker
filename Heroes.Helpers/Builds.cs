﻿using System;

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
                    case Season.StormLeague2021Season2:
                        return new Tuple<int?, int?>(85027, 99999);
                    case Season.StormLeague2021Season1:
                        return new Tuple<int?, int?>(83004, 85027);
                    case Season.StormLeague2020Season4:
                        return new Tuple<int?, int?>(81700, 83004);
                    case Season.StormLeague2020Season3:
                        return new Tuple<int?, int?>(80333, 81700);
                    case Season.StormLeague2020Season2:
                        return new Tuple<int?, int?>(79155, 80333);
                    case Season.StormLeague2020Season1:
                        return new Tuple<int?, int?>(77525, 79155);
                    case Season.StormLeague2019Season3:
                        return new Tuple<int?, int?>(75589, 77525);
                    case Season.StormLeaguePreseason:
                        return new Tuple<int?, int?>(73016, 75589);
                    case Season.Year2019Season1:
                        return new Tuple<int?, int?>(70920, 73016);
                    case Season.Year2018Season4:
                        return new Tuple<int?, int?>(68740, 70920);
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
