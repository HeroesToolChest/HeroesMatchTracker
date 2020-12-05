using HeroesMatchTracker.Infrastructure.Database.Contexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace HeroesMatchTracker.Infrastructure.Contexts.Database.Design
{
    /// <summary>
    /// For design-time only. Used for ef migration command.
    /// </summary>
    public class HeroesReplaysDbContextFactory : IDesignTimeDbContextFactory<HeroesReplaysDbContext>
    {
        public HeroesReplaysDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<HeroesReplaysDbContext>();
            optionsBuilder.UseSqlite(DbConnectionString.HeroesReplays);

            return new HeroesReplaysDbContext(optionsBuilder.Options);
        }
    }
}
