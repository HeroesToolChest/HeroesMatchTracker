using HeroesMatchTracker.Infrastructure.Database.Contexts;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace HeroesMatchTracker.Infrastructure.Tests
{
    public static class DbServices
    {
        /// <summary>
        /// Gets a new <see cref="HeroesReplaysDbContext"/>.
        /// </summary>
        public static HeroesReplaysDbContext GetHeroesReplaysDbContext()
        {
            HeroesReplaysDbContext dbContext = new HeroesReplaysDbContext(new DbContextOptionsBuilder<HeroesReplaysDbContext>()
                .UseLazyLoadingProxies()
                .UseSqlite(new SqliteConnection(DbConnectionString.HeroesReplays))
                .Options);

            dbContext.Database.EnsureCreated();

            return dbContext;
        }

        private static void Seed()
        {
            // new 
        }
    }
}
