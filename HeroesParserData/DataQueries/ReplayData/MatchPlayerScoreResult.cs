using HeroesParserData.Models.DbModels;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.SQLite;
using System.Linq;

namespace HeroesParserData.DataQueries
{
    public static partial class Query
    {
        internal static class MatchPlayerScoreResult
        {
            public static long CreateRecord(ReplayMatchPlayerScoreResult replayMatchPlayerScoreResult)
            {
                using (var db = new HeroesParserDataContext())
                {
                    db.ReplayMatchPlayerScoreResults.Add(replayMatchPlayerScoreResult);
                    db.SaveChanges();
                }

                return replayMatchPlayerScoreResult.MatchPlayerScoreResultId;
            }

            public static long CreateRecord(HeroesParserDataContext db, ReplayMatchPlayerScoreResult replayMatchPlayerScoreResult)
            {
                db.ReplayMatchPlayerScoreResults.Add(replayMatchPlayerScoreResult);
                db.SaveChanges();

                return replayMatchPlayerScoreResult.MatchPlayerScoreResultId;
            }

            public static List<ReplayMatchPlayerScoreResult> ReadAllRecords()
            {
                using (var db = new HeroesParserDataContext())
                {
                    return db.ReplayMatchPlayerScoreResults.ToList();
                }
            }

            public static List<ReplayMatchPlayerScoreResult> ReadTopRecords(int num)
            {
                using (var db = new HeroesParserDataContext())
                {
                    return db.ReplayMatchPlayerScoreResults.Take(num).ToList();
                }
            }

            public static List<ReplayMatchPlayerScoreResult> ReadLastRecords(int num)
            {
                using (var db = new HeroesParserDataContext())
                {
                    return db.ReplayMatchPlayerScoreResults.OrderByDescending(x => x.ReplayId).Take(num).ToList();
                }
            }

            public static List<ReplayMatchPlayerScoreResult> ReadRecordsCustomTop(int count, string columnName, string orderBy)
            {
                if (string.IsNullOrEmpty(columnName) || string.IsNullOrEmpty(orderBy))
                    return new List<ReplayMatchPlayerScoreResult>();

                if (columnName.Contains("TimeSpentDead"))
                    columnName = string.Concat(columnName, "Ticks");

                if (count == 0)
                    count = 1;

                using (var db = new HeroesParserDataContext())
                {
                    return db.ReplayMatchPlayerScoreResults.SqlQuery($"SELECT * FROM ReplayMatchPlayerScoreResults ORDER BY {columnName} {orderBy} LIMIT {count}").ToList();
                }
            }

            public static List<ReplayMatchPlayerScoreResult> ReadRecordsWhere(string columnName, string operand, string input)
            {
                if (string.IsNullOrEmpty(columnName) || string.IsNullOrEmpty(operand))
                    return new List<ReplayMatchPlayerScoreResult>();

                if (columnName.Contains("TimeSpentDead"))
                {
                    TimeSpan timeSpan;
                    if (TimeSpan.TryParse(input, out timeSpan))
                    {
                        input = timeSpan.Ticks.ToString();
                        columnName = string.Concat(columnName, "Ticks");
                    }
                    else
                        return new List<ReplayMatchPlayerScoreResult>();
                }
                else if (Utilities.LikeOperatorInputCheck(operand, input))
                    input = $"%{input}%";
                else if (input == null)
                    input = string.Empty;

                using (var db = new HeroesParserDataContext())
                {
                    return db.ReplayMatchPlayerScoreResults.SqlQuery($"SELECT * FROM ReplayMatchPlayerScoreResults WHERE {columnName} {operand} @Input", new SQLiteParameter("@Input", input)).ToList();
                }
            }
        }
    }
}
