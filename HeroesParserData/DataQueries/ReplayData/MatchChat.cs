using HeroesParserData.Models.DbModels;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace HeroesParserData.DataQueries.ReplayData
{
    public static partial class Query
    {
        internal static class MatchChat
        {
            public static long CreateRecord(ReplayMatchChat replayMatchChat)
            {
                using (var db = new HeroesParserDataContext())
                {
                    db.ReplayMatchChats.Add(replayMatchChat);
                    db.SaveChanges();
                }

                return replayMatchChat.ReplayId;
            }

            public static long CreateRecord(HeroesParserDataContext db, ReplayMatchChat replayMatchChat)
            {
                db.ReplayMatchChats.Add(replayMatchChat);
                db.SaveChanges();

                return replayMatchChat.ReplayId;
            }

            public static List<ReplayMatchChat> ReadAllRecords()
            {
                using (var db = new HeroesParserDataContext())
                {
                    return db.ReplayMatchChats.ToList();
                }
            }

            public static async Task<List<ReplayMatchChat>> ReadAllRecordsAsync()
            {
                using (var db = new HeroesParserDataContext())
                {
                    return await db.ReplayMatchChats.ToListAsync();
                }
            }

            public static async Task<List<ReplayMatchChat>> ReadTop100RecordsAsync()
            {
                using (var db = new HeroesParserDataContext())
                {
                    return await db.ReplayMatchChats.Take(100).ToListAsync();
                }
            }

            public static async Task<List<ReplayMatchChat>> ReadLast100RecordsAsync()
            {
                using (var db = new HeroesParserDataContext())
                {
                    return await db.ReplayMatchChats.OrderByDescending(x => x.ReplayId).Take(100).ToListAsync();
                }
            }

            public static async Task<List<ReplayMatchChat>> ReadRecordsCustomTopAsync(int count, string columnName, string orderBy)
            {
                if (string.IsNullOrEmpty(columnName) || string.IsNullOrEmpty(orderBy))
                    return new List<ReplayMatchChat>();

                if (count == 0)
                    count = 1;

                using (var db = new HeroesParserDataContext())
                {
                    return await db.ReplayMatchChats.SqlQuery($"SELECT TOP {count} * FROM dbo.ReplayMatchChats ORDER BY {columnName} {orderBy}").ToListAsync();
                }
            }

            public static async Task<List<ReplayMatchChat>> ReadRecordsWhereAsync(string columnName, string operand, string input)
            {
                if (string.IsNullOrEmpty(columnName) || string.IsNullOrEmpty(operand))
                    return new List<ReplayMatchChat>();

                if (input == null)
                    input = string.Empty;

                using (var db = new HeroesParserDataContext())
                {
                    return await db.ReplayMatchChats.SqlQuery($"SELECT * FROM dbo.ReplayMatchChats WHERE {columnName} {operand} @Input", new SqlParameter("@Input", input)).ToListAsync();
                }
            }
        }
    }
}
