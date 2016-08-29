using HeroesParserData.Models.DbModels;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HeroesParserData.DataQueries.ReplayData
{
    public static partial class Query
    {
        internal static class HotsPlayerHero
        {
            public static long CreateRecord(HeroesParserDataContext db, ReplayAllHotsPlayerHero replayAllHotsPlayerHero)
            {
                db.ReplayAllHotsPlayerHeroes.Add(replayAllHotsPlayerHero);
                db.SaveChanges();

                return replayAllHotsPlayerHero.PlayerId;
            }

            public static async Task<List<ReplayAllHotsPlayerHero>> ReadTopRecordsAsync(int num)
            {
                using (var db = new HeroesParserDataContext())
                {
                    return await db.ReplayAllHotsPlayerHeroes.Take(num).ToListAsync();
                }
            }

            public static async Task<List<ReplayAllHotsPlayerHero>> ReadLastRecordsAsync(int num)
            {
                using (var db = new HeroesParserDataContext())
                {
                    return await db.ReplayAllHotsPlayerHeroes.OrderByDescending(x => x.PlayerId).Take(num).ToListAsync();
                }
            }

            public static async Task<List<ReplayAllHotsPlayerHero>> ReadRecordsCustomTopAsync(int count, string columnName, string orderBy)
            {
                if (string.IsNullOrEmpty(columnName) || string.IsNullOrEmpty(orderBy))
                    return new List<ReplayAllHotsPlayerHero>();

                if (count == 0)
                    count = 1;

                using (var db = new HeroesParserDataContext())
                {
                    return await db.ReplayAllHotsPlayerHeroes.SqlQuery($"SELECT * FROM ReplayAllHotsPlayerHeroes ORDER BY {columnName} {orderBy} LIMIT {count}").ToListAsync();
                }
            }

            public static async Task<List<ReplayAllHotsPlayerHero>> ReadRecordsWhereAsync(string columnName, string operand, string input)
            {
                if (string.IsNullOrEmpty(columnName) || string.IsNullOrEmpty(operand))
                    return new List<ReplayAllHotsPlayerHero>();

                if (input == null)
                    input = string.Empty;

                using (var db = new HeroesParserDataContext())
                {
                    return await db.ReplayAllHotsPlayerHeroes.SqlQuery($"SELECT * FROM ReplayAllHotsPlayerHeroes WHERE {columnName} {operand} @Input", new SQLiteParameter("@Input", input)).ToListAsync();
                }
            }
        }
    }
}
