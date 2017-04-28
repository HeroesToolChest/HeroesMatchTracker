using HeroesMatchTracker.Data.Databases;
using HeroesMatchTracker.Data.Models.Replays;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;

namespace HeroesMatchTracker.Data.Queries.Replays
{
    public class MatchAward : NonContextQueriesBase<ReplayMatchAward>, IRawDataQueries<ReplayMatchAward>
    {
        public List<ReplayMatchAward> ReadAllRecords()
        {
            using (var db = new ReplaysContext())
            {
                return db.ReplayMatchAwards.ToList();
            }
        }

        public List<ReplayMatchAward> ReadLastRecords(int amount)
        {
            using (var db = new ReplaysContext())
            {
                return db.ReplayMatchAwards.OrderByDescending(x => x.ReplayId).Take(amount).ToList();
            }
        }

        public List<ReplayMatchAward> ReadRecordsCustomTop(int amount, string columnName, string orderBy)
        {
            if (string.IsNullOrEmpty(columnName) || string.IsNullOrEmpty(orderBy))
                return new List<ReplayMatchAward>();

            if (amount == 0)
                amount = 1;

            using (var db = new ReplaysContext())
            {
                return db.ReplayMatchAwards.SqlQuery($"SELECT * FROM ReplayMatchAwards ORDER BY {columnName} {orderBy} LIMIT {amount}").ToList();
            }
        }

        public List<ReplayMatchAward> ReadRecordsWhere(string columnName, string operand, string input)
        {
            if (string.IsNullOrEmpty(columnName) || string.IsNullOrEmpty(operand))
                return new List<ReplayMatchAward>();

            if (LikeOperatorInputCheck(operand, input))
                input = $"%{input}%";
            else if (input == null)
                input = string.Empty;

            using (var db = new ReplaysContext())
            {
                return db.ReplayMatchAwards.SqlQuery($"SELECT * FROM ReplayMatchAwards WHERE {columnName} {operand} @Input", new SQLiteParameter("@Input", input)).ToList();
            }
        }

        public List<ReplayMatchAward> ReadTopRecords(int amount)
        {
            using (var db = new ReplaysContext())
            {
                return db.ReplayMatchAwards.Take(amount).ToList();
            }
        }

        internal override long CreateRecord(ReplaysContext db, ReplayMatchAward model)
        {
            db.ReplayMatchAwards.Add(model);
            db.SaveChanges();

            return model.PlayerId;
        }

        internal override bool IsExistingRecord(ReplaysContext db, ReplayMatchAward model)
        {
            throw new NotImplementedException();
        }

        internal override long UpdateRecord(ReplaysContext db, ReplayMatchAward model)
        {
            throw new NotImplementedException();
        }
    }
}
