using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace Heroes.Helpers
{
    public enum FilterGameTimeOption
    {
        Any,
        [Description("Less than 10 Minutes")]
        LessThan10Minutes,
        [Description("Less than 15 Minutes")]
        LessThan15Minutes,
        [Description("Less than 20 Minutes")]
        LessThan20Minutes,
        [Description("Less than 25 Minutes")]
        LessThan25Minutes,
        [Description("Less than 30 Minutes")]
        LessThan30Minutes,
        [Description("Less than 35 Minutes")]
        LessThan35Minutes,
        [Description("Less than 40 Minutes")]
        LessThan40Minutes,
        [Description("Longer than 10 Minutes")]
        LongerThan10Minutes,
        [Description("Longer than 15 Minutes")]
        LongerThan15Minutes,
        [Description("Longer than 20 Minutes")]
        LongerThan20Minutes,
        [Description("Longer than 25 Minutes")]
        LongerThan25Minutes,
        [Description("Longer than 30 Minutes")]
        LongerThan30Minutes,
        [Description("Longer than 35 Minutes")]
        LongerThan35Minutes,
        [Description("Longer than 40 Minutes")]
        LongerThan40Minutes,
    }

    public enum FilterGameDateOption
    {
        Any,
        [Description("Last 7 Days")]
        Last7Days,
        [Description("Last 14 Days")]
        Last14Days,
        [Description("Last 21 Days")]
        Last21Days,
        [Description("Last 28 Days")]
        Last28Days,
        [Description("More than 7 Days")]
        MoreThan7Days,
        [Description("More than 14 Days")]
        MoreThan14Days,
        [Description("More than 21 Days")]
        MoreThan21Days,
        [Description("More than 28 Days")]
        MoreThan28Days,
    }

    public static partial class HeroesHelpers
    {
        public static class GameDateFilters
        {
            private static readonly string LessThan = "less than";
            private static readonly string LongerThan = "longer than";
            private static readonly string Last = "last";
            private static readonly string MoreThan = "more than";

            public static List<string> GameTimeList => SetGameTimeList();
            public static List<string> GameDateList => SetGameDateList();

            public static Tuple<string, TimeSpan> GetGameTimeModifiedTime(FilterGameTimeOption option)
            {
                Tuple<string, TimeSpan> tuple;

                switch (option)
                {
                    case FilterGameTimeOption.LessThan10Minutes:
                        tuple = new Tuple<string, TimeSpan>(LessThan, TimeSpan.FromMinutes(10));
                        break;
                    case FilterGameTimeOption.LessThan15Minutes:
                        tuple = new Tuple<string, TimeSpan>(LessThan, TimeSpan.FromMinutes(15));
                        break;
                    case FilterGameTimeOption.LessThan20Minutes:
                        tuple = new Tuple<string, TimeSpan>(LessThan, TimeSpan.FromMinutes(20));
                        break;
                    case FilterGameTimeOption.LessThan25Minutes:
                        tuple = new Tuple<string, TimeSpan>(LessThan, TimeSpan.FromMinutes(25));
                        break;
                    case FilterGameTimeOption.LessThan30Minutes:
                        tuple = new Tuple<string, TimeSpan>(LessThan, TimeSpan.FromMinutes(30));
                        break;
                    case FilterGameTimeOption.LessThan35Minutes:
                        tuple = new Tuple<string, TimeSpan>(LessThan, TimeSpan.FromMinutes(30));
                        break;
                    case FilterGameTimeOption.LessThan40Minutes:
                        tuple = new Tuple<string, TimeSpan>(LessThan, TimeSpan.FromMinutes(30));
                        break;
                    case FilterGameTimeOption.LongerThan10Minutes:
                        tuple = new Tuple<string, TimeSpan>(LongerThan, TimeSpan.FromMinutes(10));
                        break;
                    case FilterGameTimeOption.LongerThan15Minutes:
                        tuple = new Tuple<string, TimeSpan>(LongerThan, TimeSpan.FromMinutes(15));
                        break;
                    case FilterGameTimeOption.LongerThan20Minutes:
                        tuple = new Tuple<string, TimeSpan>(LongerThan, TimeSpan.FromMinutes(20));
                        break;
                    case FilterGameTimeOption.LongerThan25Minutes:
                        tuple = new Tuple<string, TimeSpan>(LongerThan, TimeSpan.FromMinutes(25));
                        break;
                    case FilterGameTimeOption.LongerThan30Minutes:
                        tuple = new Tuple<string, TimeSpan>(LongerThan, TimeSpan.FromMinutes(30));
                        break;
                    case FilterGameTimeOption.LongerThan35Minutes:
                        tuple = new Tuple<string, TimeSpan>(LongerThan, TimeSpan.FromMinutes(35));
                        break;
                    case FilterGameTimeOption.LongerThan40Minutes:
                        tuple = new Tuple<string, TimeSpan>(LongerThan, TimeSpan.FromMinutes(40));
                        break;
                    default:
                        tuple = new Tuple<string, TimeSpan>(null, TimeSpan.FromSeconds(0));
                        break;
                }

                return tuple;
            }

            public static Tuple<string, DateTime?> GetGameDateModifiedDate(FilterGameDateOption option)
            {
                Tuple<string, DateTime?> tuple;

                switch (option)
                {
                    case FilterGameDateOption.Last7Days:
                        tuple = new Tuple<string, DateTime?>(Last, DateTime.Now.AddDays(-7));
                        break;
                    case FilterGameDateOption.Last14Days:
                        tuple = new Tuple<string, DateTime?>(Last, DateTime.Now.AddDays(-14));
                        break;
                    case FilterGameDateOption.Last21Days:
                        tuple = new Tuple<string, DateTime?>(Last, DateTime.Now.AddDays(-21));
                        break;
                    case FilterGameDateOption.Last28Days:
                        tuple = new Tuple<string, DateTime?>(Last, DateTime.Now.AddDays(-28));
                        break;
                    case FilterGameDateOption.MoreThan7Days:
                        tuple = new Tuple<string, DateTime?>(MoreThan, DateTime.Now.AddDays(-7));
                        break;
                    case FilterGameDateOption.MoreThan14Days:
                        tuple = new Tuple<string, DateTime?>(MoreThan, DateTime.Now.AddDays(-14));
                        break;
                    case FilterGameDateOption.MoreThan21Days:
                        tuple = new Tuple<string, DateTime?>(MoreThan, DateTime.Now.AddDays(-21));
                        break;
                    case FilterGameDateOption.MoreThan28Days:
                        tuple = new Tuple<string, DateTime?>(MoreThan, DateTime.Now.AddDays(-28));
                        break;
                    default:
                        tuple = new Tuple<string, DateTime?>(null, DateTime.Now.AddDays(0));
                        break;
                }

                return tuple;
            }

            private static List<string> SetGameTimeList()
            {
                List<string> list = new List<string>();

                foreach (FilterGameTimeOption option in Enum.GetValues(typeof(FilterGameTimeOption)))
                {
                    list.Add(option.GetFriendlyName());
                }

                return list;
            }

            private static List<string> SetGameDateList()
            {
                List<string> list = new List<string>();

                foreach (FilterGameDateOption option in Enum.GetValues(typeof(FilterGameDateOption)))
                {
                    list.Add(option.GetFriendlyName());
                }

                return list;
            }
        }
    }
}
