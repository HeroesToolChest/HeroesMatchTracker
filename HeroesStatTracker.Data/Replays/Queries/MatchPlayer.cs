using HeroesStatTracker.Data.Databases;
using HeroesStatTracker.Data.Replays.Models;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;

namespace HeroesStatTracker.Data.Replays.Queries
{
    public class MatchPlayer : ReplayDataTablesBase<ReplayMatchPlayer>, IRawQueries<ReplayMatchPlayer>
    {
        internal MatchPlayer() { }

        internal override long CreateRecord(ReplaysContext db, ReplayMatchPlayer model)
        {
            db.ReplayMatchPlayers.Add(model);
            db.SaveChanges();

            return model.MatchPlayerId;
        }

        internal override long UpdateRecord(ReplaysContext db, ReplayMatchPlayer model)
        {
            throw new NotImplementedException();
        }

        internal override bool IsExistingRecord(ReplaysContext db, ReplayMatchPlayer model)
        {
            throw new NotImplementedException();
        }

        public List<ReplayMatchPlayer> ReadLastRecords(int amount)
        {
            using (var db = new ReplaysContext())
            {
                return db.ReplayMatchPlayers.OrderByDescending(x => x.ReplayId).Take(amount).ToList();
            }
        }

        public List<ReplayMatchPlayer> ReadRecordsCustomTop(int amount, string columnName, string orderBy)
        {
            if (string.IsNullOrEmpty(columnName) || string.IsNullOrEmpty(orderBy))
                return new List<ReplayMatchPlayer>();

            if (amount == 0)
                amount = 1;

            using (var db = new ReplaysContext())
            {
                return db.ReplayMatchPlayers.SqlQuery($"SELECT * FROM ReplayMatchPlayers ORDER BY {columnName} {orderBy} LIMIT {amount}").ToList();
            }
        }

        public List<ReplayMatchPlayer> ReadRecordsWhere(string columnName, string operand, string input)
        {
            if (string.IsNullOrEmpty(columnName) || string.IsNullOrEmpty(operand))
                return new List<ReplayMatchPlayer>();

            if (columnName.Contains("Is"))
            {
                if (input.ToUpperInvariant() == "TRUE")
                    input = "1";
                else if (input.ToUpperInvariant() == "FALSE")
                    input = "0";
            }
            else if (LikeOperatorInputCheck(operand, input))
                input = $"%{input}%";
            else if (input == null)
                input = string.Empty;

            using (var db = new ReplaysContext())
            {
                return db.ReplayMatchPlayers.SqlQuery($"SELECT * FROM ReplayMatchPlayers WHERE {columnName} {operand} @Input", new SQLiteParameter("@Input", input)).ToList();
            }
        }

        public List<ReplayMatchPlayer> ReadTopRecords(int amount)
        {
            using (var db = new ReplaysContext())
            {
                return db.ReplayMatchPlayers.Take(amount).ToList();
            }
        }
    }
}
