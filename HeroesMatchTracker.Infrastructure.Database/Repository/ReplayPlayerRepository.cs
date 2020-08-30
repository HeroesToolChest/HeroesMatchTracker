using HeroesMatchTracker.Core.Database;
using HeroesMatchTracker.Core.Entities;
using HeroesMatchTracker.Infrastructure.Database.Contexts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace HeroesMatchTracker.Infrastructure.Database.Repository
{
    public class ReplayPlayerRepository : RepositoryBase<HeroesReplaysDbContext>, IReplayPlayerRepository
    {
        public ReplayPlayerRepository(IDbContextFactory<HeroesReplaysDbContext> dbContextFactory)
            : base(dbContextFactory)
        {
        }


    }
}
