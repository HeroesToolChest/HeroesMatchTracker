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
        internal static class MatchTeamBan
        {
            public static void CreateRecord(ReplayMatchTeamBan replayMatchTeamBan)
            {
                using (var db = new HeroesParserDataContext())
                {
                    db.ReplayMatchTeamBans.Add(replayMatchTeamBan);
                    db.SaveChanges();
                }
            }

            public static void CreateRecord(HeroesParserDataContext db, ReplayMatchTeamBan replayMatchTeamBan)
            {
                db.ReplayMatchTeamBans.Add(replayMatchTeamBan);
                db.SaveChanges();
            }

            public static List<ReplayMatchTeamBan> ReadAllRecords()
            {
                using (var db = new HeroesParserDataContext())
                {
                    return db.ReplayMatchTeamBans.ToList();
                }
            }

            public static async Task<List<ReplayMatchTeamBan>> ReadAllRecordsAsync()
            {
                using (var db = new HeroesParserDataContext())
                {
                    return await db.ReplayMatchTeamBans.ToListAsync();
                }
            }

            public static async Task<List<ReplayMatchTeamBan>> ReadTop100RecordsAsync()
            {
                using (var db = new HeroesParserDataContext())
                {
                    return await db.ReplayMatchTeamBans.Take(100).ToListAsync();
                }
            }

            public static async Task<List<ReplayMatchTeamBan>> ReadLast100RecordsAsync()
            {
                using (var db = new HeroesParserDataContext())
                {
                    return await db.ReplayMatchTeamBans.OrderByDescending(x => x.ReplayId).Take(100).ToListAsync();
                }
            }

            public static async Task<List<ReplayMatchTeamBan>> ReadRecordsCustomTopAsync(int count, string columnName, string orderBy)
            {
                if (string.IsNullOrEmpty(columnName) || string.IsNullOrEmpty(orderBy))
                    return new List<ReplayMatchTeamBan>();

                if (count == 0)
                    count = 1;

                using (var db = new HeroesParserDataContext())
                {
                    return await db.ReplayMatchTeamBans.SqlQuery($"SELECT TOP {count} * FROM dbo.ReplayMatchTeamBans ORDER BY {columnName} {orderBy}").ToListAsync();
                }
            }

            public static async Task<List<ReplayMatchTeamBan>> ReadRecordsWhereAsync(string columnName, string operand, string input)
            {
                if (string.IsNullOrEmpty(columnName) || string.IsNullOrEmpty(operand))
                    return new List<ReplayMatchTeamBan>();

                if (input == null)
                    input = string.Empty;

                using (var db = new HeroesParserDataContext())
                {
                    return await db.ReplayMatchTeamBans.SqlQuery($"SELECT * FROM dbo.ReplayMatchTeamBans WHERE {columnName} {operand} @Input", new SqlParameter("@Input", input)).ToListAsync();
                }
            }
        }
    }
}
