using HeroesMatchTracker.Data.Databases;
using HeroesMatchTracker.Data.Models.Replays;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;

namespace HeroesMatchTracker.Data.Queries.Replays
{
    public class MatchMessage : NonContextQueriesBase<ReplayMatchMessage>, IRawDataQueries<ReplayMatchMessage>
    {
        public IEnumerable<ReplayMatchMessage> ReadAllRecords()
        {
            using (var db = new ReplaysContext())
            {
                return db.ReplayMatchMessages.AsNoTracking().ToList();
            }
        }

        public IEnumerable<ReplayMatchMessage> ReadLastRecords(int amount)
        {
            using (var db = new ReplaysContext())
            {
                return db.ReplayMatchMessages.AsNoTracking().OrderByDescending(x => x.ReplayId).Take(amount).ToList();
            }
        }

        public IEnumerable<ReplayMatchMessage> ReadRecordsCustomTop(int amount, string columnName, string orderBy)
        {
            if (string.IsNullOrEmpty(columnName) || string.IsNullOrEmpty(orderBy))
                return new List<ReplayMatchMessage>();

            if (columnName.Contains("TimeStamp"))
                columnName = string.Concat(columnName, "Ticks");

            if (amount == 0)
                amount = 1;

            using (var db = new ReplaysContext())
            {
                return db.ReplayMatchMessages.SqlQuery($"SELECT * FROM ReplayMatchMessages ORDER BY {columnName} {orderBy} LIMIT {amount}").AsNoTracking().ToList();
            }
        }

        public IEnumerable<ReplayMatchMessage> ReadRecordsWhere(string columnName, string operand, string input)
        {
            if (string.IsNullOrEmpty(columnName) || string.IsNullOrEmpty(operand))
                return new List<ReplayMatchMessage>();

            if (columnName.Contains("TimeStamp"))
            {
                if (TimeSpan.TryParse(input, out TimeSpan timeSpan))
                {
                    input = timeSpan.Ticks.ToString();
                    columnName = string.Concat(columnName, "Ticks");
                }
                else
                {
                    return new List<ReplayMatchMessage>();
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
                return db.ReplayMatchMessages.SqlQuery($"SELECT * FROM ReplayMatchMessages WHERE {columnName} {operand} @Input", new SQLiteParameter("@Input", input)).AsNoTracking().ToList();
            }
        }

        public IEnumerable<ReplayMatchMessage> ReadTopRecords(int amount)
        {
            using (var db = new ReplaysContext())
            {
                return db.ReplayMatchMessages.AsNoTracking().Take(amount).ToList();
            }
        }

        internal override long CreateRecord(ReplaysContext db, ReplayMatchMessage model)
        {
            db.Database.ExecuteSqlCommand(
                @"INSERT INTO ReplayMatchMessages(ReplayId, MessageEventType, TimeStampTicks, MessageTarget, PlayerName, CharacterName, Message) VALUES 
                                                (@ReplayId, @MessageEventType, @TimeStampTicks, @MessageTarget, @PlayerName, @CharacterName, @Message)",
                new SQLiteParameter("@ReplayId", model.ReplayId),
                new SQLiteParameter("@MessageEventType", model.MessageEventType),
                new SQLiteParameter("@TimeStampTicks", model.TimeStampTicks),
                new SQLiteParameter("@MessageTarget", model.MessageTarget),
                new SQLiteParameter("@PlayerName", model.PlayerName),
                new SQLiteParameter("@CharacterName", model.CharacterName),
                new SQLiteParameter("@Message", model.Message));

            return model.ReplayId;
        }

        internal override long UpdateRecord(ReplaysContext db, ReplayMatchMessage model)
        {
            throw new NotImplementedException();
        }

        internal override bool IsExistingRecord(ReplaysContext db, ReplayMatchMessage model)
        {
            throw new NotImplementedException();
        }
    }
}
