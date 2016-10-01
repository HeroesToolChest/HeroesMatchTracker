using Heroes.ReplayParser;
using HeroesParserData.Models.DbModels;
using HeroesParserData.Properties;
using System.Data.SQLite;
using System.Linq;

namespace HeroesParserData.DataQueries.ReplayData
{
    public static partial class Query
    {
        internal class PlayerStatistics
        {
            public static int ReadTotalStatTypeForCharacter(StatType statType, Season season, GameMode gameMode, string character)
            {
                var replayBuild = Utilities.GetSeasonReplayBuild(season);

                string type = string.Empty;
                if (statType == StatType.assists)
                    type = "Assists";
                else if (statType == StatType.deaths)
                    type = "Deaths";
                else if (statType == StatType.kills)
                    type = "SoloKills";

                string gameModeString;
                if (gameMode != GameMode.Cooperative)
                    gameModeString = GameModeQueryString(false);
                else
                    gameModeString = GameModeQueryString(true);

                using (var db = new HeroesParserDataContext())
                {
                    if (statType == StatType.wins)
                    {
                        int? wins = db.Database.SqlQuery<int?>($@"SELECT Count(IsWinner) FROM ReplayMatchPlayers mp
                                                                  JOIN Replays r
                                                                  ON mp.ReplayId = r.ReplayId
                                                                  WHERE PlayerId = @PlayerId AND IsWinner = 1 AND Character = @Character AND {gameModeString} AND ReplayBuild >= @ReplayBuildBegin AND ReplayBuild < @ReplayBuildEnd",
                                                                  new SQLiteParameter("@PlayerId", Settings.Default.UserPlayerId),
                                                                  new SQLiteParameter("@Character", character),
                                                                  new SQLiteParameter("@ReplayBuildBegin", replayBuild.Item1),
                                                                  new SQLiteParameter("@ReplayBuildEnd", replayBuild.Item2),
                                                                  new SQLiteParameter("@GameMode", gameMode)).FirstOrDefault();
                        return wins.HasValue ? wins.Value : 0;
                    }
                    else
                    {
                        int? amount = db.Database.SqlQuery<int?>($@"SELECT Sum(mpsr.{type}) FROM ReplayMatchPlayers mp
                                                                    JOIN Replays r
                                                                    JOIN ReplayAllHotsPlayers hp
                                                                    JOIN ReplayMatchPlayerScoreResults mpsr 
                                                                    ON mp.ReplayId = mpsr.ReplayId AND 
                                                                    mp.ReplayId = r.ReplayId AND 
                                                                    mp.PlayerId = mpsr.PlayerId AND
                                                                    mp.PlayerId = hp.PlayerId AND
                                                                    mp.PlayerId = mpsr.PlayerId
                                                                    WHERE mp.PlayerId = @PlayerId AND Character = @Character AND {gameModeString} AND ReplayBuild >= @ReplayBuildBegin AND ReplayBuild < @ReplayBuildEnd",
                                                                    new SQLiteParameter("@PlayerId", Settings.Default.UserPlayerId),
                                                                    new SQLiteParameter("@Character", character),
                                                                    new SQLiteParameter("@ReplayBuildBegin", replayBuild.Item1),
                                                                    new SQLiteParameter("@ReplayBuildEnd", replayBuild.Item2),
                                                                    new SQLiteParameter("@GameMode", gameMode)).FirstOrDefault();
                        return amount.HasValue ? amount.Value : 0;
                    }
                }
            }

            public static int ReadMapWins(Season season, GameMode gameMode, string map)
            {
                var replayBuild = Utilities.GetSeasonReplayBuild(season);

                string gameModeString;
                if (gameMode != GameMode.Cooperative)
                    gameModeString = GameModeQueryString(false);
                else
                    gameModeString = GameModeQueryString(true);

                using (var db = new HeroesParserDataContext())
                {
                    int? wins = db.Database.SqlQuery<int?>($@"SELECT Count(IsWinner) FROM ReplayMatchPlayers mp
                                                              JOIN Replays r
                                                              ON mp.ReplayId = r.ReplayId
                                                              WHERE PlayerId = @PlayerId AND MapName = @Map AND IsWinner = 1 AND {gameModeString} AND ReplayBuild >= @ReplayBuildBegin AND ReplayBuild < @ReplayBuildEnd",
                                                              new SQLiteParameter("@PlayerId", Settings.Default.UserPlayerId),
                                                              new SQLiteParameter("@Map", map),
                                                              new SQLiteParameter("@ReplayBuildBegin", replayBuild.Item1),
                                                              new SQLiteParameter("@ReplayBuildEnd", replayBuild.Item2),
                                                              new SQLiteParameter("@GameMode", gameMode)).FirstOrDefault();
                    return wins.HasValue ? wins.Value : 0;
                }
            }

