using Heroes.ReplayParser;
using HeroesParserData.Models.DbModels;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.SQLite;
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
            public static async Task<List<Models.DbModels.Replay>> ReadGameModeRecordsAsync(GameMode gameMode)
            {
                using (var db = new HeroesParserDataContext())
                {
                    return await db.Replays.Where(x => x.GameMode == gameMode).OrderByDescending(x => x.TimeStamp).ToListAsync();
                }
            }

            public static async Task<List<Models.DbModels.Replay>> ReadTopRecordsAsync(int num)
            {
                using (var db = new HeroesParserDataContext())
                {
                    return await db.Replays.Take(num).ToListAsync();
                }
            }

            // last parsed replays
            public static async Task<List<Models.DbModels.Replay>> ReadLastRecordsAsync(int num)
            {
                using (var db = new HeroesParserDataContext())
                {
                    if (await db.Replays.CountAsync() > 0)
                        return await db.Replays.OrderByDescending(x => x.ReplayId).Take(num).ToListAsync();
                    else
                        return new List<Models.DbModels.Replay>();
                }
            }

            // latest replays by time stamp
            public static async Task<List<Models.DbModels.Replay>> ReadLatestRecordsAsync(int num)
            {
                using (var db = new HeroesParserDataContext())
                {
                    if (await db.Replays.CountAsync() > 0)
                        return await db.Replays.OrderByDescending(x => x.TimeStamp).Take(num).ToListAsync();
                    else
                        return new List<Models.DbModels.Replay>();
                }
            }

            public static async Task<List<Models.DbModels.Replay>> ReadRecordsCustomTopAsync(int count, string columnName, string orderBy)
            {
                if ( string.IsNullOrEmpty(columnName) || string.IsNullOrEmpty(orderBy))
                    return new List<Models.DbModels.Replay>();

                if (columnName.Contains("ReplayLength"))
                    columnName = string.Concat(columnName, "Ticks");

                if (count == 0)
                count = 1;

                using (var db = new HeroesParserDataContext())
                {
                    return await db.Replays.SqlQuery($"SELECT * FROM Replays ORDER BY {columnName} {orderBy} LIMIT {count}").ToListAsync();
                }
            }

            public static async Task<List<Models.DbModels.Replay>> ReadRecordsWhereAsync(string columnName, string operand, string input)
            {
                if (string.IsNullOrEmpty(columnName) || string.IsNullOrEmpty(operand))
                    return new List<Models.DbModels.Replay>();

                if (columnName.Contains("ReplayLength"))
                {
                    TimeSpan timeSpan;
                    if (TimeSpan.TryParse(input, out timeSpan))
                    {
                        input = timeSpan.Ticks.ToString();
                        columnName = string.Concat(columnName, "Ticks");
                    }
                    else
                        return new List<Models.DbModels.Replay>();
                }
                else if (columnName == "GameMode")
                {
                    GameMode gameMode;
                    if (Enum.TryParse(input, true, out gameMode))
                        input = ((int)gameMode).ToString();
                }

                if (input == null)
                    input = string.Empty;

                using (var db = new HeroesParserDataContext())
                {
                    return await db.Replays.SqlQuery($"SELECT * FROM Replays WHERE {columnName} {operand} @Input", new SQLiteParameter("@Input", input)).ToListAsync();
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

            public static DateTime? ReadLatestReplayByDateTime()
            {
                using (var db = new HeroesParserDataContext())
                {
                    var record = db.Replays.OrderByDescending(x => x.TimeStamp).FirstOrDefault();

                    if (record != null)
                        return record.TimeStamp;
                    else
                        return new DateTime();         
                }                   
            }

            public static DateTime? ReadLastReplayByDateTime()
            {
                using (var db = new HeroesParserDataContext())
                {
                    var record = db.Replays.OrderByDescending(x => x.ReplayId).FirstOrDefault();

                    if (record != null)
                        return record.TimeStamp;
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

            /// <summary>
            /// Returns the Replay along with ReplayMatchPlayers, ReplayMatchPlayerTalents, ReplayMatchTeamBan, ReplayMatchPlayerScoreResults
            /// </summary>
            /// <param name="replayId">Replay Id</param>
            /// <returns>Replay</returns>
            public static async Task<Models.DbModels.Replay> ReadReplayIncludeRecord(long replayId)
            {
                Models.DbModels.Replay replay = new Models.DbModels.Replay();

                using (var db = new HeroesParserDataContext())
                {
                    replay = await db.Replays.Where(x => x.ReplayId == replayId)
                        .Include(x => x.ReplayMatchPlayers)
                        .Include(x => x.ReplayMatchPlayerTalents)
                        .Include(x => x.ReplayMatchTeamBan)
                        .Include(x => x.ReplayMatchPlayerScoreResults)
                        .FirstOrDefaultAsync();

                    if (replay == null)
                        return null;
                }

                return replay;
            }
        }
    }
}
