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

            private static List<string> SetGameTimeList()
            {
                List<string> list = new List<string>();

                list.Add("Any");
                list.Add("less than 10 minutes");
                list.Add("less than 15 minutes");
                list.Add("less than 20 minutes");
                list.Add("less than 25 minutes");
                list.Add("less than 30 minutes");
                list.Add("less than 35 minutes");
                list.Add("less than 40 minutes");
                list.Add("longer than 10 minutes");
                list.Add("longer than 15 minutes");
                list.Add("longer than 20 minutes");
                list.Add("longer than 25 minutes");
                list.Add("longer than 30 minutes");
                list.Add("longer than 35 minutes");
                list.Add("longer than 40 minutes");

                return list;
            }

            private static List<string> SetGameDateList()
            {
                List<string> list = new List<string>();

                list.Add("Any");
                list.Add("Last 7 days");
                list.Add("Last 14 days");
                list.Add("Last 21 days");
                list.Add("Last 28 days");
                list.Add("More than 7 days ago");
                list.Add("More than 14 days ago");
                list.Add("More than 21 days ago");
                list.Add("More than 28 days ago");

                return list;
            }
        }
    }
}
