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

            public static async Task<List<ReplayMatchTeamLevel>> ReadAllRecordsAsync()
            {
                using (var db = new HeroesParserDataContext())
                {
                    return await db.ReplayMatchTeamLevels.ToListAsync();
                }
            }

            public static async Task<List<ReplayMatchTeamLevel>> ReadTop100RecordsAsync()
            {
                using (var db = new HeroesParserDataContext())
                {
                    return await db.ReplayMatchTeamLevels.Take(100).ToListAsync();
                }
            }

            public static async Task<List<ReplayMatchTeamLevel>> ReadLast100RecordsAsync()
            {
                using (var db = new HeroesParserDataContext())
                {
                    return await db.ReplayMatchTeamLevels.OrderByDescending(x => x.ReplayId).Take(100).ToListAsync();
                }
            }

            public static async Task<List<ReplayMatchTeamLevel>> ReadRecordsCustomTopAsync(int count, string columnName, string orderBy)
            {
                if (string.IsNullOrEmpty(columnName) || string.IsNullOrEmpty(orderBy))
                    return new List<ReplayMatchTeamLevel>();

                if (count == 0)
                    count = 1;

                using (var db = new HeroesParserDataContext())
                {
                    return await db.ReplayMatchTeamLevels.SqlQuery($"SELECT TOP {count} * FROM dbo.ReplayMatchTeamLevels ORDER BY {columnName} {orderBy}").ToListAsync();
                }
            }

            public static async Task<List<ReplayMatchTeamLevel>> ReadRecordsWhereAsync(string columnName, string operand, string input)
            {
                if (string.IsNullOrEmpty(columnName) || string.IsNullOrEmpty(operand))
                    return new List<ReplayMatchTeamLevel>();

                if (input == null)
                    input = string.Empty;

                using (var db = new HeroesParserDataContext())
                {
                    return await db.ReplayMatchTeamLevels.SqlQuery($"SELECT * FROM dbo.ReplayMatchTeamLevels WHERE {columnName} {operand} @Input", new SqlParameter("@Input", input)).ToListAsync();
                }
            }
        }
    }
}
