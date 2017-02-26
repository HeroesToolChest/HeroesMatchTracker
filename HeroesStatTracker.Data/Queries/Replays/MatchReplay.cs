using Heroes.Helpers;
using Heroes.ReplayParser;
using HeroesStatTracker.Data.Databases;
using HeroesStatTracker.Data.Models.Replays;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.SQLite;
using System.Linq;

namespace HeroesStatTracker.Data.Queries.Replays
{
    public class MatchReplay : NonContextQueriesBase<ReplayMatch>, IRawDataQueries<ReplayMatch>
    {
        public List<ReplayMatch> ReadAllRecords()
        {
            using (var db = new ReplaysContext())
            {
                return db.Replays.ToList();
            }
        }

        public List<ReplayMatch> ReadTopRecords(int amount)
        {
            using (var db = new ReplaysContext())
            {
                return db.Replays.OrderByDescending(x => x.ReplayId).Take(amount).ToList();
            }
        }

        public List<ReplayMatch> ReadLastRecords(int amount)
        {
            using (var db = new ReplaysContext())
            {
                if (db.Replays.Count() > 0)
                    return db.Replays.OrderByDescending(x => x.ReplayId).Take(amount).ToList();
                else
                    return new List<ReplayMatch>();
            }
        }

        public List<ReplayMatch> ReadRecordsCustomTop(int amount, string columnName, string orderBy)
        {
            if (string.IsNullOrEmpty(columnName) || string.IsNullOrEmpty(orderBy))
                return new List<ReplayMatch>();

            if (columnName.Contains("ReplayLength"))
                columnName = string.Concat(columnName, "Ticks");

            if (amount == 0)
                amount = 1;

            using (var db = new ReplaysContext())
            {
                return db.Replays.SqlQuery($"SELECT * FROM Replays ORDER BY {columnName} {orderBy} LIMIT {amount}").ToList();
            }
        }

        public List<ReplayMatch> ReadRecordsWhere(string columnName, string operand, string input)
        {
            if (string.IsNullOrEmpty(columnName) || string.IsNullOrEmpty(operand))
                return new List<ReplayMatch>();

            if (columnName.Contains("ReplayLength"))
            {
                TimeSpan timeSpan;
                if (TimeSpan.TryParse(input, out timeSpan))
                {
                    input = timeSpan.Ticks.ToString();
                    columnName = string.Concat(columnName, "Ticks");
                }
                else
                {
                    return new List<ReplayMatch>();
                }
            }
            else if (columnName == "GameMode")
            {
                GameMode gameMode;
                if (Enum.TryParse(input, true, out gameMode))
                    input = ((int)gameMode).ToString();
            }
            else if (LikeOperatorInputCheck(operand, input))
            {
                input = $"%{input}%";
            }
            else if (input == null)
            {
                input = string.Empty;
            }

            using (var db = new ReplaysContext())
            {
                return db.Replays.SqlQuery($"SELECT * FROM Replays WHERE {columnName} {operand} @Input", new SQLiteParameter("@Input", input)).ToList();
            }
        }

        public List<ReplayMatch> ReadLatestReplaysByDateTime(int amount)
        {
            using (var db = new ReplaysContext())
            {
                return db.Replays.OrderByDescending(x => x.TimeStamp).Take(amount).ToList();
            }
        }

        public long ReadReplayIdByRandomValue(ReplayMatch model)
        {
            using (var db = new ReplaysContext())
            {
                return db.Replays.SingleOrDefault(x => x.RandomValue == model.RandomValue).ReplayId;
            }
        }

        public long GetTotalReplayCount()
        {
            using (var db = new ReplaysContext())
            {
                return db.Replays.Count();
            }
        }

        public DateTime ReadLatestReplayByDateTime()
        {
            using (var db = new ReplaysContext())
            {
                var record = db.Replays.OrderByDescending(x => x.TimeStamp).FirstOrDefault();

                if (record != null)
                    return record.TimeStamp.Value;
                else
                    return DateTime.Now;
            }
        }

        public DateTime ReadLastReplayByDateTime()
        {
            using (var db = new ReplaysContext())
            {
                var record = db.Replays.OrderByDescending(x => x.ReplayId).FirstOrDefault();

                if (record != null)
                    return record.TimeStamp.Value;
                else
                    return DateTime.Now;
            }
        }

