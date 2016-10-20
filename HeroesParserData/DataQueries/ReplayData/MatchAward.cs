using HeroesParserData.Models.DbModels;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.SQLite;
using System.Linq;

namespace HeroesParserData.DataQueries
{
    public static partial class Query
    {
        internal static class MatchAward
        {
            public static long CreateRecord(HeroesParserDataContext db, ReplayMatchAward replayMatchAward)
            {
                db.ReplayMatchAwards.Add(replayMatchAward);
                db.SaveChanges();

                return replayMatchAward.PlayerId;
            }

            public static List<ReplayMatchAward> ReadTopRecords(int num)
            {
                using (var db = new HeroesParserDataContext())
                {
                    return db.ReplayMatchAwards.Take(num).ToList();
                }
            }

            public static List<ReplayMatchAward> ReadLastRecords(int num)
            {
                using (var db = new HeroesParserDataContext())
                {
                    return db.ReplayMatchAwards.OrderByDescending(x => x.ReplayId).Take(num).ToList();
                }
            }

            public static List<ReplayMatchAward> ReadRecordsCustomTop(int count, string columnName, string orderBy)
            {
                if (string.IsNullOrEmpty(columnName) || string.IsNullOrEmpty(orderBy))
                    return new List<ReplayMatchAward>();

                if (count == 0)
                    count = 1;

                using (var db = new HeroesParserDataContext())
                {
                    return db.ReplayMatchAwards.SqlQuery($"SELECT * FROM ReplayMatchAwards ORDER BY {columnName} {orderBy} LIMIT {count}").ToList();
                }
            }

            public static List<ReplayMatchAward> ReadRecordsWhere(string columnName, string operand, string input)
            {
                if (string.IsNullOrEmpty(columnName) || string.IsNullOrEmpty(operand))
                    return new List<ReplayMatchAward>();

                 if (Utilities.LikeOperatorInputCheck(operand, input))
                    input = $"%{input}%";
                else if (input == null)
                    input = string.Empty;

                using (var db = new HeroesParserDataContext())
                {
                    return db.ReplayMatchAwards.SqlQuery($"SELECT * FROM ReplayMatchAwards WHERE {columnName} {operand} @Input", new SQLiteParameter("@Input", input)).ToList();
                }
            }
        }
    }
}
