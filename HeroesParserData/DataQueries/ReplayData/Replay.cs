using Heroes.ReplayParser;
using HeroesParserData.Models.DbModels;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.SQLite;
using System.Linq;

namespace HeroesParserData.DataQueries
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

            public static List<Models.DbModels.Replay> ReadGameModeRecords(GameMode gameMode, Season season)
            {
                var replayBuild = Utilities.GetSeasonReplayBuild(season);

                using (var db = new HeroesParserDataContext())
                {
                    return db.Replays.Where(x => x.GameMode == gameMode && x.ReplayBuild >= replayBuild.Item1 && x.ReplayBuild < replayBuild.Item2)
                        .OrderByDescending(x => x.TimeStamp).ToList();
                }
            }

            public static List<Models.DbModels.Replay> ReadTopRecords(int num)
            {
                using (var db = new HeroesParserDataContext())
                {
                    return db.Replays.Take(num).ToList();
                }
            }

            // last parsed replays
            public static List<Models.DbModels.Replay> ReadLastRecords(int num)
            {
                using (var db = new HeroesParserDataContext())
                {
                    if (db.Replays.Count() > 0)
                        return db.Replays.OrderByDescending(x => x.ReplayId).Take(num).ToList();
                    else
                        return new List<Models.DbModels.Replay>();
                }
            }

            // latest replays by time stamp
            public static List<Models.DbModels.Replay> ReadLatestRecords(int num)
            {
                using (var db = new HeroesParserDataContext())
                {
                    if (db.Replays.Count() > 0)
                        return db.Replays.OrderByDescending(x => x.TimeStamp).Take(num).ToList();
                    else
                        return new List<Models.DbModels.Replay>();
                }
            }

            public static List<Models.DbModels.Replay> ReadRecordsCustomTop(int count, string columnName, string orderBy)
            {
                if ( string.IsNullOrEmpty(columnName) || string.IsNullOrEmpty(orderBy))
                    return new List<Models.DbModels.Replay>();

                if (columnName.Contains("ReplayLength"))
                    columnName = string.Concat(columnName, "Ticks");

                if (count == 0)
                count = 1;

                using (var db = new HeroesParserDataContext())
                {
                    return db.Replays.SqlQuery($"SELECT * FROM Replays ORDER BY {columnName} {orderBy} LIMIT {count}").ToList();
                }
            }

            public static List<Models.DbModels.Replay> ReadRecordsWhere(string columnName, string operand, string input)
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
                else if (Utilities.LikeOperatorInputCheck(operand, input))
                    input = $"%{input}%";
                else if (input == null)
                    input = string.Empty;

                using (var db = new HeroesParserDataContext())
                {
                    return db.Replays.SqlQuery($"SELECT * FROM Replays WHERE {columnName} {operand} @Input", new SQLiteParameter("@Input", input)).ToList();
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

            public static DateTime ReadLatestReplayByDateTime()
            {
                using (var db = new HeroesParserDataContext())
                {
                    var record = db.Replays.OrderByDescending(x => x.TimeStamp).FirstOrDefault();

                    if (record != null)
                        return record.TimeStamp.Value;
                    else
                        return new DateTime();         
                }                   
            }

            public static DateTime ReadLastReplayByDateTime()
            {
                using (var db = new HeroesParserDataContext())
                {
                    var record = db.Replays.OrderByDescending(x => x.ReplayId).FirstOrDefault();

                    if (record != null)
                        return record.TimeStamp.Value;
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
            /// Returns the Replay along with ReplayMatchPlayers, ReplayMatchPlayerTalents, ReplayMatchTeamBan, ReplayMatchPlayerScoreResults, ReplayMatchMessage, ReplayMatchAward, ReplayMatchTeamLevel
            /// </summary>
            /// <param name="replayId">Replay Id</param>
            /// <returns>Replay</returns>
            public static Models.DbModels.Replay ReadReplayIncludeRecord(long replayId)
            {
                Models.DbModels.Replay replay = new Models.DbModels.Replay();

                using (var db = new HeroesParserDataContext())
                {
                    replay = db.Replays.Where(x => x.ReplayId == replayId)
                        .Include(x => x.ReplayMatchPlayers)
                        .Include(x => x.ReplayMatchPlayerTalents)
                        .Include(x => x.ReplayMatchTeamBan)
                        .Include(x => x.ReplayMatchPlayerScoreResults)
                        .Include(x => x.ReplayMatchMessage)
                        .Include(x => x.ReplayMatchAward)
                        .Include(x => x.ReplayMatchTeamLevels)
                        .FirstOrDefault();

                    if (replay == null)
                        return null;
                }

                return replay;
            }

            public static int ReadTotalGames(GameMode gameMode)
            {
                using (var db = new HeroesParserDataContext())
                {
                    return db.Replays.Where(x => x.GameMode == gameMode).Count();
                }
            }

            public static int ReadTotalGamesForSeason(GameMode gameMode, Season season)
            {
                using (var db = new HeroesParserDataContext())
                {
                    var replayBuild = Utilities.GetSeasonReplayBuild(season);
                    if (replayBuild != null)
                        return db.Replays.Where(x => x.GameMode == gameMode && x.ReplayBuild >= replayBuild.Item1 && x.ReplayBuild < replayBuild.Item2).Count();
                    else
                        return -1;
                }
            }
        }
    }
}
