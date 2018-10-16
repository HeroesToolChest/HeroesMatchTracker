using HeroesMatchTracker.Data.Databases;
using HeroesMatchTracker.Data.Models.Replays;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;

namespace HeroesMatchTracker.Data.Queries.Replays
{
    public class MatchDraft : NonContextQueriesBase<ReplayMatchDraftPick>, IRawDataQueries<ReplayMatchDraftPick>
    {
        public IEnumerable<ReplayMatchDraftPick> ReadAllRecords()
        {
            using (var db = new ReplaysContext())
            {
                return db.ReplayMatchDrafts.AsNoTracking().ToList();
            }
        }

        public IEnumerable<ReplayMatchDraftPick> ReadLastRecords(int amount)
        {
            using (var db = new ReplaysContext())
            {
                return db.ReplayMatchDrafts.AsNoTracking().OrderByDescending(x => x.ReplayId).Take(amount).ToList();
            }
        }

        public IEnumerable<ReplayMatchDraftPick> ReadRecordsCustomTop(int amount, string columnName, string orderBy)
        {
            if (string.IsNullOrEmpty(columnName) || string.IsNullOrEmpty(orderBy))
                return new List<ReplayMatchDraftPick>();

            if (amount == 0)
                amount = 1;

            using (var db = new ReplaysContext())
            {
                return db.ReplayMatchDrafts.SqlQuery($"SELECT * FROM ReplayMatchDrafts ORDER BY {columnName} {orderBy} LIMIT {amount}").AsNoTracking().ToList();
            }
        }

        public IEnumerable<ReplayMatchDraftPick> ReadRecordsWhere(string columnName, string operand, string input)
        {
            if (string.IsNullOrEmpty(columnName) || string.IsNullOrEmpty(operand))
                return new List<ReplayMatchDraftPick>();

            if (LikeOperatorInputCheck(operand, input))
            {
                input = $"%{input}%";
            }
            else if (input == null)
            {
                input = string.Empty;
            }

            using (var db = new ReplaysContext())
            {
                return db.ReplayMatchDrafts.SqlQuery($"SELECT * FROM ReplayMatchDrafts WHERE {columnName} {operand} @Input", new SQLiteParameter("@Input", input)).AsNoTracking().ToList();
            }
        }

        public IEnumerable<ReplayMatchDraftPick> ReadTopRecords(int amount)
        {
            using (var db = new ReplaysContext())
            {
                return db.ReplayMatchDrafts.AsNoTracking().Take(amount).ToList();
            }
        }

        internal override long CreateRecord(ReplaysContext db, ReplayMatchDraftPick model)
        {
            db.ReplayMatchDrafts.Add(model);
            db.SaveChanges();

            return model.ReplayId;
        }

        internal override bool IsExistingRecord(ReplaysContext db, ReplayMatchDraftPick model)
        {
            throw new NotImplementedException();
        }

        internal override long UpdateRecord(ReplaysContext db, ReplayMatchDraftPick model)
        {
            throw new NotImplementedException();
        }
    }
}
