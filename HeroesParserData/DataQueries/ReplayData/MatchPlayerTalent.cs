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
        internal static class MatchPlayerTalent
        {
            public static long CreateRecord(ReplayMatchPlayerTalent replayMatchPlayerTalent)
            {
                using (var db = new HeroesParserDataContext())
                {
                    db.ReplayMatchPlayerTalents.Add(replayMatchPlayerTalent);
                    db.SaveChanges();
                }

                return replayMatchPlayerTalent.MatchPlayerTalentId;
            }

            public static long CreateRecord(HeroesParserDataContext db, ReplayMatchPlayerTalent replayMatchPlayerTalent)
            {
                db.ReplayMatchPlayerTalents.Add(replayMatchPlayerTalent);
                db.SaveChanges();

                return replayMatchPlayerTalent.MatchPlayerTalentId;
            }

            public static List<ReplayMatchPlayerTalent> ReadAllRecords()
            {
                using (var db = new HeroesParserDataContext())
                {
                    return db.ReplayMatchPlayerTalents.ToList();
                }
            }

            public static List<ReplayMatchPlayerTalent> ReadTopRecords(int num)
            {
                using (var db = new HeroesParserDataContext())
                {
                    return db.ReplayMatchPlayerTalents.Take(num).ToList();
                }
            }

            public static List<ReplayMatchPlayerTalent> ReadLastRecords(int num)
            {
                using (var db = new HeroesParserDataContext())
                {
                    return db.ReplayMatchPlayerTalents.OrderByDescending(x => x.ReplayId).Take(num).ToList();
                }
            }

            public static List<ReplayMatchPlayerTalent> ReadRecordsCustomTop(int count, string columnName, string orderBy)
            {
                if (string.IsNullOrEmpty(columnName) || string.IsNullOrEmpty(orderBy))
                    return new List<ReplayMatchPlayerTalent>();

                if (columnName.Contains("TimeSpanSelected"))
                    columnName = string.Concat(columnName, "Ticks");

                if (count == 0)
                    count = 1;

                using (var db = new HeroesParserDataContext())
                {
                    return db.ReplayMatchPlayerTalents.SqlQuery($"SELECT * FROM ReplayMatchPlayerTalents ORDER BY {columnName} {orderBy} LIMIT {count}").ToList();
                }
            }

            public static List<ReplayMatchPlayerTalent> ReadRecordsWhere(string columnName, string operand, string input)
            {
                if (string.IsNullOrEmpty(columnName) || string.IsNullOrEmpty(operand))
                    return new List<ReplayMatchPlayerTalent>();

                if (columnName.Contains("TimeSpanSelected"))
                {
                    TimeSpan timeSpan;
                    if (TimeSpan.TryParse(input, out timeSpan))
                    {
                        input = timeSpan.Ticks.ToString();
                        columnName = string.Concat(columnName, "Ticks");
                    }
                    else
                        return new List<ReplayMatchPlayerTalent>();
                }
                else if (Utilities.LikeOperatorInputCheck(operand, input))
                    input = $"%{input}%";
                else if (input == null)
                    input = string.Empty;

                using (var db = new HeroesParserDataContext())
                {
                    return db.ReplayMatchPlayerTalents.SqlQuery($"SELECT * FROM ReplayMatchPlayerTalents WHERE {columnName} {operand} @Input", new SQLiteParameter("@Input", input)).ToList();
                }
            }
        }
    }
}
