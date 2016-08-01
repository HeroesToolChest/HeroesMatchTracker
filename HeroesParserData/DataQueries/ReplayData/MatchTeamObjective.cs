using HeroesParserData.Models.DbModels;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.SqlClient;
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

                if (count == 0)
                    count = 1;

                using (var db = new HeroesParserDataContext())
                {
                    return await db.ReplayMatchTeamObjectives.SqlQuery($"SELECT TOP {count} * FROM dbo.ReplayMatchTeamObjectives ORDER BY {columnName} {orderBy}").ToListAsync();
                }
            }

            public static async Task<List<ReplayMatchTeamObjective>> ReadRecordsWhereAsync(string columnName, string operand, string input)
            {
                if (string.IsNullOrEmpty(columnName) || string.IsNullOrEmpty(operand))
                    return new List<ReplayMatchTeamObjective>();

                if (input == null)
                    input = string.Empty;

                using (var db = new HeroesParserDataContext())
                {
                    return await db.ReplayMatchTeamObjectives.SqlQuery($"SELECT * FROM dbo.ReplayMatchTeamObjectives WHERE {columnName} {operand} @Input", new SqlParameter("@Input", input)).ToListAsync();
                }
            }
        }
    }
}
