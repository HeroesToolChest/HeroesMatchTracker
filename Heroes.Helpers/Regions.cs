using System.Collections.Generic;

namespace Heroes.Helpers
{
    public static partial class HeroesHelpers
    {
        public static class Regions
        {
            public enum Region
            {
                US = 1,
                EU = 2,
                KR = 3,
                CN = 5,
                XX = 99,
            }

            public static List<string> GetRegionsList()
            {
                List<string> regions = new List<string>();
                regions.Add("US");
                regions.Add("EU");
                regions.Add("KR");
                regions.Add("CN");
                regions.Add("XX");

                return regions;
            }
        }
    }
}
