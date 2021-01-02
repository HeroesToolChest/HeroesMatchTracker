using Microsoft.EntityFrameworkCore;

namespace HeroesMatchTracker.Infrastructure.Database.Contexts.HMT2Contexts
{
    public class HMT2ReplaysDbContext : DbContext
    {
        public HMT2ReplaysDbContext(DbContextOptions<HMT2ReplaysDbContext> options)
            : base(options)
        {
        }
    }
}
