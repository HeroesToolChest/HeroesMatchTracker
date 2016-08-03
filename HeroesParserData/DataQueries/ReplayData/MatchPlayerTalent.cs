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

            public static async Task<List<ReplayMatchPlayerTalent>> ReadAllRecordsAsync()
            {
                using (var db = new HeroesParserDataContext())
                {
                    return await db.ReplayMatchPlayerTalents.ToListAsync();
                }
            }

            public static async Task<List<ReplayMatchPlayerTalent>> ReadTopRecordsAsync(int num)
            {
                using (var db = new HeroesParserDataContext())
                {
                    return await db.ReplayMatchPlayerTalents.Take(num).ToListAsync();
                }
            }

            public static async Task<List<ReplayMatchPlayerTalent>> ReadLastRecordsAsync(int num)
            {
                using (var db = new HeroesParserDataContext())
                {
                    return await db.ReplayMatchPlayerTalents.OrderByDescending(x => x.ReplayId).Take(num).ToListAsync();
                }
            }

            public static async Task<List<ReplayMatchPlayerTalent>> ReadRecordsCustomTopAsync(int count, string columnName, string orderBy)
            {
                if (string.IsNullOrEmpty(columnName) || string.IsNullOrEmpty(orderBy))
                    return new List<ReplayMatchPlayerTalent>();

                if (count == 0)
                    count = 1;

                using (var db = new HeroesParserDataContext())
                {
                    return await db.ReplayMatchPlayerTalents.SqlQuery($"SELECT TOP {count} * FROM dbo.ReplayMatchPlayerTalents ORDER BY {columnName} {orderBy}").ToListAsync();
                }
            }

            public static async Task<List<ReplayMatchPlayerTalent>> ReadRecordsWhereAsync(string columnName, string operand, string input)
            {
                if (string.IsNullOrEmpty(columnName) || string.IsNullOrEmpty(operand))
                    return new List<ReplayMatchPlayerTalent>();

                if (input == null)
                    input = string.Empty;

                using (var db = new HeroesParserDataContext())
                {
                    return await db.ReplayMatchPlayerTalents.SqlQuery($"SELECT * FROM dbo.ReplayMatchPlayerTalents WHERE {columnName} {operand} @Input", new SqlParameter("@Input", input)).ToListAsync();
                }
            }

            public static async Task<List<ReplayMatchPlayerTalent>> ReadRecordsByReplayId(long replayId)
            {
                using (var db = new HeroesParserDataContext())
                {
                    return await db.ReplayMatchPlayerTalents.Where(x => x.ReplayId == replayId).ToListAsync();
                }
            }
        }
    }
}
