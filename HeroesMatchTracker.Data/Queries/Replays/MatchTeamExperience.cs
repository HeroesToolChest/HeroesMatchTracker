using HeroesMatchTracker.Data.Databases;
using HeroesMatchTracker.Data.Models.Replays;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;

namespace HeroesMatchTracker.Data.Queries.Replays
{
    public class MatchTeamExperience : NonContextQueriesBase<ReplayMatchTeamExperience>, IRawDataQueries<ReplayMatchTeamExperience>
    {
        public IEnumerable<ReplayMatchTeamExperience> ReadAllRecords()
        {
            using (var db = new ReplaysContext())
            {
                return db.ReplayMatchTeamExperiences.AsNoTracking().ToList();
            }
        }

        public IEnumerable<ReplayMatchTeamExperience> ReadLastRecords(int amount)
        {
            using (var db = new ReplaysContext())
            {
                return db.ReplayMatchTeamExperiences.AsNoTracking().OrderByDescending(x => x.ReplayId).Take(amount).ToList();
            }
        }

        public IEnumerable<ReplayMatchTeamExperience> ReadRecordsCustomTop(int amount, string columnName, string orderBy)
        {
            if (string.IsNullOrEmpty(columnName) || string.IsNullOrEmpty(orderBy))
                return new List<ReplayMatchTeamExperience>();

            if (columnName.Contains("Time"))
                columnName = string.Concat(columnName, "Ticks");

            if (amount == 0)
                amount = 1;

            using (var db = new ReplaysContext())
            {
                return db.ReplayMatchTeamExperiences.SqlQuery($"SELECT * FROM ReplayMatchTeamExperiences ORDER BY {columnName} {orderBy} LIMIT {amount}").AsNoTracking().ToList();
            }
        }

        public IEnumerable<ReplayMatchTeamExperience> ReadRecordsWhere(string columnName, string operand, string input)
        {
            if (string.IsNullOrEmpty(columnName) || string.IsNullOrEmpty(operand))
                return new List<ReplayMatchTeamExperience>();

            if (columnName.Contains("Time"))
            {
                if (TimeSpan.TryParse(input, out TimeSpan timeSpan))
                {
                    input = timeSpan.Ticks.ToString();
                    columnName = string.Concat(columnName, "Ticks");
                }
                else
                {
                    return new List<ReplayMatchTeamExperience>();
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
                return db.ReplayMatchTeamExperiences.SqlQuery($"SELECT * FROM ReplayMatchTeamExperiences WHERE {columnName} {operand} @Input", new SQLiteParameter("@Input", input)).AsNoTracking().ToList();
            }
        }

        public IEnumerable<ReplayMatchTeamExperience> ReadTopRecords(int amount)
        {
            using (var db = new ReplaysContext())
            {
                return db.ReplayMatchTeamExperiences.AsNoTracking().Take(amount).ToList();
            }
        }

        internal override long CreateRecord(ReplaysContext db, ReplayMatchTeamExperience model)
        {
            db.ReplayMatchTeamExperiences.Add(model);
            db.SaveChanges();

            return model.ReplayId;
        }

        internal override long UpdateRecord(ReplaysContext db, ReplayMatchTeamExperience model)
        {
            throw new NotImplementedException();
        }

        internal override bool IsExistingRecord(ReplaysContext db, ReplayMatchTeamExperience model)
        {
            throw new NotImplementedException();
        }
    }
}
