using Microsoft.EntityFrameworkCore;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace HeroesMatchTracker.Infrastructure.Database.Repository
{
    public abstract class RepositoryBase<TContext>
        where TContext : DbContext
    {
        public int SaveChanges(TContext context)
        {
            if (context is null)
                throw new ArgumentNullException(nameof(context));

            return context.SaveChanges();
        }

        public int SaveChanges(TContext context, bool acceptAllChangesOnSuccess)
        {
            if (context is null)
                throw new ArgumentNullException(nameof(context));

            return context.SaveChanges(acceptAllChangesOnSuccess);
        }

        public Task<int> SaveChangesAsync(TContext context, bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default)
        {
            if (context is null)
                throw new ArgumentNullException(nameof(context));

            return context.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
        }

        public Task<int> SaveChangesAsync(TContext context, CancellationToken cancellationToken = default)
        {
            if (context is null)
                throw new ArgumentNullException(nameof(context));

            return context.SaveChangesAsync(cancellationToken);
        }
    }
}
