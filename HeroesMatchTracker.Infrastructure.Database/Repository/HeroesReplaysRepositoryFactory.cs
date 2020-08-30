using HeroesMatchTracker.Core.Database;
using Splat;

namespace HeroesMatchTracker.Infrastructure.Database.Repository
{
    public class HeroesReplaysRepositoryFactory : IRepositoryFactory
    {
        public HeroesReplaysRepositoryFactory()
        {
        }

        public TRepository CreateRepository<TRepository>()
            where TRepository : IUnitOfWork
        {
            return Locator.Current.GetService<TRepository>();
        }
    }
}
