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
        internal static class HotsPlayer
        {
            public static long CreateRecord(ReplayAllHotsPlayer replayAllHotsPlayer)
            {
                using (var db = new HeroesParserDataContext())
                {
                    db.ReplayAllHotsPlayers.Add(replayAllHotsPlayer);
                    db.SaveChanges();
                }

                return replayAllHotsPlayer.PlayerId;
            }

            public static long CreateRecord(HeroesParserDataContext db, ReplayAllHotsPlayer replayAllHotsPlayer)
            {
                db.ReplayAllHotsPlayers.Add(replayAllHotsPlayer);
                db.SaveChanges();

                return replayAllHotsPlayer.PlayerId;
            }

            public static List<ReplayAllHotsPlayer> ReadAllRecords()
            {
                using (var db = new HeroesParserDataContext())
                {
                    return db.ReplayAllHotsPlayers.ToList();
                }
            }

            public static async Task<List<ReplayAllHotsPlayer>> ReadAllRecordsAsync()
            {
                using (var db = new HeroesParserDataContext())
                {
                    return await db.ReplayAllHotsPlayers.ToListAsync();
                }
            }

            public static async Task<List<ReplayAllHotsPlayer>> ReadTop100RecordsAsync()
            {
                using (var db = new HeroesParserDataContext())
                {
                    return await db.ReplayAllHotsPlayers.Take(100).ToListAsync();
                }
            }

            public static async Task<List<ReplayAllHotsPlayer>> ReadLast100RecordsAsync()
            {
                using (var db = new HeroesParserDataContext())
                {
                    return await db.ReplayAllHotsPlayers.OrderByDescending(x => x.PlayerId).Take(100).ToListAsync();
                }
            }

            public static async Task<List<ReplayAllHotsPlayer>> ReadRecordsCustomTopAsync(int count, string columnName, string orderBy)
            {
                if (string.IsNullOrEmpty(columnName) || string.IsNullOrEmpty(orderBy))
                    return new List<ReplayAllHotsPlayer>();

                if (count == 0)
                    count = 1;

                using (var db = new HeroesParserDataContext())
                {
                    return await db.ReplayAllHotsPlayers.SqlQuery($"SELECT TOP {count} * FROM dbo.ReplayAllHotsPlayers ORDER BY {columnName} {orderBy}").ToListAsync();
                }
            }

            public static async Task<List<ReplayAllHotsPlayer>> ReadRecordsWhereAsync(string columnName, string operand, string input)
            {
                if (string.IsNullOrEmpty(columnName) || string.IsNullOrEmpty(operand))
                    return new List<ReplayAllHotsPlayer>();

                if (input == null)
                    input = string.Empty;

                using (var db = new HeroesParserDataContext())
                {
                    return await db.ReplayAllHotsPlayers.SqlQuery($"SELECT * FROM dbo.ReplayAllHotsPlayers WHERE {columnName} {operand} @Input", new SqlParameter("@Input", input)).ToListAsync();
                }
            }

            public static bool IsExistingHotsPlayer(ReplayAllHotsPlayer replayAllHotsPlayer)
            {
                using (var db = new HeroesParserDataContext())
                {
                    return db.ReplayAllHotsPlayers.Any(x => x.BattleTagName == replayAllHotsPlayer.BattleTagName);
                }
            }

            public static bool IsExistingHotsPlayer(HeroesParserDataContext db, ReplayAllHotsPlayer replayAllHotsPlayer)
            {
                return db.ReplayAllHotsPlayers.Any(x => x.BattleTagName == replayAllHotsPlayer.BattleTagName);
            }

            public static void UpdateSeen(ReplayAllHotsPlayer replayAllHotsPlayer)
            {
                using (var db = new HeroesParserDataContext())
                {
                    var record = db.ReplayAllHotsPlayers.SingleOrDefault(x => x.BattleTagName == replayAllHotsPlayer.BattleTagName);

                    if (record != null)
                    {
                        record.Seen += 1;

                        db.SaveChanges();
                    }
                }
            }

            public static long UpdateSeen(HeroesParserDataContext db, ReplayAllHotsPlayer replayAllHotsPlayer)
            {
                var record = db.ReplayAllHotsPlayers.SingleOrDefault(x => x.BattleTagName == replayAllHotsPlayer.BattleTagName);

                if (record != null)
                {
                    record.Seen += 1;

                    db.SaveChanges();
                }

                return record.PlayerId;
            }

            public static long ReadPlayerIdFromBattleNetId(HeroesParserDataContext db, string battleTagName, int battleNetId)
            {
                // battleNetId is not unique
                return db.ReplayAllHotsPlayers.SingleOrDefault(x => x.BattleTagName == battleTagName && x.BattleNetId == battleNetId).PlayerId;
            }
        }
    }
}
