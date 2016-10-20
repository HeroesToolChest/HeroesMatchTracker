using HeroesParserData.Models.DbModels;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.SQLite;
using System.Linq;

namespace HeroesParserData.DataQueries
{
    public static partial class Query
    {
        internal static class MatchPlayer
        {
            public static long CreateRecord(ReplayMatchPlayer replayMatchPlayer)
            {
                using (var db = new HeroesParserDataContext())
                {
                    db.ReplayMatchPlayers.Add(replayMatchPlayer);
                    db.SaveChanges();
                }

                return replayMatchPlayer.MatchPlayerId;
            }

            public static long CreateRecord(HeroesParserDataContext db, ReplayMatchPlayer replayPlayer)
            {
                db.ReplayMatchPlayers.Add(replayPlayer);
                db.SaveChanges();

                return replayPlayer.MatchPlayerId;
            }

            public static List<ReplayMatchPlayer> ReadAllRecords()
            {
                using (var db = new HeroesParserDataContext())
                {
                    return db.ReplayMatchPlayers.ToList();
                }
            }

            public static List<ReplayMatchPlayer> ReadTopRecords(int num)
            {
                using (var db = new HeroesParserDataContext())
                {
                    return db.ReplayMatchPlayers.Take(num).ToList();
                }
            }

            public static List<ReplayMatchPlayer> ReadLastRecords(int num)
            {
                using (var db = new HeroesParserDataContext())
                {
                    return db.ReplayMatchPlayers.OrderByDescending(x => x.ReplayId).Take(num).ToList();
                }
            }

            public static List<ReplayMatchPlayer> ReadRecordsCustomTop(int count, string columnName, string orderBy)
            {
                if (string.IsNullOrEmpty(columnName) || string.IsNullOrEmpty(orderBy))
                    return new List<ReplayMatchPlayer>();

                if (count == 0)
                    count = 1;

                using (var db = new HeroesParserDataContext())
                {
                    return db.ReplayMatchPlayers.SqlQuery($"SELECT * FROM ReplayMatchPlayers ORDER BY {columnName} {orderBy} LIMIT {count}").ToList();
                }
            }

            public static List<ReplayMatchPlayer> ReadRecordsWhere(string columnName, string operand, string input)
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
                else if (Utilities.LikeOperatorInputCheck(operand, input))
                    input = $"%{input}%";
                else if (input == null)
                    input = string.Empty;

                using (var db = new HeroesParserDataContext())
                {
                    return db.ReplayMatchPlayers.SqlQuery($"SELECT * FROM ReplayMatchPlayers WHERE {columnName} {operand} @Input", new SQLiteParameter("@Input", input)).ToList();
                }
            }
        }
    }
}
