using HeroesMatchTracker.Infrastructure.Database.Contexts;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using System.Data.Common;

namespace HeroesMatchTracker.Tests
{
    public static class DbServices
    {
        /// <summary>
        /// Gets a new <see cref="HeroesReplaysDbContext"/>.
        /// </summary>
        /// <returns>A new context of <see cref="HeroesReplaysDbContext"/>.</returns>
        public static HeroesReplaysDbContext GetHeroesReplaysDbContext()
        {
            HeroesReplaysDbContext dbContext = new HeroesReplaysDbContext(new DbContextOptionsBuilder<HeroesReplaysDbContext>()
                .UseLazyLoadingProxies()
                .UseSqlite(CreateInMemoryDatabase())
                .Options);

            dbContext.Database.EnsureDeleted();
            dbContext.Database.EnsureCreated();

            return dbContext;
        }

        private static DbConnection CreateInMemoryDatabase()
        {
            var connection = new SqliteConnection("Filename=:memory:");

            connection.Open();

            return connection;
        }

        private static void Seed()
        {
            // new
        }
    }
}
