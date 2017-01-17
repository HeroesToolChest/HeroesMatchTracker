using HeroesStatTracker.Data.Databases;
using HeroesStatTracker.Data.Models.Replays;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;

namespace HeroesStatTracker.Data.Queries.Replays
{
    public class MatchPlayerTalent : NonContextQueriesBase<ReplayMatchPlayerTalent>, IRawDataQueries<ReplayMatchPlayerTalent>
    {
        public List<ReplayMatchPlayerTalent> ReadAllRecords()
        {
            using (var db = new ReplaysContext())
            {
                return db.ReplayMatchPlayerTalents.ToList();
            }
        }

        public List<ReplayMatchPlayerTalent> ReadLastRecords(int amount)
        {
            using (var db = new ReplaysContext())
            {
                return db.ReplayMatchPlayerTalents.OrderByDescending(x => x.ReplayId).Take(amount).ToList();
            }
        }

        public List<ReplayMatchPlayerTalent> ReadRecordsCustomTop(int amount, string columnName, string orderBy)
        {
            if (string.IsNullOrEmpty(columnName) || string.IsNullOrEmpty(orderBy))
                return new List<ReplayMatchPlayerTalent>();

            if (columnName.Contains("TimeSpanSelected"))
                columnName = string.Concat(columnName, "Ticks");

            if (amount == 0)
                amount = 1;

            using (var db = new ReplaysContext())
            {
                return db.ReplayMatchPlayerTalents.SqlQuery($"SELECT * FROM ReplayMatchPlayerTalents ORDER BY {columnName} {orderBy} LIMIT {amount}").ToList();
            }
        }

        public List<ReplayMatchPlayerTalent> ReadRecordsWhere(string columnName, string operand, string input)
        {
            if (string.IsNullOrEmpty(columnName) || string.IsNullOrEmpty(operand))
                return new List<ReplayMatchPlayerTalent>();

            if (columnName.Contains("TimeSpanSelected"))
            {
                TimeSpan timeSpan;
                if (TimeSpan.TryParse(input, out timeSpan))
                {
                    input = timeSpan.Ticks.ToString();
                    columnName = string.Concat(columnName, "Ticks");
                }
                else
                {
                    return new List<ReplayMatchPlayerTalent>();
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
                return db.ReplayMatchPlayerTalents.SqlQuery($"SELECT * FROM ReplayMatchPlayerTalents WHERE {columnName} {operand} @Input", new SQLiteParameter("@Input", input)).ToList();
            }
        }

        public List<ReplayMatchPlayerTalent> ReadTopRecords(int amount)
        {
            using (var db = new ReplaysContext())
            {
                return db.ReplayMatchPlayerTalents.Take(amount).ToList();
            }
        }

        internal override long CreateRecord(ReplaysContext db, ReplayMatchPlayerTalent model)
        {
            db.ReplayMatchPlayerTalents.Add(model);
            db.SaveChanges();

            return model.MatchPlayerTalentId;
        }

        internal override bool IsExistingRecord(ReplaysContext db, ReplayMatchPlayerTalent model)
        {
            throw new NotImplementedException();
        }

        internal override long UpdateRecord(ReplaysContext db, ReplayMatchPlayerTalent model)
        {
            throw new NotImplementedException();
        }
    }
}
