using Heroes.Helpers;
using Heroes.Icons.Models;
using HeroesMatchTracker.Data.Databases;
using HeroesMatchTracker.Data.Models.Replays;
using HeroesMatchTracker.Data.Queries.Settings;
using LinqKit;
using System;
using System.Collections.Generic;
using System.Linq;

namespace HeroesMatchTracker.Data.Queries.Replays
{
    public class Statistics
    {
        private UserSettings UserSettings = new UserSettings();

        /// <summary>
        /// Gets the total count of wins or losses for given hero on a particular map
        /// </summary>
        /// <param name="character">Hero name</param>
        /// <param name="season">Selected season</param>
        /// <param name="gameMode">Selected GameMode (multiple)</param>
        /// <param name="isWins">Return wins if true otherwise return losses</param>
        /// <param name="mapName">Selected map</param>
        /// <returns></returns>
        public int ReadTotalGameResults(string character, Season season, GameMode gameMode, bool isWins, string mapName)
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
                        var query = from mp in db.ReplayMatchPlayers.AsNoTracking()
                                    join r in db.Replays.AsNoTracking() on mp.ReplayId equals r.ReplayId
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
        /// Gets the total count of wins or losses for given hero
        /// </summary>
        /// <param name="character">Hero name</param>
        /// <param name="season">Selected season</param>
        /// <param name="gameMode">Selected GameMode (only one)</param>
        /// <param name="isWins">Return wins if true otherwise return losses</param>
        /// <returns></returns>
        public int ReadGameResults(string character, Season season, GameMode gameMode, bool isWins)
        {
            var replayBuild = HeroesHelpers.Builds.GetReplayBuildsFromSeason(season);

            using (var db = new ReplaysContext())
            {
                db.Configuration.AutoDetectChangesEnabled = false;

                var gameModeFilter = PredicateBuilder.New<ReplayMatch>();
                foreach (Enum value in Enum.GetValues(gameMode.GetType()))
                {
                    if ((GameMode)value != GameMode.Unknown && gameMode.HasFlag(value))
                    {
                        Enum temp = value;
                        gameModeFilter = gameModeFilter.Or(x => x.GameMode == (GameMode)temp);
                    }
                }

                var query = from mp in db.ReplayMatchPlayers.AsNoTracking()
                            join r in db.Replays.AsNoTracking() on mp.ReplayId equals r.ReplayId
                            where mp.PlayerId == UserSettings.UserPlayerId &&
                                  mp.Character == character &&
                                  mp.IsWinner == isWins &&
                                  r.ReplayBuild >= replayBuild.Item1 && r.ReplayBuild < replayBuild.Item2
                            select r;

                query = query.AsExpandable().Where(gameModeFilter);

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
            List<ReplayMatchPlayerScoreResult> list = new List<ReplayMatchPlayerScoreResult>();

            using (var db = new ReplaysContext())
            {
                db.Configuration.AutoDetectChangesEnabled = false;

                foreach (Enum value in Enum.GetValues(gameMode.GetType()))
                {
                    if ((GameMode)value != GameMode.Unknown && gameMode.HasFlag(value))
                    {
                        var query = from r in db.Replays.AsNoTracking()
                                    join mp in db.ReplayMatchPlayers.AsNoTracking() on r.ReplayId equals mp.ReplayId
                                    join mpsr in db.ReplayMatchPlayerScoreResults.AsNoTracking() on new { mp.ReplayId, mp.PlayerId } equals new { mpsr.ReplayId, mpsr.PlayerId }
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

            using (var db = new ReplaysContext())
            {
                db.Configuration.AutoDetectChangesEnabled = false;

                var gameModeFilter = PredicateBuilder.New<ReplayMatch>();
                foreach (Enum value in Enum.GetValues(gameMode.GetType()))
                {
                    if ((GameMode)value != GameMode.Unknown && gameMode.HasFlag(value))
                    {
                        Enum temp = value;
                        gameModeFilter = gameModeFilter.Or(x => x.GameMode == (GameMode)temp);
                    }
                }

                var query = from mp in db.ReplayMatchPlayers.AsNoTracking()
                            join r in db.Replays.AsNoTracking() on mp.ReplayId equals r.ReplayId
                            where mp.PlayerId == UserSettings.UserPlayerId &&
                                    mp.Character == character &&
                                    r.ReplayBuild >= replayBuild.Item1 && r.ReplayBuild < replayBuild.Item2 &&
                                    r.MapName == mapName
                            select r;

                query = query.AsExpandable().Where(gameModeFilter);

                return TimeSpan.FromTicks(query.Count() > 0 ? query.Sum(x => x.ReplayLengthTicks) : 0);
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
        public int ReadTalentsCountForHero(string character, Season season, GameMode gameMode, List<string> maps, Talent talent, bool isWinner)
        {
            var replayBuild = HeroesHelpers.Builds.GetReplayBuildsFromSeason(season);

            using (var db = new ReplaysContext())
            {
                db.Configuration.AutoDetectChangesEnabled = false;

                var gameModeFilter = PredicateBuilder.New<ReplayMatch>();
                foreach (Enum value in Enum.GetValues(gameMode.GetType()))
                {
                    if ((GameMode)value != GameMode.Unknown && gameMode.HasFlag(value))
                    {
                        Enum temp = value;
                        gameModeFilter = gameModeFilter.Or(x => x.GameMode == (GameMode)temp);
                    }
                }

                var mapFilter = PredicateBuilder.New<ReplayMatch>();
                foreach (var map in maps)
                {
                    string temp = map;
                    mapFilter = mapFilter.Or(x => x.MapName == temp);
                }

                IQueryable<ReplayMatch> query = null;

                switch (talent.Tier)
                {
                    case TalentTier.Level1:
                        query = from mpt in db.ReplayMatchPlayerTalents.AsNoTracking()
                                join r in db.Replays.AsNoTracking() on mpt.ReplayId equals r.ReplayId
                                join mp in db.ReplayMatchPlayers.AsNoTracking() on new { mpt.ReplayId, mpt.PlayerId } equals new { mp.ReplayId, mp.PlayerId }
                                where mpt.PlayerId == UserSettings.UserPlayerId &&
                                        mp.IsWinner == isWinner &&
                                        mpt.Character == character &&
                                        mpt.TalentName1 == talent.ReferenceName &&
                                        r.ReplayBuild >= replayBuild.Item1 && r.ReplayBuild < replayBuild.Item2
                                select r;
                        break;
                    case TalentTier.Level4:
                        query = from mpt in db.ReplayMatchPlayerTalents.AsNoTracking()
                                join r in db.Replays.AsNoTracking() on mpt.ReplayId equals r.ReplayId
                                join mp in db.ReplayMatchPlayers.AsNoTracking() on new { mpt.ReplayId, mpt.PlayerId } equals new { mp.ReplayId, mp.PlayerId }
                                where mpt.PlayerId == UserSettings.UserPlayerId &&
                                      mp.IsWinner == isWinner &&
                                      mpt.Character == character &&
                                      mpt.TalentName4 == talent.ReferenceName &&
                                      r.ReplayBuild >= replayBuild.Item1 && r.ReplayBuild < replayBuild.Item2
                                select r;
                        break;
                    case TalentTier.Level7:
                        query = from mpt in db.ReplayMatchPlayerTalents.AsNoTracking()
                                join r in db.Replays.AsNoTracking() on mpt.ReplayId equals r.ReplayId
                                join mp in db.ReplayMatchPlayers.AsNoTracking() on new { mpt.ReplayId, mpt.PlayerId } equals new { mp.ReplayId, mp.PlayerId }
                                where mpt.PlayerId == UserSettings.UserPlayerId &&
                                      mp.IsWinner == isWinner &&
                                      mpt.Character == character &&
                                      mpt.TalentName7 == talent.ReferenceName &&
                                      r.ReplayBuild >= replayBuild.Item1 && r.ReplayBuild < replayBuild.Item2
                                select r;
                        break;
                    case TalentTier.Level10:
                        query = from mpt in db.ReplayMatchPlayerTalents.AsNoTracking()
                                join r in db.Replays.AsNoTracking() on mpt.ReplayId equals r.ReplayId
                                join mp in db.ReplayMatchPlayers.AsNoTracking() on new { mpt.ReplayId, mpt.PlayerId } equals new { mp.ReplayId, mp.PlayerId }
                                where mpt.PlayerId == UserSettings.UserPlayerId &&
                                      mp.IsWinner == isWinner &&
                                      mpt.Character == character &&
                                      mpt.TalentName10 == talent.ReferenceName &&
                                      r.ReplayBuild >= replayBuild.Item1 && r.ReplayBuild < replayBuild.Item2
                                select r;
                        break;
                    case TalentTier.Level13:
                        query = from mpt in db.ReplayMatchPlayerTalents.AsNoTracking()
                                join r in db.Replays.AsNoTracking() on mpt.ReplayId equals r.ReplayId
                                join mp in db.ReplayMatchPlayers.AsNoTracking() on new { mpt.ReplayId, mpt.PlayerId } equals new { mp.ReplayId, mp.PlayerId }
                                where mpt.PlayerId == UserSettings.UserPlayerId &&
                                      mp.IsWinner == isWinner &&
                                      mpt.Character == character &&
                                      mpt.TalentName13 == talent.ReferenceName &&
                                      r.ReplayBuild >= replayBuild.Item1 && r.ReplayBuild < replayBuild.Item2
                                select r;
                        break;
                    case TalentTier.Level16:
                        query = from mpt in db.ReplayMatchPlayerTalents.AsNoTracking()
                                join r in db.Replays.AsNoTracking() on mpt.ReplayId equals r.ReplayId
                                join mp in db.ReplayMatchPlayers.AsNoTracking() on new { mpt.ReplayId, mpt.PlayerId } equals new { mp.ReplayId, mp.PlayerId }
                                where mpt.PlayerId == UserSettings.UserPlayerId &&
                                      mp.IsWinner == isWinner &&
                                      mpt.Character == character &&
                                      mpt.TalentName16 == talent.ReferenceName &&
                                      r.ReplayBuild >= replayBuild.Item1 && r.ReplayBuild < replayBuild.Item2
                                select r;
                        break;
                    case TalentTier.Level20:
                        query = from mpt in db.ReplayMatchPlayerTalents.AsNoTracking()
                                join r in db.Replays.AsNoTracking() on mpt.ReplayId equals r.ReplayId
                                join mp in db.ReplayMatchPlayers.AsNoTracking() on new { mpt.ReplayId, mpt.PlayerId } equals new { mp.ReplayId, mp.PlayerId }
                                where mpt.PlayerId == UserSettings.UserPlayerId &&
                                      mp.IsWinner == isWinner &&
                                      mpt.Character == character &&
                                      mpt.TalentName20 == talent.ReferenceName &&
                                      r.ReplayBuild >= replayBuild.Item1 && r.ReplayBuild < replayBuild.Item2
                                select r;
                        break;
                    default:
                        break;
                }

                query = query.AsExpandable()
                    .Where(gameModeFilter)
                    .Where(mapFilter);

                return query.Count();
            }
        }

        public int ReadMatchAwardCountForHero(string character, Season season, GameMode gameMode, List<string> maps, string mvpAwardType)
        {
            var replayBuild = HeroesHelpers.Builds.GetReplayBuildsFromSeason(season);

            using (var db = new ReplaysContext())
            {
                db.Configuration.AutoDetectChangesEnabled = false;

                var mapFilter = PredicateBuilder.New<ReplayMatch>();
                foreach (var map in maps)
                {
                    string temp = map;
                    mapFilter = mapFilter.Or(x => x.MapName == temp);
                }

                var query = from r in db.Replays.AsNoTracking()
                            join mp in db.ReplayMatchPlayers.AsNoTracking() on r.ReplayId equals mp.ReplayId
                            join ma in db.ReplayMatchAwards.AsNoTracking() on new { mp.ReplayId, mp.PlayerId } equals new { ma.ReplayId, ma.PlayerId }
                            where mp.PlayerId == UserSettings.UserPlayerId &&
                                    mp.Character == character &&
                                    r.GameMode == gameMode &&
                                    r.ReplayBuild >= replayBuild.Item1 && r.ReplayBuild < replayBuild.Item2 &&
                                    ma.Award == mvpAwardType
                            select r;
                query = query.AsExpandable().Where(mapFilter);

                return query.Count();
            }
        }

        /// <summary>
        /// Gets the total sum of a specific OverviewHeroStatOption
        /// </summary>
        /// <param name="character">Hero name</param>
        /// <param name="season">Selected season</param>
        /// <param name="gameMode">Selected GameMode (only one)</param>
        /// <param name="isWins">Return wins if true otherwise return losses</param>
        /// <returns></returns>
        public int ReadStatValue(string character, Season season, GameMode gameMode, OverviewHeroStatOption statOption)
        {
            var replayBuild = HeroesHelpers.Builds.GetReplayBuildsFromSeason(season);
            int total = 0;
            using (var db = new ReplaysContext())
            {
                db.Configuration.AutoDetectChangesEnabled = false;

                foreach (Enum value in Enum.GetValues(gameMode.GetType()))
                {
                    if ((GameMode)value != GameMode.Unknown && gameMode.HasFlag(value))
                    {
                        var query = from mp in db.ReplayMatchPlayers.AsNoTracking()
                                    join r in db.Replays.AsNoTracking() on mp.ReplayId equals r.ReplayId
                                    join mpsr in db.ReplayMatchPlayerScoreResults.AsNoTracking() on new { mp.ReplayId, mp.PlayerId } equals new { mpsr.ReplayId, mpsr.PlayerId }
                                    where mp.PlayerId == UserSettings.UserPlayerId &&
                                          mp.Character == character &&
                                          r.GameMode == (GameMode)value &&
                                          r.ReplayBuild >= replayBuild.Item1 && r.ReplayBuild < replayBuild.Item2
                                    select mpsr;

                        if (statOption == OverviewHeroStatOption.MostDeaths)
                            total += query.Sum(x => x.Deaths) ?? 0;
                        else if (statOption == OverviewHeroStatOption.MostKills)
                            total += query.Sum(x => x.SoloKills) ?? 0;
                        else if (statOption == OverviewHeroStatOption.MostAssists)
                            total += query.Sum(x => x.Assists) ?? 0;
                    }
                }

                return total;
            }
        }

        /// <summary>
        /// Gets the total count of wins or losses for a map
        /// </summary>
        /// <param name="season">Selected season</param>
        /// <param name="gameMode">Selected GameMode (multiple)</param>
        /// <param name="isWins">Return wins if true otherwise return losses</param>
        /// <param name="mapName">Selected map</param>
        /// <returns></returns>
        public int ReadMapResults(Season season, GameMode gameMode, bool isWins, string mapName)
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
                        var query = from mp in db.ReplayMatchPlayers.AsNoTracking()
                                    join r in db.Replays.AsNoTracking() on mp.ReplayId equals r.ReplayId
                                    where mp.PlayerId == UserSettings.UserPlayerId &&
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

        public List<ReplayMatchPlayer> ReadListOfMatchPlayerHeroes(Season season, GameMode gameMode)
        {
            var replayBuild = HeroesHelpers.Builds.GetReplayBuildsFromSeason(season);
            var list = new List<ReplayMatchPlayer>();

            using (var db = new ReplaysContext())
            {
                db.Configuration.AutoDetectChangesEnabled = false;

                foreach (Enum value in Enum.GetValues(gameMode.GetType()))
                {
                    if (gameMode.HasFlag(value))
                    {
                        var query = from mp in db.ReplayMatchPlayers.AsNoTracking()
                                    join r in db.Replays.AsNoTracking() on mp.ReplayId equals r.ReplayId
                                    where mp.PlayerId == UserSettings.UserPlayerId &&
                                          r.GameMode == (GameMode)value &&
                                          r.ReplayBuild >= replayBuild.Item1 && r.ReplayBuild < replayBuild.Item2
                                    select mp;

                        list.AddRange(query.ToList());
                    }
                }

                return list;
            }
        }

        public int ReadPartyGameResult(Season season, GameMode gameMode, int partySize, bool isWin)
        {
            if (partySize < 2)
                partySize = 0;

            var replayBuild = HeroesHelpers.Builds.GetReplayBuildsFromSeason(season);

            using (var db = new ReplaysContext())
            {
                db.Configuration.AutoDetectChangesEnabled = false;

                var gameModeFilter = PredicateBuilder.New<ReplayMatch>();
                foreach (Enum value in Enum.GetValues(gameMode.GetType()))
                {
                    if ((GameMode)value != GameMode.Unknown && gameMode.HasFlag(value))
                    {
                        Enum temp = value;
                        gameModeFilter = gameModeFilter.Or(x => x.GameMode == (GameMode)temp);
                    }
                }

                var query = from mp in db.ReplayMatchPlayers.AsNoTracking()
                            join r in db.Replays.AsNoTracking() on mp.ReplayId equals r.ReplayId
                            where mp.PlayerId == UserSettings.UserPlayerId &&
                                  r.ReplayBuild >= replayBuild.Item1 && r.ReplayBuild < replayBuild.Item2 &&
                                  mp.PartySize == partySize &&
                                  mp.IsWinner == isWin
                            select r;

                query = query.AsExpandable().Where(gameModeFilter);

                return query.Count();
            }
        }

        public int ReadPartyGameResult(Season season, GameMode gameMode, List<long> playerIds, List<string> characters, bool isWin)
        {
            if (playerIds.Count < 1)
                return 0;

            var replayBuild = HeroesHelpers.Builds.GetReplayBuildsFromSeason(season);

            using (var db = new ReplaysContext())
            {
                db.Configuration.AutoDetectChangesEnabled = false;

                var gameModeFilter = PredicateBuilder.New<ReplayMatch>();
                foreach (Enum value in Enum.GetValues(gameMode.GetType()))
                {
                    if ((GameMode)value != GameMode.Unknown && gameMode.HasFlag(value))
                    {
                        Enum temp = value;
                        gameModeFilter = gameModeFilter.Or(x => x.GameMode == (GameMode)temp);
                    }
                }

                IQueryable<ReplayMatch> query = null;

                query = from r in db.Replays.AsNoTracking()
                        join party in
                            (from mp in db.ReplayMatchPlayers.AsNoTracking()
                             join r in db.Replays.AsNoTracking() on mp.ReplayId equals r.ReplayId
                             where r.ReplayBuild >= replayBuild.Item1 && r.ReplayBuild < replayBuild.Item2 && (mp.Team == 0 || mp.Team == 1) && mp.IsWinner == isWin && 
                                playerIds.Contains(mp.PlayerId) && characters.Contains(mp.Character)
                             group r by mp.ReplayId into grp
                             where grp.Count() == playerIds.Count()
                             select new { grp.Key }) on r.ReplayId equals party.Key
                        select r;


                //if (playerIds.Count == 1)
                //{
                //    long id0 = playerIds[0];

                //    query = from r in db.Replays.AsNoTracking()
                //            join party in
                //                (from mp in db.ReplayMatchPlayers.AsNoTracking()
                //                 join r in db.Replays.AsNoTracking() on mp.ReplayId equals r.ReplayId
                //                 where r.ReplayBuild >= replayBuild.Item1 && r.ReplayBuild < replayBuild.Item2 &&
                //                     (mp.Team == 0 || mp.Team == 1) &&
                //                     ((mp.IsWinner == isWin && mp.PlayerId == id0))
                //                 group r by mp.ReplayId into grp
                //                 where grp.Count() == playerIds.Count()
                //                 select new { grp.Key }) on r.ReplayId equals party.Key
                //            select r;
                //}
                //else if (playerIds.Count == 2)
                //{
                //    long id0 = playerIds[0];
                //    long id1 = playerIds[1];

                //    query = from r in db.Replays.AsNoTracking()
                //            join party in
                //                (from mp in db.ReplayMatchPlayers.AsNoTracking()
                //                join r in db.Replays.AsNoTracking() on mp.ReplayId equals r.ReplayId
                //                where r.ReplayBuild >= replayBuild.Item1 && r.ReplayBuild < replayBuild.Item2 &&
                //                    (mp.Team == 0 || mp.Team == 1) &&
                //                    ((mp.IsWinner == isWin && mp.PlayerId == id0) || (mp.IsWinner == isWin && mp.PlayerId == id1))
                //                group r by mp.ReplayId into grp
                //                where grp.Count() == playerIds.Count()
                //                select new { grp.Key }) on r.ReplayId equals party.Key
                //            select r;
                //}
                //else if (playerIds.Count == 3)
                //{
                //    long id0 = playerIds[0];
                //    long id1 = playerIds[1];
                //    long id2 = playerIds[2];

                //    query = from r in db.Replays.AsNoTracking()
                //            join party in
                //                (from mp in db.ReplayMatchPlayers.AsNoTracking()
                //                 join r in db.Replays.AsNoTracking() on mp.ReplayId equals r.ReplayId
                //                 where r.ReplayBuild >= replayBuild.Item1 && r.ReplayBuild < replayBuild.Item2 &&
                //                     (mp.Team == 0 || mp.Team == 1) &&
                //                     ((mp.IsWinner == isWin && mp.PlayerId == id0) || (mp.IsWinner == isWin && mp.PlayerId == id1) || (mp.IsWinner == isWin && mp.PlayerId == id2))
                //                 group r by mp.ReplayId into grp
                //                 where grp.Count() == playerIds.Count()
                //                 select new { grp.Key }) on r.ReplayId equals party.Key
                //            select r;
                //}
                //else if (playerIds.Count == 4)
                //{
                //    long id0 = playerIds[0];
                //    long id1 = playerIds[1];
                //    long id2 = playerIds[2];
                //    long id3 = playerIds[3];

                //    query = from r in db.Replays.AsNoTracking()
                //            join party in
                //                (from mp in db.ReplayMatchPlayers.AsNoTracking()
                //                 join r in db.Replays.AsNoTracking() on mp.ReplayId equals r.ReplayId
                //                 where r.ReplayBuild >= replayBuild.Item1 && r.ReplayBuild < replayBuild.Item2 &&
                //                     (mp.Team == 0 || mp.Team == 1) &&
                //                     ((mp.IsWinner == isWin && mp.PlayerId == id0) || (mp.IsWinner == isWin && mp.PlayerId == id1) || (mp.IsWinner == isWin && mp.PlayerId == id2) ||
                //                     (mp.IsWinner == isWin && mp.PlayerId == id3))
                //                 group r by mp.ReplayId into grp
                //                 where grp.Count() == playerIds.Count()
                //                 select new { grp.Key }) on r.ReplayId equals party.Key
                //            select r;
                //}
                //else if (playerIds.Count == 5)
                //{
                //    long id0 = playerIds[0];
                //    long id1 = playerIds[1];
                //    long id2 = playerIds[2];
                //    long id3 = playerIds[3];
                //    long id4 = playerIds[3];

                //    query = from r in db.Replays.AsNoTracking()
                //            join party in
                //                (from mp in db.ReplayMatchPlayers.AsNoTracking()
                //                 join r in db.Replays.AsNoTracking() on mp.ReplayId equals r.ReplayId
                //                 where r.ReplayBuild >= replayBuild.Item1 && r.ReplayBuild < replayBuild.Item2 &&
                //                     (mp.Team == 0 || mp.Team == 1) &&
                //                     ((mp.IsWinner == isWin && mp.PlayerId == id0) || (mp.IsWinner == isWin && mp.PlayerId == id1) || (mp.IsWinner == isWin && mp.PlayerId == id2) ||
                //                     (mp.IsWinner == isWin && mp.PlayerId == id3) || (mp.IsWinner == isWin && mp.PlayerId == id4))
                //                 group r by mp.ReplayId into grp
                //                 where grp.Count() == playerIds.Count()
                //                 select new { grp.Key }) on r.ReplayId equals party.Key
                //            select r;
                //}

                query = query.AsExpandable().Where(gameModeFilter);

                return query.Count();
            }
        }
    }
}
