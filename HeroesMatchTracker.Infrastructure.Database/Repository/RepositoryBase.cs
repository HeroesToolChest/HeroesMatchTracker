using Microsoft.EntityFrameworkCore;

namespace HeroesMatchTracker.Infrastructure.Database.Repository
{
    public abstract class RepositoryBase<TContext>
        where TContext : DbContext
    {
    }
}
