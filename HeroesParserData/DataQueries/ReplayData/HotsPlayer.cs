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

            public static async Task<List<ReplayAllHotsPlayer>> ReadTopRecordsAsync(int num)
            {
                using (var db = new HeroesParserDataContext())
                {
                    return await db.ReplayAllHotsPlayers.Take(num).ToListAsync();
                }
            }

            public static async Task<List<ReplayAllHotsPlayer>> ReadLastRecordsAsync(int num)
            {
                using (var db = new HeroesParserDataContext())
                {
                    return await db.ReplayAllHotsPlayers.OrderByDescending(x => x.PlayerId).Take(num).ToListAsync();
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
                    return await db.ReplayAllHotsPlayers.SqlQuery($"SELECT * FROM ReplayAllHotsPlayers ORDER BY {columnName} {orderBy} LIMIT {count}").ToListAsync();
                }
            }

            public static async Task<List<ReplayAllHotsPlayer>> ReadRecordsWhereAsync(string columnName, string operand, string input)
            {
                if (string.IsNullOrEmpty(columnName) || string.IsNullOrEmpty(operand))
                    return new List<ReplayAllHotsPlayer>();

                if (Utilities.LikeOperatorInputCheck(operand, input))
                    input = $"%{input}%";
                else if (input == null)
                    input = string.Empty;

                using (var db = new HeroesParserDataContext())
                {
                    return await db.ReplayAllHotsPlayers.SqlQuery($"SELECT * FROM ReplayAllHotsPlayers WHERE {columnName} {operand} @Input", new SQLiteParameter("@Input", input)).ToListAsync();
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
                // battleNetId is not unique, player can change their battletag and their battleNetId stays the same
                return db.ReplayAllHotsPlayers.Any(x => x.BattleTagName == replayAllHotsPlayer.BattleTagName && 
                                                        x.BattleNetId == replayAllHotsPlayer.BattleNetId &&
                                                        x.BattleNetSubId == replayAllHotsPlayer.BattleNetSubId);
            }

            public static long UpdateRecord(HeroesParserDataContext db, ReplayAllHotsPlayer replayAllHotsPlayer)
            {
                var record = db.ReplayAllHotsPlayers.SingleOrDefault(x => x.BattleTagName == replayAllHotsPlayer.BattleTagName &&
                                                                     x.BattleNetId == replayAllHotsPlayer.BattleNetId &&
                                                                     x.BattleNetSubId == replayAllHotsPlayer.BattleNetSubId);

                if (record != null)
                {
                    if (record.BattleNetId < 1)
                    {
                        record.BattleNetId = replayAllHotsPlayer.BattleNetId;
                        record.BattleNetRegionId = replayAllHotsPlayer.BattleNetRegionId;
                        record.BattleNetSubId = replayAllHotsPlayer.BattleNetSubId;
                        record.LastSeen = replayAllHotsPlayer.LastSeen;
                    }

                    if (replayAllHotsPlayer.LastSeen > record.LastSeen)
                        record.LastSeen = replayAllHotsPlayer.LastSeen;

                    record.Seen += 1;

                    db.SaveChanges();
                }

                return record.PlayerId;
            }

            public static long ReadPlayerIdFromBattleNetId(HeroesParserDataContext db, string battleTagName, int battleNetId)
            {
                // battleNetId is not unique, player can change their battletag and their battleNetId stays the same
                return db.ReplayAllHotsPlayers.SingleOrDefault(x => x.BattleTagName == battleTagName && x.BattleNetId == battleNetId).PlayerId;
            }

            public static async Task<ReplayAllHotsPlayer> ReadRecordFromPlayerId(long playerId)
            {
                using (var db = new HeroesParserDataContext())
                {
                    return await db.ReplayAllHotsPlayers.Where(x => x.PlayerId == playerId).FirstOrDefaultAsync();
                }
            }

            public static bool IsValidBattleNetTagName(string battleTagName)
            {
                using (var db = new HeroesParserDataContext())
                {
                    return db.ReplayAllHotsPlayers.Any(x => x.BattleTagName == battleTagName);
                }
            }

            public static long ReadPlayerIdFromBattleNetTag(string battleTagName)
            {
                using (var db = new HeroesParserDataContext())
                {
                    return db.ReplayAllHotsPlayers.FirstOrDefault(x => x.BattleTagName == battleTagName).PlayerId;
                }
            }
        }
    }
}
