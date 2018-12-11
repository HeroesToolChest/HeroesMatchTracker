using Heroes.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace Heroes.Helpers
{
    [Flags]
    public enum Season
    {
        [Description("Lifetime")]
        Lifetime = 0,
        [Description("Preseason")]
        Preseason = 1 << 0,
        [Description("Year 2016 Season 1")]
        Year2016Season1 = 1 << 1,
        [Description("Year 2016 Season 2")]
        Year2016Season2 = 1 << 2,
        [Description("Year 2016 Season 3")]
        Year2016Season3 = 1 << 3,
        [Description("Year 2017 Season 1")]
        Year2017Season1 = 1 << 4,
        [Description("Year 2017 Season 2")]
        Year2017Season2 = 1 << 5,
        [Description("Year 2017 Season 3")]
        Year2017Season3 = 1 << 6,
        [Description("Year 2018 Season 1")]
        Year2018Season1 = 1 << 7,
        [Description("Year 2018 Season 2")]
        Year2018Season2 = 1 << 8,
        [Description("Year 2018 Season 3")]
        Year2018Season3 = 1 << 9,
        [Description("Year 2018 Season 4")]
        Year2018Season4 = 1 << 10,
        [Description("Year 2018 Season 5")]
        Year2018Season5 = 1 << 11,
    }

    public static partial class HeroesHelpers
    {
        public static class Seasons
        {
            public static List<string> GetSeasonList()
            {
                List<string> list = new List<string>();

                foreach (Season season in Enum.GetValues(typeof(Season)))
                {
                    list.Add(season.GetFriendlyName());
                }

                return list;
            }
        }
    }
}
