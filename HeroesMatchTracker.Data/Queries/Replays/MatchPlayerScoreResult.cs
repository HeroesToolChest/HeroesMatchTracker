using HeroesMatchTracker.Data.Databases;
using HeroesMatchTracker.Data.Models.Replays;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;

namespace HeroesMatchTracker.Data.Queries.Replays
{
    public class MatchPlayerScoreResult : NonContextQueriesBase<ReplayMatchPlayerScoreResult>, IRawDataQueries<ReplayMatchPlayerScoreResult>
    {
        public IEnumerable<ReplayMatchPlayerScoreResult> ReadAllRecords()
        {
            using (var db = new ReplaysContext())
            {
                return db.ReplayMatchPlayerScoreResults.AsNoTracking().ToList();
            }
        }

        public IEnumerable<ReplayMatchPlayerScoreResult> ReadLastRecords(int amount)
        {
            using (var db = new ReplaysContext())
            {
                return db.ReplayMatchPlayerScoreResults.AsNoTracking().OrderByDescending(x => x.ReplayId).Take(amount).ToList();
            }
        }

        public IEnumerable<ReplayMatchPlayerScoreResult> ReadRecordsCustomTop(int amount, string columnName, string orderBy)
        {
            if (string.IsNullOrEmpty(columnName) || string.IsNullOrEmpty(orderBy))
                return new List<ReplayMatchPlayerScoreResult>();

            if (columnName.Contains("TimeSpentDead"))
                columnName = string.Concat(columnName, "Ticks");

            if (amount == 0)
                amount = 1;

            using (var db = new ReplaysContext())
            {
                return db.ReplayMatchPlayerScoreResults.SqlQuery($"SELECT * FROM ReplayMatchPlayerScoreResults ORDER BY {columnName} {orderBy} LIMIT {amount}").AsNoTracking().ToList();
            }
        }

        public IEnumerable<ReplayMatchPlayerScoreResult> ReadRecordsWhere(string columnName, string operand, string input)
        {
            if (string.IsNullOrEmpty(columnName) || string.IsNullOrEmpty(operand))
                return new List<ReplayMatchPlayerScoreResult>();

            if (columnName.Contains("TimeSpentDead"))
            {
                if (TimeSpan.TryParse(input, out TimeSpan timeSpan))
                {
                    input = timeSpan.Ticks.ToString();
                    columnName = string.Concat(columnName, "Ticks");
                }
                else
                {
                    return new List<ReplayMatchPlayerScoreResult>();
                }
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
                return db.ReplayMatchPlayerScoreResults.SqlQuery($"SELECT * FROM ReplayMatchPlayerScoreResults WHERE {columnName} {operand} @Input", new SQLiteParameter("@Input", input)).AsNoTracking().ToList();
            }
        }

        public IEnumerable<ReplayMatchPlayerScoreResult> ReadTopRecords(int amount)
        {
            using (var db = new ReplaysContext())
            {
                return db.ReplayMatchPlayerScoreResults.AsNoTracking().Take(amount).ToList();
            }
        }

        internal override long CreateRecord(ReplaysContext db, ReplayMatchPlayerScoreResult model)
        {
            db.ReplayMatchPlayerScoreResults.Add(model);
            db.SaveChanges();

            return model.MatchPlayerScoreResultId;
        }

        internal override long UpdateRecord(ReplaysContext db, ReplayMatchPlayerScoreResult model)
        {
            throw new NotImplementedException();
        }

        internal override bool IsExistingRecord(ReplaysContext db, ReplayMatchPlayerScoreResult model)
        {
            throw new NotImplementedException();
        }
    }
}
