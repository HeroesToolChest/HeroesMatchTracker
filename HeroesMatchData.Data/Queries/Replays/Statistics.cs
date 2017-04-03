using Heroes.Helpers;
using Heroes.ReplayParser;
using HeroesMatchData.Data.Databases;
using HeroesMatchData.Data.Models.Replays;
using HeroesMatchData.Data.Queries.Settings;
using System;
using System.Collections.Generic;
using System.Linq;

namespace HeroesMatchData.Data.Queries.Replays
{
    public class Statistics
    {
        private UserSettings UserSettings = new UserSettings();

        /// <summary>
        /// Gets the total count of wins or losses for given hero on a particular map
        /// </summary>
        /// <param name="character">Hero name</param>
        /// <param name="season">Selected season</param>
        /// <param name="gameMode">Selected GameMode</param>
        /// <param name="isWins">Return wins if true otherwise return losses</param>
        /// <param name="mapName">Selected map</param>
        /// <returns></returns>
        public int ReadTotalGameResults(string character, Season season, GameMode gameMode, bool isWins, string mapName = null)
        {
            var replayBuild = HeroesHelpers.Builds.GetReplayBuildsFromSeason(season);

            using (var db = new ReplaysContext())
            {
                var query = from mp in db.ReplayMatchPlayers
                            join r in db.Replays on mp.ReplayId equals r.ReplayId
                            where mp.PlayerId == UserSettings.UserPlayerId &&
                            mp.Character == character &&
                            mp.IsWinner == isWins &&
                            r.GameMode == gameMode &&
                            r.ReplayBuild >= replayBuild.Item1 && r.ReplayBuild < replayBuild.Item2 &&
                            r.MapName == mapName
                            select mp.IsWinner;

                return query.Count();
            }
        }

        /// <summary>
        /// Gets the score results for a hero on a particular map
        /// </summary>
        /// <param name="character">Hero name</param>
        /// <param name="season">Selected season</param>
        /// <param name="gameMode">Selected GameMode</param>
        /// <param name="mapName">Selected map</param>
        /// <returns></returns>
        public List<ReplayMatchPlayerScoreResult> ReadScoreResult(string character, Season season, GameMode gameMode, string mapName)
        {
            var replayBuild = HeroesHelpers.Builds.GetReplayBuildsFromSeason(season);

            using (var db = new ReplaysContext())
            {
                var query = from r in db.Replays
                            join mp in db.ReplayMatchPlayers on r.ReplayId equals mp.ReplayId
                            join mpsr in db.ReplayMatchPlayerScoreResults on new { mp.ReplayId, mp.PlayerId } equals new { mpsr.ReplayId, mpsr.PlayerId }
                            where mp.PlayerId == UserSettings.UserPlayerId &&
                                  mp.Character == character &&
                                  r.GameMode == gameMode &&
                                  r.ReplayBuild >= replayBuild.Item1 && r.ReplayBuild < replayBuild.Item2 &&
                                  r.MapName == mapName
                            select mpsr;

                return query.ToList();
            }
        }

        public TimeSpan ReadTotalMapGameTime(string character, Season season, GameMode gameMode, string mapName)
        {
            var replayBuild = HeroesHelpers.Builds.GetReplayBuildsFromSeason(season);

            using (var db = new ReplaysContext())
            {
                var query = from mp in db.ReplayMatchPlayers
                            join r in db.Replays on mp.ReplayId equals r.ReplayId
                            where mp.PlayerId == UserSettings.UserPlayerId &&
                            mp.Character == character &&
                            r.GameMode == gameMode &&
                            r.ReplayBuild >= replayBuild.Item1 && r.ReplayBuild < replayBuild.Item2 &&
                            r.MapName == mapName
                            select r.ReplayLengthTicks;

                return TimeSpan.FromTicks(query.Count() > 0 ? query.Sum() : 0);
            }
        }
    }
}
