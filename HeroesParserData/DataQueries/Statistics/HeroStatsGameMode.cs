using Heroes.ReplayParser;
using HeroesParserData.Models.DbModels;
using HeroesParserData.Properties;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HeroesParserData.DataQueries
{
    public static partial class Query
    {
        internal class HeroStatsGameMode
        {
            public static int GetHighestLevelOfHero(string heroName)
            {
                using (var db = new HeroesParserDataContext())
                {
                    int level = db.Database.SqlQuery<int>($@"SELECT CharacterLevel FROM ReplayMatchPlayers
                                                             WHERE Character = @Character AND PlayerId = @PlayerId 
                                                             ORDER BY CharacterLevel desc",
                                                             new SQLiteParameter("@PlayerId", Settings.Default.UserPlayerId),
                                                             new SQLiteParameter("@Character", heroName)).FirstOrDefault();
                    return level;
                }
            }

            public static int GetWinsOrLossesForHero(string heroName, Season season, GameMode gameMode, bool winOrLoss)
            {
                var replayBuild = Utilities.GetSeasonReplayBuild(season);

                using (var db = new HeroesParserDataContext())
                {
                    int? wins = db.Database.SqlQuery<int?>($@"SELECT Count(IsWinner) FROM ReplayMatchPlayers mp
                                                              JOIN Replays r
                                                              ON mp.ReplayId = r.ReplayId
                                                              WHERE PlayerId = @PlayerId AND Character = @Character AND IsWinner = @Flag AND GameMode = @GameMode AND ReplayBuild >= @ReplayBuildBegin AND ReplayBuild < @ReplayBuildEnd",
                                                              new SQLiteParameter("@PlayerId", Settings.Default.UserPlayerId),
                                                              new SQLiteParameter("@Character", heroName),
                                                              new SQLiteParameter("@Flag", winOrLoss),
                                                              new SQLiteParameter("@ReplayBuildBegin", replayBuild.Item1),
                                                              new SQLiteParameter("@ReplayBuildEnd", replayBuild.Item2),
                                                              new SQLiteParameter("@GameMode", gameMode)).FirstOrDefault();
                    return wins.HasValue ? wins.Value : 0;
                }
            }
        }
    }
}
