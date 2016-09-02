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

            public static async Task<List<ReplayMatchTeamBan>> ReadTopRecordsAsync(int num)
            {
                using (var db = new HeroesParserDataContext())
                {
                    return await db.ReplayMatchTeamBans.Take(num).ToListAsync();
                }
            }

            public static async Task<List<ReplayMatchTeamBan>> ReadLastRecordsAsync(int num)
            {
                using (var db = new HeroesParserDataContext())
                {
                    return await db.ReplayMatchTeamBans.OrderByDescending(x => x.ReplayId).Take(num).ToListAsync();
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
                    return await db.ReplayMatchTeamBans.SqlQuery($"SELECT * FROM ReplayMatchTeamBans ORDER BY {columnName} {orderBy} LIMIT {count}").ToListAsync();
                }
            }

            public static async Task<List<ReplayMatchTeamBan>> ReadRecordsWhereAsync(string columnName, string operand, string input)
            {
                if (string.IsNullOrEmpty(columnName) || string.IsNullOrEmpty(operand))
                    return new List<ReplayMatchTeamBan>();

                if (input.Length == 1 || (input.Length >= 2 && input[0] != '%' && input[input.Length - 1] != '%'))
                    input = $"%{input}%";
                else if (input == null)
                    input = string.Empty;

                using (var db = new HeroesParserDataContext())
                {
                    return await db.ReplayMatchTeamBans.SqlQuery($"SELECT * FROM ReplayMatchTeamBans WHERE {columnName} {operand} @Input", new SQLiteParameter("@Input", input)).ToListAsync();
                }
            }
        }
    }
}
