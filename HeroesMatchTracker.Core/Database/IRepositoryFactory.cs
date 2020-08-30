namespace HeroesMatchTracker.Core.Database
{
    public interface IRepositoryFactory
    {
        /// <summary>
        /// Creates an instance of the repository. Must be disposed by the caller.
        /// </summary>
        /// <typeparam name="TRepository">Repository of type <see cref="IUnitOfWork"/>.</typeparam>
        /// <returns>An instance of the <see cref="IUnitOfWork"/>.</returns>
        TRepository CreateRepository<TRepository>()
            where TRepository : IUnitOfWork;
    }
}
