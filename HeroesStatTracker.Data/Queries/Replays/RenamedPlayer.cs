using HeroesStatTracker.Data.Databases;
using HeroesStatTracker.Data.Models.Replays;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;

namespace HeroesStatTracker.Data.Queries.Replays
{
    public class RenamedPlayer : NonContextQueriesBase<ReplayRenamedPlayer>, IRawDataQueries<ReplayRenamedPlayer>
    {
        public List<ReplayRenamedPlayer> ReadAllRecords()
        {
            using (var db = new ReplaysContext())
            {
                return db.ReplayRenamedPlayers.ToList();
            }
        }

        public List<ReplayRenamedPlayer> ReadLastRecords(int amount)
        {
            using (var db = new ReplaysContext())
            {
                return db.ReplayRenamedPlayers.OrderByDescending(x => x.RenamedPlayerId).Take(amount).ToList();
            }
        }

        public List<ReplayRenamedPlayer> ReadRecordsCustomTop(int amount, string columnName, string orderBy)
        {
            if (string.IsNullOrEmpty(columnName) || string.IsNullOrEmpty(orderBy))
                return new List<ReplayRenamedPlayer>();

            if (columnName.Contains("TimeStamp"))
                columnName = string.Concat(columnName, "Ticks");

            if (amount == 0)
                amount = 1;

            using (var db = new ReplaysContext())
            {
                return db.ReplayRenamedPlayers.SqlQuery($"SELECT * FROM ReplayRenamedPlayers ORDER BY {columnName} {orderBy} LIMIT {amount}").ToList();
            }
        }

        public List<ReplayRenamedPlayer> ReadRecordsWhere(string columnName, string operand, string input)
        {
            if (string.IsNullOrEmpty(columnName) || string.IsNullOrEmpty(operand))
                return new List<ReplayRenamedPlayer>();

            if (columnName.Contains("TimeStamp"))
            {
                if (TimeSpan.TryParse(input, out TimeSpan timeSpan))
                {
                    input = timeSpan.Ticks.ToString();
                    columnName = string.Concat(columnName, "Ticks");
                }
                else
                {
                    return new List<ReplayRenamedPlayer>();
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
                return db.ReplayRenamedPlayers.SqlQuery($"SELECT * FROM ReplayRenamedPlayers WHERE {columnName} {operand} @Input", new SQLiteParameter("@Input", input)).ToList();
            }
        }

        public List<ReplayRenamedPlayer> ReadTopRecords(int amount)
        {
            using (var db = new ReplaysContext())
            {
                return db.ReplayRenamedPlayers.Take(amount).ToList();
            }
        }

        internal override long CreateRecord(ReplaysContext db, ReplayRenamedPlayer model)
        {
            db.ReplayRenamedPlayers.Add(model);
            db.SaveChanges();

            return model.RenamedPlayerId;
        }

        internal override long UpdateRecord(ReplaysContext db, ReplayRenamedPlayer model)
        {
            throw new NotImplementedException();
        }

        internal override bool IsExistingRecord(ReplaysContext db, ReplayRenamedPlayer model)
        {
            throw new NotImplementedException();
        }
    }
}
