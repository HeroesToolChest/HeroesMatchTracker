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
        internal static class MatchTeamExperience
        {
            public static long CreateRecord(ReplayMatchTeamExperience replayMatchTeamExperience)
            {
                using (var db = new HeroesParserDataContext())
                {
                    db.ReplayMatchTeamExperiences.Add(replayMatchTeamExperience);
                    db.SaveChanges();
                }

                return replayMatchTeamExperience.ReplayId;
            }

            public static long CreateRecord(HeroesParserDataContext db, ReplayMatchTeamExperience replayMatchTeamExperience)
            {
                db.ReplayMatchTeamExperiences.Add(replayMatchTeamExperience);
                db.SaveChanges();

                return replayMatchTeamExperience.ReplayId;
            }

            public static List<ReplayMatchTeamExperience> ReadAllRecords()
            {
                using (var db = new HeroesParserDataContext())
                {
                    return db.ReplayMatchTeamExperiences.ToList();
                }
            }

            public static async Task<List<ReplayMatchTeamExperience>> ReadAllRecordsAsync()
            {
                using (var db = new HeroesParserDataContext())
                {
                    return await db.ReplayMatchTeamExperiences.ToListAsync();
                }
            }

            public static async Task<List<ReplayMatchTeamExperience>> ReadTopRecordsAsync(int num)
            {
                using (var db = new HeroesParserDataContext())
                {
                    return await db.ReplayMatchTeamExperiences.Take(num).ToListAsync();
                }
            }

            public static async Task<List<ReplayMatchTeamExperience>> ReadLastRecordsAsync(int num)
            {
                using (var db = new HeroesParserDataContext())
                {
                    return await db.ReplayMatchTeamExperiences.OrderByDescending(x => x.ReplayId).Take(num).ToListAsync();
                }
            }

            public static async Task<List<ReplayMatchTeamExperience>> ReadRecordsCustomTopAsync(int count, string columnName, string orderBy)
            {
                if (string.IsNullOrEmpty(columnName) || string.IsNullOrEmpty(orderBy))
                    return new List<ReplayMatchTeamExperience>();

                if (columnName.Contains("Time"))
                    columnName = string.Concat(columnName, "Ticks");

                if (count == 0)
                    count = 1;

                using (var db = new HeroesParserDataContext())
                {
                    return await db.ReplayMatchTeamExperiences.SqlQuery($"SELECT * FROM ReplayMatchTeamExperiences ORDER BY {columnName} {orderBy} LIMIT {count}").ToListAsync();
                }
            }

            public static async Task<List<ReplayMatchTeamExperience>> ReadRecordsWhereAsync(string columnName, string operand, string input)
            {
                if (string.IsNullOrEmpty(columnName) || string.IsNullOrEmpty(operand))
                    return new List<ReplayMatchTeamExperience>();

                if (columnName.Contains("Time"))
                {
                    TimeSpan timeSpan;
                    if (TimeSpan.TryParse(input, out timeSpan))
                    {
                        input = timeSpan.Ticks.ToString();
                        columnName = string.Concat(columnName, "Ticks");
                    }
                    else
                        return new List<ReplayMatchTeamExperience>();
                }

                if (input == null)
                    input = string.Empty;

                using (var db = new HeroesParserDataContext())
                {
                    return await db.ReplayMatchTeamExperiences.SqlQuery($"SELECT * FROM ReplayMatchTeamExperiences WHERE {columnName} {operand} @Input", new SQLiteParameter("@Input", input)).ToListAsync();
                }
            }
        }
    }
}
