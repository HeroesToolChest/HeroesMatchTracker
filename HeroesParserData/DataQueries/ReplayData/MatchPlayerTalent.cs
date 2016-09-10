using HeroesParserData.Models.DbModels;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.SQLite;
using System.Linq;
using System.Threading.Tasks;

namespace HeroesParserData.DataQueries.ReplayData
{
    public static partial class Query
    {
        internal static class MatchPlayerTalent
        {
            public static long CreateRecord(ReplayMatchPlayerTalent replayMatchPlayerTalent)
            {
                using (var db = new HeroesParserDataContext())
                {
                    db.ReplayMatchPlayerTalents.Add(replayMatchPlayerTalent);
                    db.SaveChanges();
                }

                return replayMatchPlayerTalent.MatchPlayerTalentId;
            }

            public static long CreateRecord(HeroesParserDataContext db, ReplayMatchPlayerTalent replayMatchPlayerTalent)
            {
                db.ReplayMatchPlayerTalents.Add(replayMatchPlayerTalent);
                db.SaveChanges();

                return replayMatchPlayerTalent.MatchPlayerTalentId;
            }

            public static List<ReplayMatchPlayerTalent> ReadAllRecords()
            {
                using (var db = new HeroesParserDataContext())
                {
                    return db.ReplayMatchPlayerTalents.ToList();
                }
            }

            public static async Task<List<ReplayMatchPlayerTalent>> ReadAllRecordsAsync()
            {
                using (var db = new HeroesParserDataContext())
                {
                    return await db.ReplayMatchPlayerTalents.ToListAsync();
                }
            }

            public static async Task<List<ReplayMatchPlayerTalent>> ReadTopRecordsAsync(int num)
            {
                using (var db = new HeroesParserDataContext())
                {
                    return await db.ReplayMatchPlayerTalents.Take(num).ToListAsync();
                }
            }

            public static async Task<List<ReplayMatchPlayerTalent>> ReadLastRecordsAsync(int num)
            {
                using (var db = new HeroesParserDataContext())
                {
                    return await db.ReplayMatchPlayerTalents.OrderByDescending(x => x.ReplayId).Take(num).ToListAsync();
                }
            }

            public static async Task<List<ReplayMatchPlayerTalent>> ReadRecordsCustomTopAsync(int count, string columnName, string orderBy)
            {
                if (string.IsNullOrEmpty(columnName) || string.IsNullOrEmpty(orderBy))
                    return new List<ReplayMatchPlayerTalent>();

                if (columnName.Contains("TimeSpanSelected"))
                    columnName = string.Concat(columnName, "Ticks");

                if (count == 0)
                    count = 1;

                using (var db = new HeroesParserDataContext())
                {
                    return await db.ReplayMatchPlayerTalents.SqlQuery($"SELECT * FROM ReplayMatchPlayerTalents ORDER BY {columnName} {orderBy} LIMIT {count}").ToListAsync();
                }
            }

            public static async Task<List<ReplayMatchPlayerTalent>> ReadRecordsWhereAsync(string columnName, string operand, string input)
            {
                if (string.IsNullOrEmpty(columnName) || string.IsNullOrEmpty(operand))
                    return new List<ReplayMatchPlayerTalent>();

                if (columnName.Contains("TimeSpanSelected"))
                {
                    TimeSpan timeSpan;
                    if (TimeSpan.TryParse(input, out timeSpan))
                    {
                        input = timeSpan.Ticks.ToString();
                        columnName = string.Concat(columnName, "Ticks");
                    }
                    else
                        return new List<ReplayMatchPlayerTalent>();
                }
                else if (Utilities.LikeOperatorInputCheck(operand, input))
                    input = $"%{input}%";
                else if (input == null)
                    input = string.Empty;

                using (var db = new HeroesParserDataContext())
                {
                    return await db.ReplayMatchPlayerTalents.SqlQuery($"SELECT * FROM ReplayMatchPlayerTalents WHERE {columnName} {operand} @Input", new SQLiteParameter("@Input", input)).ToListAsync();
                }
            }

            public static async Task<List<ReplayMatchPlayerTalent>> ReadRecordsByReplayId(long replayId)
            {
                using (var db = new HeroesParserDataContext())
                {
                    return await db.ReplayMatchPlayerTalents.Where(x => x.ReplayId == replayId).ToListAsync();
                }
            }
        }
    }
}
