using HeroesMatchTracker.Infrastructure.Database.Contexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace HeroesMatchTracker.Infrastructure.Database.Design
{
    /// <summary>
    /// For design-time only. Used for ef migration command.
    /// </summary>
    public class HMTDataDbContextFactory : IDesignTimeDbContextFactory<HMTDataDbContext>
    {
        public HMTDataDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<HMTDataDbContext>();
            optionsBuilder.UseSqlite(DbConnectionString.HMTData);

            return new HMTDataDbContext(optionsBuilder.Options);
        }
    }
}
