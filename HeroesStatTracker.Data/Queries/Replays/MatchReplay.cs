using Heroes.ReplayParser;
using HeroesStatTracker.Data.Databases;
using HeroesStatTracker.Data.Models.Replays;
using System;
using System.Collections.Generic;
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
