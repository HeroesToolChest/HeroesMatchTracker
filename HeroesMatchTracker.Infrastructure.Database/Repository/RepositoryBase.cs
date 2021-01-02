using HeroesMatchTracker.Core.Repositories;
using HeroesMatchTracker.Infrastructure.Database.Contexts;
using Microsoft.EntityFrameworkCore;
using System;

namespace HeroesMatchTracker.Infrastructure.Database.Repository
{
    public abstract class RepositoryBase<TContext>
        where TContext : DbContext
    {
        protected HeroesReplaysDbContext UnitOfWorkHeroesReplaysDbContext(IUnitOfWork unitOfWork)
        {
            if (unitOfWork is not HeroesReplaysDbContext context)
                throw new ArgumentException($"{nameof(unitOfWork)} is not of type {nameof(HeroesReplaysDbContext)}");

            return context;
        }
    }
}
