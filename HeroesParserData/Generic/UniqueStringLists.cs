using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HeroesParserData
{
    public class UniqueStringLists
    {
        public List<string> GameTimeList { get; private set; } = new List<string>();
        public List<string> GameDateList { get; private set; } = new List<string>();

        public UniqueStringLists()
        {
            SetGameTimeList();
            SetGameDateList();
        }

        private void SetGameTimeList()
        {
            GameTimeList.Add("Any");
            GameTimeList.Add("less than 10 minutes");
            GameTimeList.Add("less than 15 minutes");
            GameTimeList.Add("less than 20 minutes");
            GameTimeList.Add("less than 25 minutes");
            GameTimeList.Add("less than 30 minutes");
            GameTimeList.Add("less than 35 minutes");
            GameTimeList.Add("less than 40 minutes");
            GameTimeList.Add("longer than 10 minutes");
            GameTimeList.Add("longer than 15 minutes");
            GameTimeList.Add("longer than 20 minutes");
            GameTimeList.Add("longer than 25 minutes");
            GameTimeList.Add("longer than 30 minutes");
            GameTimeList.Add("longer than 35 minutes");
            GameTimeList.Add("longer than 40 minutes");
        }

        private void SetGameDateList()
        {
            GameDateList.Add("Any");
            GameDateList.Add("Last 7 days");
            GameDateList.Add("Last 14 days");
            GameDateList.Add("Last 21 days");
            GameDateList.Add("Last 28 days");
            GameDateList.Add("More than 7 days ago");
            GameDateList.Add("More than 14 days ago");
            GameDateList.Add("More than 21 days ago");
            GameDateList.Add("More than 28 days ago");
        }

        public Tuple<string, TimeSpan> GetGameTimeModifiedTime(string value)
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

        public Tuple<string, DateTime?> GetGameDateModifiedDate(string value)
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
    }
}
