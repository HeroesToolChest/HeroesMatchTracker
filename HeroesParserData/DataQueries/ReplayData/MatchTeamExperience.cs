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
        internal static class MatchTeamExperience
        {
            public static long CreateRecord(ReplayMatchTeamExperience replayMatchTeamExperience)
            {
                using (var db = new HeroesParserDataContext())
                {
                    db.ReplayMatchTeamExperiences.Add(replayMatchTeamExperience);
                    db.SaveChanges();
                }

                return replayMatchTeamExperience.ReplayId;
            }

            public static long CreateRecord(HeroesParserDataContext db, ReplayMatchTeamExperience replayMatchTeamExperience)
            {
                db.ReplayMatchTeamExperiences.Add(replayMatchTeamExperience);
                db.SaveChanges();

                return replayMatchTeamExperience.ReplayId;
            }

            public static List<ReplayMatchTeamExperience> ReadAllRecords()
            {
                using (var db = new HeroesParserDataContext())
                {
                    return db.ReplayMatchTeamExperiences.ToList();
                }
            }

            public static async Task<List<ReplayMatchTeamExperience>> ReadAllRecordsAsync()
            {
                using (var db = new HeroesParserDataContext())
                {
                    return await db.ReplayMatchTeamExperiences.ToListAsync();
                }
            }

            public static async Task<List<ReplayMatchTeamExperience>> ReadTop100RecordsAsync()
            {
                using (var db = new HeroesParserDataContext())
                {
                    return await db.ReplayMatchTeamExperiences.Take(100).ToListAsync();
                }
            }

            public static async Task<List<ReplayMatchTeamExperience>> ReadLast100RecordsAsync()
            {
                using (var db = new HeroesParserDataContext())
                {
                    return await db.ReplayMatchTeamExperiences.OrderByDescending(x => x.ReplayId).Take(100).ToListAsync();
                }
            }

            public static async Task<List<ReplayMatchTeamExperience>> ReadRecordsCustomTopAsync(int count, string columnName, string orderBy)
            {
                if (string.IsNullOrEmpty(columnName) || string.IsNullOrEmpty(orderBy))
                    return new List<ReplayMatchTeamExperience>();

                if (count == 0)
                    count = 1;

                using (var db = new HeroesParserDataContext())
                {
                    return await db.ReplayMatchTeamExperiences.SqlQuery($"SELECT TOP {count} * FROM dbo.ReplayMatchTeamExperiences ORDER BY {columnName} {orderBy}").ToListAsync();
                }
            }

            public static async Task<List<ReplayMatchTeamExperience>> ReadRecordsWhereAsync(string columnName, string operand, string input)
            {
                if (string.IsNullOrEmpty(columnName) || string.IsNullOrEmpty(operand))
                    return new List<ReplayMatchTeamExperience>();

                if (input == null)
                    input = string.Empty;

                using (var db = new HeroesParserDataContext())
                {
                    return await db.ReplayMatchTeamExperiences.SqlQuery($"SELECT * FROM dbo.ReplayMatchTeamExperiences WHERE {columnName} {operand} @Input", new SqlParameter("@Input", input)).ToListAsync();
                }
            }
        }
    }
}
