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
        internal static class MatchTeamLevel
        {
            public static long CreateRecord(ReplayMatchTeamLevel replayMatchTeamLevel)
            {
                using (var db = new HeroesParserDataContext())
                {
                    db.ReplayMatchTeamLevels.Add(replayMatchTeamLevel);
                    db.SaveChanges();
                }

                return replayMatchTeamLevel.ReplayId;
            }

            public static long CreateRecord(HeroesParserDataContext db, ReplayMatchTeamLevel replayMatchTeamLevel)
            {
                db.ReplayMatchTeamLevels.Add(replayMatchTeamLevel);
                db.SaveChanges();

                return replayMatchTeamLevel.ReplayId;
            }

            public static List<ReplayMatchTeamLevel> ReadAllRecords()
            {
                using (var db = new HeroesParserDataContext())
                {
                    return db.ReplayMatchTeamLevels.ToList();
                }
            }

            public static List<ReplayMatchTeamLevel> ReadTopRecords(int num)
            {
                using (var db = new HeroesParserDataContext())
                {
                    return db.ReplayMatchTeamLevels.Take(num).ToList();
                }
            }

            public static List<ReplayMatchTeamLevel> ReadLastRecords(int num)
            {
                using (var db = new HeroesParserDataContext())
                {
                    return db.ReplayMatchTeamLevels.OrderByDescending(x => x.ReplayId).Take(num).ToList();
                }
            }

            public static List<ReplayMatchTeamLevel> ReadRecordsCustomTop(int count, string columnName, string orderBy)
            {
                if (string.IsNullOrEmpty(columnName) || string.IsNullOrEmpty(orderBy))
                    return new List<ReplayMatchTeamLevel>();

                if (columnName.Contains("TeamTime"))
                    columnName = string.Concat(columnName, "Ticks");

                if (count == 0)
                    count = 1;

                using (var db = new HeroesParserDataContext())
                {
                    return db.ReplayMatchTeamLevels.SqlQuery($"SELECT * FROM ReplayMatchTeamLevels ORDER BY {columnName} {orderBy} LIMIT {count}").ToList();
                }
            }

            public static List<ReplayMatchTeamLevel> ReadRecordsWhere(string columnName, string operand, string input)
            {
                if (string.IsNullOrEmpty(columnName) || string.IsNullOrEmpty(operand))
                    return new List<ReplayMatchTeamLevel>();

                if (columnName.Contains("TeamTime"))
                {
                    TimeSpan timeSpan;
                    if (TimeSpan.TryParse(input, out timeSpan))
                    {
                        input = timeSpan.Ticks.ToString();
                        columnName = string.Concat(columnName, "Ticks");
                    }
                    else
                        return new List<ReplayMatchTeamLevel>();
                }
                else if (Utilities.LikeOperatorInputCheck(operand, input))
                    input = $"%{input}%";
                else if (input == null)
                    input = string.Empty;

                using (var db = new HeroesParserDataContext())
                {
                    return db.ReplayMatchTeamLevels.SqlQuery($"SELECT * FROM ReplayMatchTeamLevels WHERE {columnName} {operand} @Input", new SQLiteParameter("@Input", input)).ToList();
                }
            }
        }
    }
}
