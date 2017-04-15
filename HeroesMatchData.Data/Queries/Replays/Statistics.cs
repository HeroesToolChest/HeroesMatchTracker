using Heroes.Helpers;
using Heroes.Icons;
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
            int total = 0;

            using (var db = new ReplaysContext())
            {
                db.Configuration.AutoDetectChangesEnabled = false;

                foreach (Enum value in Enum.GetValues(gameMode.GetType()))
                {
                    if (gameMode.HasFlag(value))
                    {
                        var query = from mp in db.ReplayMatchPlayers
                                    join r in db.Replays on mp.ReplayId equals r.ReplayId
                                    where mp.PlayerId == UserSettings.UserPlayerId &&
                                          mp.Character == character &&
                                          mp.IsWinner == isWins &&
                                          r.GameMode == (GameMode)value &&
                                          r.ReplayBuild >= replayBuild.Item1 && r.ReplayBuild < replayBuild.Item2 &&
                                          r.MapName == mapName
                                    select mp.IsWinner;

                        total += query.Count();
                    }
                }

                return total;
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
            List<ReplayMatchPlayerScoreResult> list = new List<ReplayMatchPlayerScoreResult>();

            using (var db = new ReplaysContext())
            {
                db.Configuration.AutoDetectChangesEnabled = false;

                foreach (Enum value in Enum.GetValues(gameMode.GetType()))
                {
                    if ((GameMode)value != GameMode.Unknown && gameMode.HasFlag(value))
                    {
                        var query = from r in db.Replays
                                    join mp in db.ReplayMatchPlayers on r.ReplayId equals mp.ReplayId
                                    join mpsr in db.ReplayMatchPlayerScoreResults on new { mp.ReplayId, mp.PlayerId } equals new { mpsr.ReplayId, mpsr.PlayerId }
                                    where mp.PlayerId == UserSettings.UserPlayerId &&
                                          mp.Character == character &&
                                          r.GameMode == (GameMode)value &&
                                          r.ReplayBuild >= replayBuild.Item1 && r.ReplayBuild < replayBuild.Item2 &&
                                          r.MapName == mapName
                                    select mpsr;

                        list.AddRange(query.ToList());
                    }
                }

                return list;
            }
        }

        public TimeSpan ReadTotalMapGameTime(string character, Season season, GameMode gameMode, string mapName)
        {
            var replayBuild = HeroesHelpers.Builds.GetReplayBuildsFromSeason(season);
            TimeSpan total = TimeSpan.FromTicks(0);

            using (var db = new ReplaysContext())
            {
                db.Configuration.AutoDetectChangesEnabled = false;

                foreach (Enum value in Enum.GetValues(gameMode.GetType()))
                {
                    if ((GameMode)value != GameMode.Unknown && gameMode.HasFlag(value))
                    {
                        var query = from mp in db.ReplayMatchPlayers
                                    join r in db.Replays on mp.ReplayId equals r.ReplayId
                                    where mp.PlayerId == UserSettings.UserPlayerId &&
                                          mp.Character == character &&
                                          r.GameMode == (GameMode)value &&
                                          r.ReplayBuild >= replayBuild.Item1 && r.ReplayBuild < replayBuild.Item2 &&
                                          r.MapName == mapName
                                    select r.ReplayLengthTicks;

                        total += TimeSpan.FromTicks(query.Count() > 0 ? query.Sum() : 0);
                    }
                }

                return total;
            }
        }

        /// <summary>
        /// Gets the win or loss count of the talent for a given hero
        /// </summary>
        /// <param name="character">Hero name</param>
        /// <param name="season">Selected season</param>
        /// <param name="gameMode">Selected GameMode</param>
        /// <param name="mapName">Selected map</param>
        /// <param name="talentReferenceName">Selected talent reference name</param>
        /// <param name="tier">The tier that the talent is on</param>
        /// <param name="isWinner">Get wins if true otherwise losses</param>
        /// <returns></returns>
        public int ReadTalentsCountForHero(string character, Season season, GameMode gameMode, List<string> maps, string talentReferenceName, TalentTier tier, bool isWinner)
        {
            var replayBuild = HeroesHelpers.Builds.GetReplayBuildsFromSeason(season);
            string talentNameColumn = string.Empty;
            int total = 0;

            using (var db = new ReplaysContext())
            {
                db.Configuration.AutoDetectChangesEnabled = false;

                foreach (Enum value in Enum.GetValues(gameMode.GetType()))
                {
                    if ((GameMode)value != GameMode.Unknown && gameMode.HasFlag(value))
                    {
                        foreach (var map in maps)
                        {
                            var query = from mpt in db.ReplayMatchPlayerTalents
                                        join r in db.Replays on mpt.ReplayId equals r.ReplayId
                                        join mp in db.ReplayMatchPlayers on new { mpt.ReplayId, mpt.PlayerId } equals new { mp.ReplayId, mp.PlayerId }
                                        where mpt.PlayerId == UserSettings.UserPlayerId &&
                                              mp.IsWinner == isWinner &&
                                              mpt.Character == character &&
                                              r.GameMode == (GameMode)value &&
                                              r.MapName == map &&
                                              r.ReplayBuild >= replayBuild.Item1 && r.ReplayBuild < replayBuild.Item2
                                        select mpt;

                            switch (tier)
                            {
                                case TalentTier.Level1:
                                    query = query.Where(x => x.TalentName1 == talentReferenceName);
                                    break;
                                case TalentTier.Level4:
                                    query = query.Where(x => x.TalentName4 == talentReferenceName);
                                    break;
                                case TalentTier.Level7:
                                    query = query.Where(x => x.TalentName7 == talentReferenceName);
                                    break;
                                case TalentTier.Level10:
                                    query = query.Where(x => x.TalentName10 == talentReferenceName);
                                    break;
                                case TalentTier.Level13:
                                    query = query.Where(x => x.TalentName13 == talentReferenceName);
                                    break;
                                case TalentTier.Level16:
                                    query = query.Where(x => x.TalentName16 == talentReferenceName);
                                    break;
                                case TalentTier.Level20:
                                    query = query.Where(x => x.TalentName20 == talentReferenceName);
                                    break;
                                default:
                                    talentNameColumn = null;
                                    break;
                            }

                            total += query.Count();
                        }
                    }
                }

                return total;
            }
        }

        public int ReadMatchAwardCountForHero(string character, Season season, GameMode gameMode, List<string> maps, string mvpAwardType)
        {
            var replayBuild = HeroesHelpers.Builds.GetReplayBuildsFromSeason(season);
            int total = 0;

            using (var db = new ReplaysContext())
            {
                db.Configuration.AutoDetectChangesEnabled = false;

                foreach (var map in maps)
                {
                    var query = from r in db.Replays
                                join mp in db.ReplayMatchPlayers on r.ReplayId equals mp.ReplayId
                                join ma in db.ReplayMatchAwards on new { mp.ReplayId, mp.PlayerId } equals new { ma.ReplayId, ma.PlayerId }
                                where mp.PlayerId == UserSettings.UserPlayerId &&
                                      mp.Character == character &&
                                      r.GameMode == gameMode &&
                                      r.ReplayBuild >= replayBuild.Item1 && r.ReplayBuild < replayBuild.Item2 &&
                                      r.MapName == map &&
                                      ma.Award == mvpAwardType
                                select ma;

                    total += query.Count();
                }

                return total;
            }
        }
    }
}
