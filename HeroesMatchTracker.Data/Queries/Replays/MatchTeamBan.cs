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
        public IEnumerable<ReplayMatchTeamBan> ReadAllRecords()
        {
            using (var db = new ReplaysContext())
            {
                return db.ReplayMatchTeamBans.AsNoTracking().ToList();
            }
        }

        public IEnumerable<ReplayMatchTeamBan> ReadLastRecords(int amount)
        {
            using (var db = new ReplaysContext())
            {
                return db.ReplayMatchTeamBans.AsNoTracking().OrderByDescending(x => x.ReplayId).Take(amount).ToList();
            }
        }

        public IEnumerable<ReplayMatchTeamBan> ReadRecordsCustomTop(int amount, string columnName, string orderBy)
        {
            if (string.IsNullOrEmpty(columnName) || string.IsNullOrEmpty(orderBy))
                return new List<ReplayMatchTeamBan>();

            if (amount == 0)
                amount = 1;

            using (var db = new ReplaysContext())
            {
                return db.ReplayMatchTeamBans.SqlQuery($"SELECT * FROM ReplayMatchTeamBans ORDER BY {columnName} {orderBy} LIMIT {amount}").AsNoTracking().ToList();
            }
        }

        public IEnumerable<ReplayMatchTeamBan> ReadRecordsWhere(string columnName, string operand, string input)
        {
            if (string.IsNullOrEmpty(columnName) || string.IsNullOrEmpty(operand))
                return new List<ReplayMatchTeamBan>();

            if (LikeOperatorInputCheck(operand, input))
                input = $"%{input}%";
            else if (input == null)
                input = string.Empty;

            using (var db = new ReplaysContext())
            {
                return db.ReplayMatchTeamBans.SqlQuery($"SELECT * FROM ReplayMatchTeamBans WHERE {columnName} {operand} @Input", new SQLiteParameter("@Input", input)).AsNoTracking().ToList();
            }
        }

        public IEnumerable<ReplayMatchTeamBan> ReadTopRecords(int amount)
        {
            using (var db = new ReplaysContext())
            {
                return db.ReplayMatchTeamBans.AsNoTracking().Take(amount).ToList();
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
