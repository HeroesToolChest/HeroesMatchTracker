using System;
using System.Threading;
using System.Threading.Tasks;

namespace HeroesMatchTracker.Core.Database
{
    public interface IUnitOfWork : IDisposable
    {
        int SaveChanges();

        int SaveChanges(bool acceptAllChangesOnSuccess);

        Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default);

        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}
