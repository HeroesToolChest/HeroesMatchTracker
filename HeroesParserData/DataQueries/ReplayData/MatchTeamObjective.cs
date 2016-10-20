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
        internal static class MatchTeamObjective
        {
            public static long CreateRecord(ReplayMatchTeamObjective replayMatchTeamObjective)
            {
                using (var db = new HeroesParserDataContext())
                {
                    db.ReplayMatchTeamObjectives.Add(replayMatchTeamObjective);
                    db.SaveChanges();
                }

                return replayMatchTeamObjective.ReplayId;
            }

            public static long CreateRecord(HeroesParserDataContext db, ReplayMatchTeamObjective replayMatchTeamObjective)
            {
                db.ReplayMatchTeamObjectives.Add(replayMatchTeamObjective);
                db.SaveChanges();

                return replayMatchTeamObjective.ReplayId;
            }

            public static List<ReplayMatchTeamObjective> ReadAllRecords()
            {
                using (var db = new HeroesParserDataContext())
                {
                    return db.ReplayMatchTeamObjectives.ToList();
                }
            }

            public static List<ReplayMatchTeamObjective> ReadTopRecords(int num)
            {
                using (var db = new HeroesParserDataContext())
                {
                    return db.ReplayMatchTeamObjectives.Take(num).ToList();
                }
            }

            public static List<ReplayMatchTeamObjective> ReadLastRecords(int num)
            {
                using (var db = new HeroesParserDataContext())
                {
                    return db.ReplayMatchTeamObjectives.OrderByDescending(x => x.ReplayId).Take(num).ToList();
                }
            }

            public static List<ReplayMatchTeamObjective> ReadRecordsCustomTop(int count, string columnName, string orderBy)
            {
                if (string.IsNullOrEmpty(columnName) || string.IsNullOrEmpty(orderBy))
                    return new List<ReplayMatchTeamObjective>();

                if (columnName.Contains("TimeStamp"))
                    columnName = string.Concat(columnName, "Ticks");

                if (count == 0)
                    count = 1;

                using (var db = new HeroesParserDataContext())
                {
                    return db.ReplayMatchTeamObjectives.SqlQuery($"SELECT * FROM ReplayMatchTeamObjectives ORDER BY {columnName} {orderBy} LIMIT {count}").ToList();
                }
            }

            public static List<ReplayMatchTeamObjective> ReadRecordsWhere(string columnName, string operand, string input)
            {
                if (string.IsNullOrEmpty(columnName) || string.IsNullOrEmpty(operand))
                    return new List<ReplayMatchTeamObjective>();

                if (columnName.Contains("TimeStamp"))
                {
                    TimeSpan timeSpan;
                    if (TimeSpan.TryParse(input, out timeSpan))
                    {
                        input = timeSpan.Ticks.ToString();
                        columnName = string.Concat(columnName, "Ticks");
                    }
                    else
                        return new List<ReplayMatchTeamObjective>();
                }
                else if (Utilities.LikeOperatorInputCheck(operand, input))
                    input = $"%{input}%";
                else if (input == null)
                    input = string.Empty;

                using (var db = new HeroesParserDataContext())
                {
                    return db.ReplayMatchTeamObjectives.SqlQuery($"SELECT * FROM ReplayMatchTeamObjectives WHERE {columnName} {operand} @Input", new SQLiteParameter("@Input", input)).ToList();
                }
            }
        }
    }
}
