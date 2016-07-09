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
        internal static class MatchPlayer
        {
            public static long CreateRecord(ReplayMatchPlayer replayMatchPlayer)
            {
                using (var db = new HeroesParserDataContext())
                {
                    db.ReplayMatchPlayers.Add(replayMatchPlayer);
                    db.SaveChanges();
                }

                return replayMatchPlayer.MatchPlayerId;
            }

            public static long CreateRecord(HeroesParserDataContext db, ReplayMatchPlayer replayPlayer)
            {
                db.ReplayMatchPlayers.Add(replayPlayer);
                db.SaveChanges();

                return replayPlayer.MatchPlayerId;
            }

            public static List<ReplayMatchPlayer> ReadAllRecords()
            {
                using (var db = new HeroesParserDataContext())
                {
                    return db.ReplayMatchPlayers.ToList();
                }
            }

            public static async Task<List<ReplayMatchPlayer>> ReadAllRecordsAsync()
            {
                using (var db = new HeroesParserDataContext())
                {
                    return await db.ReplayMatchPlayers.ToListAsync();
                }
            }

            public static async Task<List<ReplayMatchPlayer>> ReadTop100RecordsAsync()
            {
                using (var db = new HeroesParserDataContext())
                {
                    return await db.ReplayMatchPlayers.Take(100).ToListAsync();
                }
            }

            public static async Task<List<ReplayMatchPlayer>> ReadLast100RecordsAsync()
            {
                using (var db = new HeroesParserDataContext())
                {
                    return await db.ReplayMatchPlayers.OrderByDescending(x => x.ReplayId).Take(100).ToListAsync();
                }
            }

            public static async Task<List<ReplayMatchPlayer>> ReadRecordsCustomTopAsync(int count, string columnName, string orderBy)
            {
                if (string.IsNullOrEmpty(columnName) || string.IsNullOrEmpty(orderBy))
                    return new List<ReplayMatchPlayer>();

                if (count == 0)
                    count = 1;

                using (var db = new HeroesParserDataContext())
                {
                    return await db.ReplayMatchPlayers.SqlQuery($"SELECT TOP {count} * FROM dbo.ReplayMatchPlayers ORDER BY {columnName} {orderBy}").ToListAsync();
                }
            }

            public static async Task<List<ReplayMatchPlayer>> ReadRecordsWhereAsync(string columnName, string operand, string input)
            {
                if (string.IsNullOrEmpty(columnName) || string.IsNullOrEmpty(operand))
                    return new List<ReplayMatchPlayer>();

                if (input == null)
                    input = string.Empty;

                using (var db = new HeroesParserDataContext())
                {
                    return await db.ReplayMatchPlayers.SqlQuery($"SELECT * FROM dbo.ReplayMatchPlayers WHERE {columnName} {operand} @Input", new SqlParameter("@Input", input)).ToListAsync();
                }
            }
        }
    }
}
