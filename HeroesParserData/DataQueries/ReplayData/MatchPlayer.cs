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

            public static async Task<List<ReplayMatchPlayer>> ReadTopRecordsAsync(int num)
            {
                using (var db = new HeroesParserDataContext())
                {
                    return await db.ReplayMatchPlayers.Take(num).ToListAsync();
                }
            }

            public static async Task<List<ReplayMatchPlayer>> ReadLastRecordsAsync(int num)
            {
                using (var db = new HeroesParserDataContext())
                {
                    return await db.ReplayMatchPlayers.OrderByDescending(x => x.ReplayId).Take(num).ToListAsync();
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
                    return await db.ReplayMatchPlayers.SqlQuery($"SELECT * FROM ReplayMatchPlayers ORDER BY {columnName} {orderBy} LIMIT {count}").ToListAsync();
                }
            }

            public static async Task<List<ReplayMatchPlayer>> ReadRecordsWhereAsync(string columnName, string operand, string input)
            {
                if (string.IsNullOrEmpty(columnName) || string.IsNullOrEmpty(operand))
                    return new List<ReplayMatchPlayer>();

                if (columnName.Contains("Is"))
                {
                    if (input.ToUpperInvariant() == "TRUE")
                        input = "1";
                    else if (input.ToUpperInvariant() == "FALSE")
                        input = "0";
                }
                else if (Utilities.LikeOperatorInputCheck(operand, input))
                    input = $"%{input}%";
                else if (input == null)
                    input = string.Empty;

                using (var db = new HeroesParserDataContext())
                {
                    return await db.ReplayMatchPlayers.SqlQuery($"SELECT * FROM ReplayMatchPlayers WHERE {columnName} {operand} @Input", new SQLiteParameter("@Input", input)).ToListAsync();
                }
            }

            public static async Task<List<ReplayMatchPlayer>> ReadRecordsByReplayId(long replayId)
            {
                using (var db = new HeroesParserDataContext())
                {
                    return await db.ReplayMatchPlayers.Where(x => x.ReplayId == replayId).ToListAsync();
                }
            }
        }
    }
}
