using Microsoft.EntityFrameworkCore;

namespace HeroesMatchTracker.Infrastructure.Database.HMT2Contexts
{
    public class HMT2ReplaysDbContext : DbContext
    {
        public HMT2ReplaysDbContext(DbContextOptions<HMT2ReplaysDbContext> options)
            : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
            => optionsBuilder.UseSqlite(HMT2ConnectionString.HMT2Replays);
    }
}
