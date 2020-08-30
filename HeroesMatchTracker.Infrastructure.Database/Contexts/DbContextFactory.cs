using Microsoft.EntityFrameworkCore;

namespace HeroesMatchTracker.Infrastructure.Database.Contexts
{
    public class DbContextFactory<TContext> : IDbContextFactory<TContext>
         where TContext : DbContext, new()
    {
        public DbContextFactory()
        {
        }

        public TContext CreateDbContext()
        {
            return new TContext();
        }
    }
}
