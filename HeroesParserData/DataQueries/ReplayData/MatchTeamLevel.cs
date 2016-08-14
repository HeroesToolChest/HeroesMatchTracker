using HeroesParserData.Models.DbModels;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.SQLite;
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

            public static async Task<List<ReplayMatchTeamLevel>> ReadTopRecordsAsync(int num)
            {
                using (var db = new HeroesParserDataContext())
                {
                    return await db.ReplayMatchTeamLevels.Take(num).ToListAsync();
                }
            }

            public static async Task<List<ReplayMatchTeamLevel>> ReadLastRecordsAsync(int num)
            {
                using (var db = new HeroesParserDataContext())
                {
                    return await db.ReplayMatchTeamLevels.OrderByDescending(x => x.ReplayId).Take(num).ToListAsync();
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
                    return await db.ReplayMatchTeamLevels.SqlQuery($"SELECT * FROM ReplayMatchTeamLevels ORDER BY {columnName} {orderBy} LIMIT {count}").ToListAsync();
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
                    return await db.ReplayMatchTeamLevels.SqlQuery($"SELECT * FROM ReplayMatchTeamLevels WHERE {columnName} {operand} @Input", new SQLiteParameter("@Input", input)).ToListAsync();
                }
            }
        }
    }
}
