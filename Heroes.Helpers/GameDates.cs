using System;
using System.Collections.Generic;

namespace Heroes.Helpers
{
    public static partial class HeroesHelpers
    {
        public static class GameDates
        {
            public static List<string> GameTimeList => SetGameTimeList();
            public static List<string> GameDateList => SetGameDateList();
            public static List<string> StatGameDateList => SetStatGameDateList();

            public static Tuple<string, TimeSpan> GetGameTimeModifiedTime(string value)
            {
                Tuple<string, TimeSpan> tuple;

                switch (value)
                {
                    case "less than 10 minutes":
                        tuple = new Tuple<string, TimeSpan>("less than", TimeSpan.FromMinutes(10));
                        break;
                    case "less than 15 minutes":
                        tuple = new Tuple<string, TimeSpan>("less than", TimeSpan.FromMinutes(15));
                        break;
                    case "less than 20 minutes":
                        tuple = new Tuple<string, TimeSpan>("less than", TimeSpan.FromMinutes(20));
                        break;
                    case "less than 25 minutes":
                        tuple = new Tuple<string, TimeSpan>("less than", TimeSpan.FromMinutes(25));
                        break;
                    case "less than 30 minutes":
                        tuple = new Tuple<string, TimeSpan>("less than", TimeSpan.FromMinutes(30));
                        break;
                    case "less than 35 minutes":
                        tuple = new Tuple<string, TimeSpan>("less than", TimeSpan.FromMinutes(30));
                        break;
                    case "less than 40 minutes":
                        tuple = new Tuple<string, TimeSpan>("less than", TimeSpan.FromMinutes(30));
                        break;
                    case "longer than 10 minutes":
                        tuple = new Tuple<string, TimeSpan>("longer than", TimeSpan.FromMinutes(10));
                        break;
                    case "longer than 15 minutes":
                        tuple = new Tuple<string, TimeSpan>("longer than", TimeSpan.FromMinutes(15));
                        break;
                    case "longer than 20 minutes":
                        tuple = new Tuple<string, TimeSpan>("longer than", TimeSpan.FromMinutes(20));
                        break;
                    case "longer than 25 minutes":
                        tuple = new Tuple<string, TimeSpan>("longer than", TimeSpan.FromMinutes(25));
                        break;
                    case "longer than 30 minutes":
                        tuple = new Tuple<string, TimeSpan>("longer than", TimeSpan.FromMinutes(30));
                        break;
                    case "longer than 35 minutes":
                        tuple = new Tuple<string, TimeSpan>("longer than", TimeSpan.FromMinutes(35));
                        break;
                    case "longer than 40 minutes":
                        tuple = new Tuple<string, TimeSpan>("longer than", TimeSpan.FromMinutes(40));
                        break;
                    default:
                        tuple = new Tuple<string, TimeSpan>(null, TimeSpan.FromSeconds(0));
                        break;
                }

                return tuple;
            }

            public static Tuple<string, DateTime?> GetGameDateModifiedDate(string value)
            {
                Tuple<string, DateTime?> tuple;

                switch (value)
                {
                    case "Last 7 days":
                        tuple = new Tuple<string, DateTime?>("last", DateTime.Now.AddDays(-7));
                        break;
                    case "Last 14 days":
                        tuple = new Tuple<string, DateTime?>("last", DateTime.Now.AddDays(-14));
                        break;
                    case "Last 21 days":
                        tuple = new Tuple<string, DateTime?>("last", DateTime.Now.AddDays(-21));
                        break;
                    case "Last 28 days":
                        tuple = new Tuple<string, DateTime?>("last", DateTime.Now.AddDays(-28));
                        break;
                    case "More than 7 days ago":
                        tuple = new Tuple<string, DateTime?>("more than", DateTime.Now.AddDays(-7));
                        break;
                    case "More than 14 days ago":
                        tuple = new Tuple<string, DateTime?>("more than", DateTime.Now.AddDays(-14));
                        break;
                    case "More than 21 days ago":
                        tuple = new Tuple<string, DateTime?>("more than", DateTime.Now.AddDays(-21));
                        break;
                    case "More than 28 days ago":
                        tuple = new Tuple<string, DateTime?>("more than", DateTime.Now.AddDays(-28));
                        break;
                    default:
                        tuple = new Tuple<string, DateTime?>("null", DateTime.Now.AddDays(0));
                        break;
                }

                return tuple;
            }

            public static Tuple<string, DateTime?> GetStatGameDateModifiedDate(string value)
            {
                return GetGameDateModifiedDate(value);
            }

            private static List<string> SetGameTimeList()
            {
                List<string> list = new List<string>
                {
                    "Any",
                    "less than 10 minutes",
                    "less than 15 minutes",
                    "less than 20 minutes",
                    "less than 25 minutes",
                    "less than 30 minutes",
                    "less than 35 minutes",
                    "less than 40 minutes",
                    "longer than 10 minutes",
                    "longer than 15 minutes",
                    "longer than 20 minutes",
                    "longer than 25 minutes",
                    "longer than 30 minutes",
                    "longer than 35 minutes",
                    "longer than 40 minutes",
                };
                return list;
            }

            private static List<string> SetGameDateList()
            {
                List<string> list = new List<string>
                {
                    "Any",
                    "Last 7 days",
                    "Last 14 days",
                    "Last 21 days",
                    "Last 28 days",
                    "More than 7 days ago",
                    "More than 14 days ago",
                    "More than 21 days ago",
                    "More than 28 days ago",
                };
                return list;
            }

            private static List<string> SetStatGameDateList()
            {
                List<string> list = new List<string>
                {
                    "Last build",
                    "Last 7 days",
                    "Last 14 days",
                    "Last 21 days",
                    "Last 28 days",
                    "More than 7 days ago",
                    "More than 14 days ago",
                    "More than 21 days ago",
                    "More than 28 days ago",
                };
                return list;
            }
        }
    }
}
