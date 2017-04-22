using HeroesMatchTracker.Data.Databases;
using HeroesMatchTracker.Data.Models.Replays;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;

namespace HeroesMatchTracker.Data.Queries.Replays
{
    public class HotsPlayerHero : NonContextQueriesBase<ReplayAllHotsPlayerHero>, IRawDataQueries<ReplayAllHotsPlayerHero>
    {
        public List<ReplayAllHotsPlayerHero> ReadAllRecords()
        {
            using (var db = new ReplaysContext())
            {
                return db.ReplayAllHotsPlayerHeroes.ToList();
            }
        }

        public List<ReplayAllHotsPlayerHero> ReadLastRecords(int amount)
        {
            using (var db = new ReplaysContext())
            {
                return db.ReplayAllHotsPlayerHeroes.OrderByDescending(x => x.PlayerId).Take(amount).ToList();
            }
        }

        public List<ReplayAllHotsPlayerHero> ReadRecordsCustomTop(int amount, string columnName, string orderBy)
        {
            if (string.IsNullOrEmpty(columnName) || string.IsNullOrEmpty(orderBy))
                return new List<ReplayAllHotsPlayerHero>();

            if (amount == 0)
                amount = 1;

            using (var db = new ReplaysContext())
            {
                return db.ReplayAllHotsPlayerHeroes.SqlQuery($"SELECT * FROM ReplayAllHotsPlayerHeroes ORDER BY {columnName} {orderBy} LIMIT {amount}").ToList();
            }
        }

        public List<ReplayAllHotsPlayerHero> ReadRecordsWhere(string columnName, string operand, string input)
        {
            if (string.IsNullOrEmpty(columnName) || string.IsNullOrEmpty(operand))
                return new List<ReplayAllHotsPlayerHero>();

            if (columnName.Contains("Is"))
            {
                if (input.ToUpperInvariant() == "TRUE")
                    input = "1";
                else if (input.ToUpperInvariant() == "FALSE")
                    input = "0";
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
                return db.ReplayAllHotsPlayerHeroes.SqlQuery($"SELECT * FROM ReplayAllHotsPlayerHeroes WHERE {columnName} {operand} @Input", new SQLiteParameter("@Input", input)).ToList();
            }
        }

        public List<ReplayAllHotsPlayerHero> ReadTopRecords(int amount)
        {
            using (var db = new ReplaysContext())
            {
                return db.ReplayAllHotsPlayerHeroes.Take(amount).ToList();
            }
        }

        internal override long CreateRecord(ReplaysContext db, ReplayAllHotsPlayerHero model)
        {
            db.Database.ExecuteSqlCommand(
                "INSERT INTO ReplayAllHotsPlayerHeroes(PlayerId, HeroName, IsUsable, LastUpdated) VALUES (@PlayerId, @HeroName, @IsUsable, @LastUpdated)",
                new SQLiteParameter("@PlayerId", model.PlayerId),
                new SQLiteParameter("@HeroName", model.HeroName),
                new SQLiteParameter("@IsUsable", model.IsUsable),
                new SQLiteParameter("@LastUpdated", model.LastUpdated));

            return 0;
        }

        internal override long UpdateRecord(ReplaysContext db, ReplayAllHotsPlayerHero model)
        {
            var record = db.Database.SqlQuery<ReplayAllHotsPlayerHero>(
                "SELECT * FROM ReplayAllHotsPlayerHeroes WHERE PlayerId=@PlayerId AND HeroName=@HeroName LIMIT 1",
                new SQLiteParameter("@PlayerId", model.PlayerId),
                new SQLiteParameter("@HeroName", model.HeroName)).FirstOrDefault();

            if (record != null)
            {
                if (model.LastUpdated > record.LastUpdated)
                {
                    db.Database.ExecuteSqlCommand(
                        "UPDATE ReplayAllHotsPlayerHeroes SET IsUsable = @IsUsable, LastUpdated = @LastUpdated WHERE PlayerId=@PlayerId AND HeroName=@HeroName",
                        new SQLiteParameter("@IsUsable", model.IsUsable),
                        new SQLiteParameter("@LastUpdated", model.LastUpdated),
                        new SQLiteParameter("@PlayerId", model.PlayerId),
                        new SQLiteParameter("@HeroName", model.HeroName));
                }
            }
            else
            {
                db.ReplayAllHotsPlayerHeroes.Add(model);
                db.SaveChanges();
                return record.PlayerId;
            }

            return 0;
        }

        internal override bool IsExistingRecord(ReplaysContext db, ReplayAllHotsPlayerHero model)
        {
            return db.Database.SqlQuery<bool>(
                "SELECT EXISTS(SELECT 1 FROM ReplayAllHotsPlayerHeroes WHERE PlayerId=@PlayerId AND HeroName=@HeroName LIMIT 1)",
                new SQLiteParameter("@PlayerId", model.PlayerId),
                new SQLiteParameter("@HeroName", model.HeroName)).FirstOrDefault();
        }
    }
}
