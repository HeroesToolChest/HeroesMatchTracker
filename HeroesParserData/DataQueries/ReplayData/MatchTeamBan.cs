using HeroesParserData.Models.DbModels;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.SQLite;
using System.Linq;

namespace HeroesParserData.DataQueries
{
    public static partial class Query
    {
        internal static class MatchTeamBan
        {
            public static void CreateRecord(ReplayMatchTeamBan replayMatchTeamBan)
            {
                using (var db = new HeroesParserDataContext())
                {
                    db.ReplayMatchTeamBans.Add(replayMatchTeamBan);
                    db.SaveChanges();
                }
            }

            public static void CreateRecord(HeroesParserDataContext db, ReplayMatchTeamBan replayMatchTeamBan)
            {
                db.ReplayMatchTeamBans.Add(replayMatchTeamBan);
                db.SaveChanges();
            }

            public static List<ReplayMatchTeamBan> ReadAllRecords()
            {
                using (var db = new HeroesParserDataContext())
                {
                    return db.ReplayMatchTeamBans.ToList();
                }
            }

            public static List<ReplayMatchTeamBan> ReadTopRecords(int num)
            {
                using (var db = new HeroesParserDataContext())
                {
                    return db.ReplayMatchTeamBans.Take(num).ToList();
                }
            }

            public static List<ReplayMatchTeamBan> ReadLastRecords(int num)
            {
                using (var db = new HeroesParserDataContext())
                {
                    return db.ReplayMatchTeamBans.OrderByDescending(x => x.ReplayId).Take(num).ToList();
                }
            }

            public static List<ReplayMatchTeamBan> ReadRecordsCustomTop(int count, string columnName, string orderBy)
            {
                if (string.IsNullOrEmpty(columnName) || string.IsNullOrEmpty(orderBy))
                    return new List<ReplayMatchTeamBan>();

                if (count == 0)
                    count = 1;

                using (var db = new HeroesParserDataContext())
                {
                    return db.ReplayMatchTeamBans.SqlQuery($"SELECT * FROM ReplayMatchTeamBans ORDER BY {columnName} {orderBy} LIMIT {count}").ToList();
                }
            }

            public static List<ReplayMatchTeamBan> ReadRecordsWhere(string columnName, string operand, string input)
            {
                if (string.IsNullOrEmpty(columnName) || string.IsNullOrEmpty(operand))
                    return new List<ReplayMatchTeamBan>();

                if (Utilities.LikeOperatorInputCheck(operand, input))
                    input = $"%{input}%";
                else if (input == null)
                    input = string.Empty;

                using (var db = new HeroesParserDataContext())
                {
                    return db.ReplayMatchTeamBans.SqlQuery($"SELECT * FROM ReplayMatchTeamBans WHERE {columnName} {operand} @Input", new SQLiteParameter("@Input", input)).ToList();
                }
            }
        }
    }
}
