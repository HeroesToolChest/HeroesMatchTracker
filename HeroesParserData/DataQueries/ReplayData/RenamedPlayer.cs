using HeroesParserData.Models.DbModels;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.SQLite;
using System.Linq;

namespace HeroesParserData.DataQueries
{
    public static partial class Query
    {
        internal static class RenamedPlayer
        {
            public static long CreateRecord(HeroesParserDataContext db, ReplayRenamedPlayer replaySamePlayer)
            {
                db.ReplayRenamedPlayers.Add(replaySamePlayer);
                db.SaveChanges();

                return replaySamePlayer.RenamedPlayerId;
            }

            public static List<ReplayRenamedPlayer> ReadTopRecords(int num)
            {
                using (var db = new HeroesParserDataContext())
                {
                    return db.ReplayRenamedPlayers.Take(num).ToList();
                }
            }

            public static List<ReplayRenamedPlayer> ReadLastRecords(int num)
            {
                using (var db = new HeroesParserDataContext())
                {
                    return db.ReplayRenamedPlayers.OrderByDescending(x => x.RenamedPlayerId).Take(num).ToList();
                }
            }

            public static List<ReplayRenamedPlayer> ReadRecordsCustomTop(int count, string columnName, string orderBy)
            {
                if (string.IsNullOrEmpty(columnName) || string.IsNullOrEmpty(orderBy))
                    return new List<ReplayRenamedPlayer>();

                if (columnName.Contains("TimeStamp"))
                    columnName = string.Concat(columnName, "Ticks");

                if (count == 0)
                    count = 1;

                using (var db = new HeroesParserDataContext())
                {
                    return db.ReplayRenamedPlayers.SqlQuery($"SELECT * FROM ReplayRenamedPlayers ORDER BY {columnName} {orderBy} LIMIT {count}").ToList();
                }
            }

            public static List<ReplayRenamedPlayer> ReadRecordsWhere(string columnName, string operand, string input)
            {
                if (string.IsNullOrEmpty(columnName) || string.IsNullOrEmpty(operand))
                    return new List<ReplayRenamedPlayer>();

                if (columnName.Contains("TimeStamp"))
                {
                    TimeSpan timeSpan;
                    if (TimeSpan.TryParse(input, out timeSpan))
                    {
                        input = timeSpan.Ticks.ToString();
                        columnName = string.Concat(columnName, "Ticks");
                    }
                    else
                        return new List<ReplayRenamedPlayer>();
                }
                else if (Utilities.LikeOperatorInputCheck(operand, input))
                    input = $"%{input}%";
                else if (input == null)
                    input = string.Empty;

                using (var db = new HeroesParserDataContext())
                {
                    return db.ReplayRenamedPlayers.SqlQuery($"SELECT * FROM ReplayRenamedPlayers WHERE {columnName} {operand} @Input", new SQLiteParameter("@Input", input)).ToList();
                }
            }
        }
    }
}
