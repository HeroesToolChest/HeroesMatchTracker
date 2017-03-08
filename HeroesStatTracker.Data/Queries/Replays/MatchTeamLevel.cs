using HeroesStatTracker.Data.Databases;
using HeroesStatTracker.Data.Models.Replays;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;

namespace HeroesStatTracker.Data.Queries.Replays
{
    public class MatchTeamLevel : NonContextQueriesBase<ReplayMatchTeamLevel>, IRawDataQueries<ReplayMatchTeamLevel>
    {
        public List<ReplayMatchTeamLevel> ReadAllRecords()
        {
            using (var db = new ReplaysContext())
            {
                return db.ReplayMatchTeamLevels.ToList();
            }
        }

        public List<ReplayMatchTeamLevel> ReadLastRecords(int amount)
        {
            using (var db = new ReplaysContext())
            {
                return db.ReplayMatchTeamLevels.OrderByDescending(x => x.ReplayId).Take(amount).ToList();
            }
        }

        public List<ReplayMatchTeamLevel> ReadRecordsCustomTop(int amount, string columnName, string orderBy)
        {
            if (string.IsNullOrEmpty(columnName) || string.IsNullOrEmpty(orderBy))
                return new List<ReplayMatchTeamLevel>();

            if (columnName.Contains("TeamTime"))
                columnName = string.Concat(columnName, "Ticks");

            if (amount == 0)
                amount = 1;

            using (var db = new ReplaysContext())
            {
                return db.ReplayMatchTeamLevels.SqlQuery($"SELECT * FROM ReplayMatchTeamLevels ORDER BY {columnName} {orderBy} LIMIT {amount}").ToList();
            }
        }

        public List<ReplayMatchTeamLevel> ReadRecordsWhere(string columnName, string operand, string input)
        {
            if (string.IsNullOrEmpty(columnName) || string.IsNullOrEmpty(operand))
                return new List<ReplayMatchTeamLevel>();

            if (columnName.Contains("TeamTime"))
            {
                if (TimeSpan.TryParse(input, out TimeSpan timeSpan))
                {
                    input = timeSpan.Ticks.ToString();
                    columnName = string.Concat(columnName, "Ticks");
                }
                else
                {
                    return new List<ReplayMatchTeamLevel>();
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
                return db.ReplayMatchTeamLevels.SqlQuery($"SELECT * FROM ReplayMatchTeamLevels WHERE {columnName} {operand} @Input", new SQLiteParameter("@Input", input)).ToList();
            }
        }

        public List<ReplayMatchTeamLevel> ReadTopRecords(int amount)
        {
            using (var db = new ReplaysContext())
            {
                return db.ReplayMatchTeamLevels.Take(amount).ToList();
            }
        }

        internal override long CreateRecord(ReplaysContext db, ReplayMatchTeamLevel model)
        {
            db.ReplayMatchTeamLevels.Add(model);
            db.SaveChanges();

            return model.ReplayId;
        }

        internal override long UpdateRecord(ReplaysContext db, ReplayMatchTeamLevel model)
        {
            throw new NotImplementedException();
        }

        internal override bool IsExistingRecord(ReplaysContext db, ReplayMatchTeamLevel model)
        {
            throw new NotImplementedException();
        }
    }
}
