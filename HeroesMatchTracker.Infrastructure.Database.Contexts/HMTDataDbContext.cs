using Microsoft.EntityFrameworkCore;

namespace HeroesMatchTracker.Infrastructure.Database.Contexts
{
    public class HMTDataDbContext : DbContext
    {
        public HMTDataDbContext(DbContextOptions<HMTDataDbContext> options)
            : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
            => optionsBuilder
                .UseLazyLoadingProxies()
                .UseSqlite(DbConnectionString.HMTData);

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
        }
    }
}
