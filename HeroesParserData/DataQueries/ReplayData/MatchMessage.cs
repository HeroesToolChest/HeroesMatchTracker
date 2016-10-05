using HeroesParserData.Models.DbModels;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.SQLite;
using System.Linq;
using System.Threading.Tasks;

namespace HeroesParserData.DataQueries
{
    public static partial class Query
    {
        internal static class MatchMessage
        {
            public static long CreateRecord(ReplayMatchMessage replayMatchMessage)
            {
                using (var db = new HeroesParserDataContext())
                {
                    db.ReplayMatchMessages.Add(replayMatchMessage);
                    db.SaveChanges();
                }

                return replayMatchMessage.ReplayId;
            }

            public static long CreateRecord(HeroesParserDataContext db, ReplayMatchMessage replayMatchMessage)
            {
                db.Database.ExecuteSqlCommand(@"INSERT INTO ReplayMatchMessages(ReplayId, MessageEventType, TimeStampTicks, MessageTarget, PlayerName, CharacterName, Message) VALUES 
                                                (@ReplayId, @MessageEventType, @TimeStampTicks, @MessageTarget, @PlayerName, @CharacterName, @Message)",
                                                new SQLiteParameter("@ReplayId", replayMatchMessage.ReplayId),
                                                new SQLiteParameter("@MessageEventType", replayMatchMessage.MessageEventType),
                                                new SQLiteParameter("@TimeStampTicks", replayMatchMessage.TimeStampTicks),
                                                new SQLiteParameter("@MessageTarget", replayMatchMessage.MessageTarget),
                                                new SQLiteParameter("@PlayerName", replayMatchMessage.PlayerName),
                                                new SQLiteParameter("@CharacterName", replayMatchMessage.CharacterName), 
                                                new SQLiteParameter("@Message", replayMatchMessage.Message));

                return replayMatchMessage.ReplayId;
            }

            public static List<ReplayMatchMessage> ReadAllRecords()
            {
                using (var db = new HeroesParserDataContext())
                {
                    return db.ReplayMatchMessages.ToList();
                }
            }

            public static async Task<List<ReplayMatchMessage>> ReadAllRecordsAsync()
            {
                using (var db = new HeroesParserDataContext())
                {
                    return await db.ReplayMatchMessages.ToListAsync();
                }
            }

            public static async Task<List<ReplayMatchMessage>> ReadTopRecordsAsync(int num)
            {
                using (var db = new HeroesParserDataContext())
                {
                    return await db.ReplayMatchMessages.Take(num).ToListAsync();
                }
            }

            public static async Task<List<ReplayMatchMessage>> ReadLastRecordsAsync(int num)
            {
                using (var db = new HeroesParserDataContext())
                {
                    return await db.ReplayMatchMessages.OrderByDescending(x => x.ReplayId).Take(num).ToListAsync();
                }
            }

            public static async Task<List<ReplayMatchMessage>> ReadRecordsCustomTopAsync(int count, string columnName, string orderBy)
            {
                if (string.IsNullOrEmpty(columnName) || string.IsNullOrEmpty(orderBy))
                    return new List<ReplayMatchMessage>();

                if (columnName.Contains("TimeStamp"))
                    columnName = string.Concat(columnName, "Ticks");

                if (count == 0)
                    count = 1;

                using (var db = new HeroesParserDataContext())
                {
                    return await db.ReplayMatchMessages.SqlQuery($"SELECT * FROM ReplayMatchMessages ORDER BY {columnName} {orderBy} LIMIT {count}").ToListAsync();
                }
            }

            public static async Task<List<ReplayMatchMessage>> ReadRecordsWhereAsync(string columnName, string operand, string input)
            {
                if (string.IsNullOrEmpty(columnName) || string.IsNullOrEmpty(operand))
                    return new List<ReplayMatchMessage>();

                if (columnName.Contains("TimeStamp"))
                {
                    TimeSpan timeSpan;
                    if (TimeSpan.TryParse(input, out timeSpan))
                    {
                        input = timeSpan.Ticks.ToString();
                        columnName = string.Concat(columnName, "Ticks");
                    }
                    else
                        return new List<ReplayMatchMessage>();
                }
                else if (Utilities.LikeOperatorInputCheck(operand, input))
                    input = $"%{input}%";
                else if (input == null)
                    input = string.Empty;

                using (var db = new HeroesParserDataContext())
                {
                    return await db.ReplayMatchMessages.SqlQuery($"SELECT * FROM ReplayMatchMessages WHERE {columnName} {operand} @Input", new SQLiteParameter("@Input", input)).ToListAsync();
                }
            }
        }
    }
}
