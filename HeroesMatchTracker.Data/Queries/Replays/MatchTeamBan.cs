using HeroesMatchTracker.Data.Databases;
using HeroesMatchTracker.Data.Models.Replays;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;

namespace HeroesMatchTracker.Data.Queries.Replays
{
    public class MatchTeamBan : NonContextQueriesBase<ReplayMatchTeamBan>, IRawDataQueries<ReplayMatchTeamBan>
    {
        public List<ReplayMatchTeamBan> ReadAllRecords()
        {
            using (var db = new ReplaysContext())
            {
                return db.ReplayMatchTeamBans.ToList();
            }
        }

        public List<ReplayMatchTeamBan> ReadLastRecords(int amount)
        {
            using (var db = new ReplaysContext())
            {
                return db.ReplayMatchTeamBans.OrderByDescending(x => x.ReplayId).Take(amount).ToList();
            }
        }

        public List<ReplayMatchTeamBan> ReadRecordsCustomTop(int amount, string columnName, string orderBy)
        {
            if (string.IsNullOrEmpty(columnName) || string.IsNullOrEmpty(orderBy))
                return new List<ReplayMatchTeamBan>();

            if (amount == 0)
                amount = 1;

            using (var db = new ReplaysContext())
            {
                return db.ReplayMatchTeamBans.SqlQuery($"SELECT * FROM ReplayMatchTeamBans ORDER BY {columnName} {orderBy} LIMIT {amount}").ToList();
            }
        }

        public List<ReplayMatchTeamBan> ReadRecordsWhere(string columnName, string operand, string input)
        {
            if (string.IsNullOrEmpty(columnName) || string.IsNullOrEmpty(operand))
                return new List<ReplayMatchTeamBan>();

            if (LikeOperatorInputCheck(operand, input))
                input = $"%{input}%";
            else if (input == null)
                input = string.Empty;

            using (var db = new ReplaysContext())
            {
                return db.ReplayMatchTeamBans.SqlQuery($"SELECT * FROM ReplayMatchTeamBans WHERE {columnName} {operand} @Input", new SQLiteParameter("@Input", input)).ToList();
            }
        }

        public List<ReplayMatchTeamBan> ReadTopRecords(int amount)
        {
            using (var db = new ReplaysContext())
            {
                return db.ReplayMatchTeamBans.Take(amount).ToList();
            }
        }

        internal override long CreateRecord(ReplaysContext db, ReplayMatchTeamBan model)
        {
            db.ReplayMatchTeamBans.Add(model);
            db.SaveChanges();

            return 0;
        }

        internal override long UpdateRecord(ReplaysContext db, ReplayMatchTeamBan model)
        {
            throw new NotImplementedException();
        }

        internal override bool IsExistingRecord(ReplaysContext db, ReplayMatchTeamBan model)
        {
            throw new NotImplementedException();
        }
    }
}
