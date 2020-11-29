using Microsoft.EntityFrameworkCore;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace HeroesMatchTracker.Core.Database
{
    public interface IUnitOfWork<TContext>
        where TContext : DbContext
    {
        int SaveChanges(TContext context);

        int SaveChanges(TContext context, bool acceptAllChangesOnSuccess);

        Task<int> SaveChangesAsync(TContext context, bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default);

        Task<int> SaveChangesAsync(TContext context, CancellationToken cancellationToken = default);
    }
}
