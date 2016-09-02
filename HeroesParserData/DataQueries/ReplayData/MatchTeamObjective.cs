using HeroesParserData.Models.DbModels;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.SQLite;
using System.Linq;
using System.Threading.Tasks;

namespace HeroesParserData.DataQueries.ReplayData
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

            public static async Task<List<ReplayMatchTeamObjective>> ReadAllRecordsAsync()
            {
                using (var db = new HeroesParserDataContext())
                {
                    return await db.ReplayMatchTeamObjectives.ToListAsync();
                }
            }

            public static async Task<List<ReplayMatchTeamObjective>> ReadTopRecordsAsync(int num)
            {
                using (var db = new HeroesParserDataContext())
                {
                    return await db.ReplayMatchTeamObjectives.Take(num).ToListAsync();
                }
            }

            public static async Task<List<ReplayMatchTeamObjective>> ReadLastRecordsAsync(int num)
            {
                using (var db = new HeroesParserDataContext())
                {
                    return await db.ReplayMatchTeamObjectives.OrderByDescending(x => x.ReplayId).Take(num).ToListAsync();
                }
            }

            public static async Task<List<ReplayMatchTeamObjective>> ReadRecordsCustomTopAsync(int count, string columnName, string orderBy)
            {
                if (string.IsNullOrEmpty(columnName) || string.IsNullOrEmpty(orderBy))
                    return new List<ReplayMatchTeamObjective>();

                if (columnName.Contains("TimeStamp"))
                    columnName = string.Concat(columnName, "Ticks");

                if (count == 0)
                    count = 1;

                using (var db = new HeroesParserDataContext())
                {
                    return await db.ReplayMatchTeamObjectives.SqlQuery($"SELECT * FROM ReplayMatchTeamObjectives ORDER BY {columnName} {orderBy} LIMIT {count}").ToListAsync();
                }
            }

            public static async Task<List<ReplayMatchTeamObjective>> ReadRecordsWhereAsync(string columnName, string operand, string input)
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

                if (input.Length == 1 || (input.Length >= 2 && input[0] != '%' && input[input.Length - 1] != '%'))
                    input = $"%{input}%";
                else if (input == null)
                    input = string.Empty;

                using (var db = new HeroesParserDataContext())
                {
                    return await db.ReplayMatchTeamObjectives.SqlQuery($"SELECT * FROM ReplayMatchTeamObjectives WHERE {columnName} {operand} @Input", new SQLiteParameter("@Input", input)).ToListAsync();
                }
            }
        }
    }
}
