using HeroesParserData.Models.DbModels;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.SQLite;
using System.Linq;
using System.Threading.Tasks;

namespace HeroesParserData.DataQueries.ReplayData
{
    public static partial class Query
    {
        internal static class HotsPlayerHero
        {
            public static void CreateRecord(HeroesParserDataContext db, ReplayAllHotsPlayerHero replayAllHotsPlayerHero)
            {
                db.Database.ExecuteSqlCommand("INSERT INTO ReplayAllHotsPlayerHeroes(ReplayId, PlayerId, HeroName, IsUsable) VALUES (@ReplayId, @PlayerId, @HeroName, @IsUsable)", 
                                                new SQLiteParameter("@ReplayId", replayAllHotsPlayerHero.ReplayId),
                                                new SQLiteParameter("@PlayerId", replayAllHotsPlayerHero.PlayerId),
                                                new SQLiteParameter("@HeroName", replayAllHotsPlayerHero.HeroName),
                                                new SQLiteParameter("@IsUsable", replayAllHotsPlayerHero.IsUsable));
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

                if (columnName.Contains("Is"))
                {
                    if (input.ToUpperInvariant() == "TRUE")
                        input = "1";
                    else if (input.ToUpperInvariant() == "FALSE")
                        input = "0";
                }

                if (input == null)
                    input = string.Empty;

                using (var db = new HeroesParserDataContext())
                {
                    return await db.ReplayAllHotsPlayerHeroes.SqlQuery($"SELECT * FROM ReplayAllHotsPlayerHeroes WHERE {columnName} {operand} @Input", new SQLiteParameter("@Input", input)).ToListAsync();
                }
            }

            public static List<ReplayAllHotsPlayerHero> ReadListOfHeroRecordsForPlayerId(HeroesParserDataContext db, long playerId)
            {
                return db.ReplayAllHotsPlayerHeroes.Where(x => x.PlayerId == playerId).ToList();
            }

            public static void UpdateRecord(HeroesParserDataContext db, ReplayAllHotsPlayerHero replayAllHotsPlayersHero)
            {
                var record = db.Database.SqlQuery<ReplayAllHotsPlayerHero>("SELECT * FROM ReplayAllHotsPlayerHeroes WHERE PlayerId=@PlayerId AND HeroName=@HeroName LIMIT 1",
                                                                new SQLiteParameter("@PlayerId", replayAllHotsPlayersHero.PlayerId),
                                                                new SQLiteParameter("@HeroName", replayAllHotsPlayersHero.HeroName)).ToList();

                if (record != null && record.Count > 0)
                {
                    db.Database.ExecuteSqlCommand("UPDATE ReplayAllHotsPlayerHeroes SET IsUsable = @IsUsable WHERE PlayerId=@PlayerId AND HeroName=@HeroName",
                                                    new SQLiteParameter("@IsUsable", replayAllHotsPlayersHero.IsUsable),
                                                    new SQLiteParameter("@PlayerId", replayAllHotsPlayersHero.PlayerId),
                                                    new SQLiteParameter("@HeroName", replayAllHotsPlayersHero.HeroName));
                }
                else
                {
                    db.ReplayAllHotsPlayerHeroes.Add(replayAllHotsPlayersHero);
                    db.SaveChanges();
                }
            }
        }
    }
}
