using Microsoft.EntityFrameworkCore;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace HeroesMatchTracker.Infrastructure.Database.Repository
{
    public abstract class RepositoryBase<TContext> : IDisposable
        where TContext : DbContext
    {
        private readonly IDbContextFactory<TContext> _dbContextFactory;

        private bool _disposedValue;

        public RepositoryBase(IDbContextFactory<TContext> dbContextFactory)
        {
            _dbContextFactory = dbContextFactory;
            Context = _dbContextFactory.CreateDbContext();
        }

        protected TContext? Context { get; private set; }

        public int SaveChanges()
        {
            throw new System.NotImplementedException();
        }

        public int SaveChanges(bool acceptAllChangesOnSuccess)
        {
            throw new System.NotImplementedException();
        }

        public Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default)
        {
            throw new System.NotImplementedException();
        }

        public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            throw new System.NotImplementedException();
        }

        public void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposedValue)
            {
                if (disposing)
                {
                    Context?.Dispose();
                }

                _disposedValue = true;
            }
        }
    }
}