        public List<ReplayMatch> ReadGameModeRecords(GameMode gameMode, ReplayFilter replayFilter)
        {
            var replayBuild = HeroesHelpers.Builds.GetReplayBuildsFromSeason(replayFilter.SelectedSeason);

            using (var db = new ReplaysContext())
            {
                IQueryable<ReplayMatch> query = db.Set<ReplayMatch>();

                if (gameMode == (GameMode.Brawl | GameMode.Custom | GameMode.HeroLeague | GameMode.QuickMatch | GameMode.TeamLeague | GameMode.UnrankedDraft))
                    query = query.Where(x => x.ReplayBuild >= replayBuild.Item1 && x.ReplayBuild < replayBuild.Item2);
                else
                    query = query.Where(x => x.GameMode == gameMode && x.ReplayBuild >= replayBuild.Item1 && x.ReplayBuild < replayBuild.Item2);

                if (replayFilter.SelectedReplayId > 0)
                    query = query.Where(x => x.ReplayId == replayFilter.SelectedReplayId);

                if (replayFilter.SelectedMapOption != replayFilter.MapOptionsList[0])
                    query = query.Where(x => x.MapName == replayFilter.SelectedMapOption);

                if (replayFilter.SelectedBuildOption != replayFilter.BuildOptionsList[0])
                {
                    int? build = Convert.ToInt32(replayFilter.SelectedBuildOption);
                    query = query.Where(x => x.ReplayBuild == build);
                }

                if (replayFilter.SelectedGameTimeOption != replayFilter.GameTimeOptionList[0])
                {
                    var value = HeroesHelpers.GameDates.GetGameTimeModifiedTime(replayFilter.SelectedGameTimeOption);

                    if (value.Item1 == "less than")
                        query = query.Where(x => x.ReplayLengthTicks < value.Item2.Ticks);
                    else if (value.Item1 == "longer than")
                        query = query.Where(x => x.ReplayLengthTicks > value.Item2.Ticks);
                }

                if (replayFilter.SelectedGameDateOption != replayFilter.GameDateOptionList[0])
                {
                    var value = HeroesHelpers.GameDates.GetGameDateModifiedDate(replayFilter.SelectedGameDateOption);

                    if (value.Item1 == "last")
                        query = query.Where(x => x.TimeStamp >= value.Item2);
                    else if (value.Item1 == "more than")
                        query = query.Where(x => x.TimeStamp <= value.Item2);
                }

                if (!string.IsNullOrEmpty(replayFilter.SelectedBattleTag))
                {
                    query = from r in query
                            join mp in db.ReplayMatchPlayers on r.ReplayId equals mp.ReplayId
                            join ahp in db.ReplayAllHotsPlayers on mp.PlayerId equals ahp.PlayerId
                            where ahp.BattleTagName.ToLower().Contains(replayFilter.SelectedBattleTag.ToLower())
                            select r;
                }

                if (replayFilter.SelectedCharacter != replayFilter.HeroesList[0])
                {
                    if (replayFilter.IsGivenBattleTagOnlyChecked)
                    {
                        query = from r in query
                                join mp in db.ReplayMatchPlayers on r.ReplayId equals mp.ReplayId
                                join ahp in db.ReplayAllHotsPlayers on mp.PlayerId equals ahp.PlayerId
                                where ahp.BattleTagName.ToLower().Contains(replayFilter.SelectedBattleTag.ToLower()) &&
                                mp.Character == replayFilter.SelectedCharacter
                                select r;
                    }
                    else
                    {
                        query = from r in query
                                join mp in db.ReplayMatchPlayers on r.ReplayId equals mp.ReplayId
                                where mp.Character == replayFilter.SelectedCharacter
                                select r;
                    }
                }

                return query.Distinct().OrderByDescending(x => x.TimeStamp).ToList();
            }
        }

        /// <summary>
        /// Returns the Replay along all the other ReplayMatch models
        /// </summary>
        /// <param name="replayId">Replay Id</param>
        /// <returns>Replay</returns>
        public ReplayMatch ReadReplayIncludeAssociatedRecords(long replayId)
        {
            ReplayMatch replayMatch = new ReplayMatch();

            using (var db = new ReplaysContext())
            {
                replayMatch = db.Replays.Where(x => x.ReplayId == replayId)
                    .Include(x => x.ReplayMatchPlayers)
                    .Include(x => x.ReplayMatchPlayerTalents)
                    .Include(x => x.ReplayMatchTeamBan)
                    .Include(x => x.ReplayMatchPlayerScoreResults)
                    .Include(x => x.ReplayMatchMessage)
                    .Include(x => x.ReplayMatchAward)
                    .Include(x => x.ReplayMatchTeamLevels)
                    .Include(x => x.ReplayMatchTeamExperiences)
                    .FirstOrDefault();

                if (replayMatch == null)
                    return null;
            }

            return replayMatch;
        }

        internal override long CreateRecord(ReplaysContext db, ReplayMatch model)
        {
            db.Replays.Add(model);
            db.SaveChanges();

            return model.ReplayId;
        }

        internal override long UpdateRecord(ReplaysContext db, ReplayMatch model)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Check if the replay was already submitted
        /// </summary>
        /// <param name="db">ReplaysContext</param>
        /// <param name="model">ReplayMatch model</param>
        /// <returns>The date (UTC) of the last replay</returns>
        internal override bool IsExistingRecord(ReplaysContext db, ReplayMatch model)
        {
            return db.Replays.Any(x => x.RandomValue == model.RandomValue);
        }
    }
}
