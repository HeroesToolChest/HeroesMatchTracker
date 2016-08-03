using HeroesParserData.Models.DbModels;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace HeroesParserData.DataQueries.ReplayData
{
    public static partial class Query
    {
        internal static class Replay
        {
            public static long CreateRecord(Models.DbModels.Replay replay)
            {
                using (var db = new HeroesParserDataContext())
                {
                    db.Replays.Add(replay);
                    db.SaveChanges();
                }

                return replay.ReplayId;
            }

            public static long CreateRecord(HeroesParserDataContext db, Models.DbModels.Replay replay)
            {
                db.Replays.Add(replay);
                db.SaveChanges();

                return replay.ReplayId;
            }

            public static List<Models.DbModels.Replay> ReadAllRecords()
            {
                using (var db = new HeroesParserDataContext())
                {
                    return db.Replays.ToList();
                }
            }

            public static async Task<List<Models.DbModels.Replay>> ReadAllRecordsAsync()
            {
                using (var db = new HeroesParserDataContext())
                {
                    return await db.Replays.ToListAsync();
                }
            }

            public static async Task<List<Models.DbModels.Replay>> ReadTopRecordsAsync(int num)
            {
                using (var db = new HeroesParserDataContext())
                {
                    return await db.Replays.Take(num).ToListAsync();
                }
            }

            public static async Task<List<Models.DbModels.Replay>> ReadLastRecordsAsync(int num)
            {
                using (var db = new HeroesParserDataContext())
                {
                    return await db.Replays.OrderByDescending(x => x.ReplayId).Take(num).ToListAsync();
                }
            }

            public static async Task<List<Models.DbModels.Replay>> ReadRecordsCustomTopAsync(int count, string columnName, string orderBy)
            {
                if ( string.IsNullOrEmpty(columnName) || string.IsNullOrEmpty(orderBy))
                    return new List<Models.DbModels.Replay>();

                if (count == 0)
                    count = 1;

                using (var db = new HeroesParserDataContext())
                {
                    return await db.Replays.SqlQuery($"SELECT TOP {count} * FROM dbo.Replays ORDER BY {columnName} {orderBy}").ToListAsync();
                }
            }

            public static async Task<List<Models.DbModels.Replay>> ReadRecordsWhereAsync(string columnName, string operand, string input)
            {
                if (string.IsNullOrEmpty(columnName) || string.IsNullOrEmpty(operand))
                    return new List<Models.DbModels.Replay>();

                if (input == null)
                    input = string.Empty;

                using (var db = new HeroesParserDataContext())
                {
                    return await db.Replays.SqlQuery($"SELECT * FROM dbo.Replays WHERE {columnName} {operand} @Input", new SqlParameter("@Input", input)).ToListAsync();
                }
            }

            /// <summary>
            /// Check if the replay was already submitted
            /// </summary>
            /// <param name="replay">Replay</param>
            /// <returns>True if existing found, otherwise false</returns>
            public static bool IsExistingReplay(Models.DbModels.Replay replay)
            {
                using (var db = new HeroesParserDataContext())
                {
                    return db.Replays.Any(x => x.RandomValue == replay.RandomValue);
                }
            }

            /// <summary>
            /// Check if the replay was already submitted
            /// </summary>
            /// <param name="db">HeroesParserDataContext</param>
            /// <param name="replay">Replay</param>
            /// <returns>The date (UTC) of the last replay</returns>
            public static bool IsExistingReplay(Models.DbModels.Replay replay, HeroesParserDataContext db)
            {
                return db.Replays.Any(x => x.RandomValue == replay.RandomValue);
            }

            public static DateTime LatestReplayByDateTimeUTC()
            {
                using (var db = new HeroesParserDataContext())
                {
                    var query = from x in db.Replays
                                orderby x.TimeStamp descending
                                select x.TimeStamp;

                    var record = query.FirstOrDefault();
                    if (record != null)
                        return record.Value;
                    else
                        return new DateTime();         
                }                   
            }

            public static long GetTotalReplayCount()
            {
                using (var db = new HeroesParserDataContext())
                {
                    return db.Replays.Count();
                }
            }

            public static async Task<Models.DbModels.Replay> ReadReplayRecord(long replayId)
            {
                Models.DbModels.Replay replay = new Models.DbModels.Replay();

                using (var db = new HeroesParserDataContext())
                {
                    replay = await db.Replays.Where(x => x.ReplayId == replayId)
                        .Include(x => x.ReplayMatchPlayers).Include(x => x.ReplayMatchPlayerTalents)
                        .FirstOrDefaultAsync();

                    if (replay == null)
                        return null;

                    //replay.ReplayMatchPlayers = await db.ReplayMatchPlayers.Where(x => x.ReplayId == replayId).ToListAsync();
                    //replay.ReplayMatchPlayerTalents = await db.ReplayMatchPlayerTalents.Where(x => x.ReplayId == replayId).ToListAsync();

                    //foreach (var player in replay.ReplayMatchPlayers)
                    //{
                    //    player.ReplayAllHotsPlayer = await db.ReplayAllHotsPlayers.Where(x => x.PlayerId == player.PlayerId).FirstAsync();
                    //}
                }

                return replay;
            }
        }
    }
}