            public static int ReadMapLosses(Season season, GameMode gameMode, string map)
            {
                var replayBuild = Utilities.GetSeasonReplayBuild(season);

                string gameModeString;
                if (gameMode != GameMode.Cooperative)
                    gameModeString = GameModeQueryString(false);
                else
                    gameModeString = GameModeQueryString(true);

                using (var db = new HeroesParserDataContext())
                {
                    int? wins = db.Database.SqlQuery<int?>($@"SELECT Count(IsWinner) FROM ReplayMatchPlayers mp
                                                              JOIN Replays r
                                                              ON mp.ReplayId = r.ReplayId
                                                              WHERE PlayerId = @PlayerId AND MapName = @Map AND IsWinner = 0 AND {gameModeString} AND ReplayBuild >= @ReplayBuildBegin AND ReplayBuild < @ReplayBuildEnd",
                                                              new SQLiteParameter("@PlayerId", Settings.Default.UserPlayerId),
                                                              new SQLiteParameter("@Map", map),
                                                              new SQLiteParameter("@ReplayBuildBegin", replayBuild.Item1),
                                                              new SQLiteParameter("@ReplayBuildEnd", replayBuild.Item2),
                                                              new SQLiteParameter("@GameMode", gameMode)).FirstOrDefault();
                    return wins.HasValue ? wins.Value : 0;
                }
            }

            public static int ReadGameModeTotalGames(Season season, GameMode gameMode)
            {
                var replayBuild = Utilities.GetSeasonReplayBuild(season);

                using (var db = new HeroesParserDataContext())
                {
                    int? wins = db.Database.SqlQuery<int?>($@"SELECT Count(IsWinner) FROM ReplayMatchPlayers mp
                                                              JOIN Replays r
                                                              ON mp.ReplayId = r.ReplayId
                                                              WHERE PlayerId = @PlayerId AND GameMode = @GameMode AND ReplayBuild >= @ReplayBuildBegin AND ReplayBuild < @ReplayBuildEnd",
                                                              new SQLiteParameter("@PlayerId", Settings.Default.UserPlayerId),
                                                              new SQLiteParameter("@ReplayBuildBegin", replayBuild.Item1),
                                                              new SQLiteParameter("@ReplayBuildEnd", replayBuild.Item2),
                                                              new SQLiteParameter("@GameMode", gameMode)).FirstOrDefault();
                    return wins.HasValue ? wins.Value : 0;
                }
            }

            public static int ReadGameModeTotalWins(Season season, GameMode gameMode)
            {
                var replayBuild = Utilities.GetSeasonReplayBuild(season);

                using (var db = new HeroesParserDataContext())
                {
                    int? wins = db.Database.SqlQuery<int?>($@"SELECT Count(IsWinner) FROM ReplayMatchPlayers mp
                                                              JOIN Replays r
                                                              ON mp.ReplayId = r.ReplayId
                                                              WHERE PlayerId = @PlayerId AND IsWinner = 1 AND GameMode = @GameMode AND ReplayBuild >= @ReplayBuildBegin AND ReplayBuild < @ReplayBuildEnd",
                                                              new SQLiteParameter("@PlayerId", Settings.Default.UserPlayerId),
                                                              new SQLiteParameter("@ReplayBuildBegin", replayBuild.Item1),
                                                              new SQLiteParameter("@ReplayBuildEnd", replayBuild.Item2),
                                                              new SQLiteParameter("@GameMode", gameMode)).FirstOrDefault();
                    return wins.HasValue ? wins.Value : 0;
                }
            }

            private static string GameModeQueryString(bool allOrSingle)
            {
                if (allOrSingle)
                    return "GameMode >= 3";
                else
                    return "GameMode = @GameMode";
            }
        }
    }
}
