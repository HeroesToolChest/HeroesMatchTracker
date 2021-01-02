using System;

namespace HeroesMatchTracker.Core.Repositories
{
    public interface IUnitOfWork : IDisposable
    {
        int SaveChanges();

        int SaveChanges(bool acceptAllChangesOnSuccess);
    }
}
