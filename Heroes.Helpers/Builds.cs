using System;
using System.Collections.Generic;

namespace Heroes.Helpers
{
    public static partial class HeroesHelpers
    {
        public static class Builds
        {
            /// <summary>
            /// Item1 is the beginning build (inclusive) and Item2 is the end (exclusive)
            /// </summary>
            /// <param name="season">The selected season</param>
            /// <returns></returns>
            public static Tuple<int?, int?> GetReplayBuildsFromSeason(Season season)
            {
                switch (season)
                {
                    case Season.Season3:
                        return new Tuple<int?, int?>(48760, 99999);
                    case Season.Season2:
                        return new Tuple<int?, int?>(45949, 48760);
                    case Season.Season1:
                        return new Tuple<int?, int?>(43571, 45949);
                    case Season.Preseason:
                        return new Tuple<int?, int?>(0, 43571);
                    case Season.Lifetime:
                        return new Tuple<int?, int?>(0, 99999);
                    default:
                        return null;
                }
            }

            public static List<string> GetBuildsList()
            {
                List<string> list = HeroesInfo.HeroesIcons.GetListOfHeroesBuilds().ConvertAll(x => x.ToString());

                list.Add("47219");
                list.Add("47133");
                list.Add("46889");
                list.Add("46869");
                list.Add("46787");
                list.Add("46690");
                list.Add("46446");
                list.Add("46158");
                list.Add("45635");
                list.Add("45228");
                list.Add("44941");
                list.Add("44797");
                list.Add("44468");
                list.Add("44468");
                list.Add("44124");
                list.Add("43905");
                list.Add("43571");
                list.Add("43259");
                list.Add("43170");
                list.Add("43051");
                list.Add("42958");
                list.Add("42590");
                list.Add("42506");
                list.Add("42406");
                list.Add("42273");
                list.Add("42178");
                list.Add("41810");
                list.Add("41504");
                list.Add("41393");
                list.Add("41150");
                list.Add("40798");
                list.Add("40697");
                list.Add("40431");
                list.Add("40322");
                list.Add("40087");
                list.Add("39951");
                list.Add("39709");
                list.Add("39595");
                list.Add("39445");

                // end here, no need to add any earlier builds
                return list;
            }
        }
    }
}
