using HeroesMatchTracker.Core.Database;
using HeroesMatchTracker.Core.Entities;
using HeroesMatchTracker.Infrastructure.Database.Contexts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace HeroesMatchTracker.Infrastructure.Database.Repository
{
    public class ReplayMatchRepository : RepositoryBase<HeroesReplaysDbContext>, IReplayMatchRepository
    {
        public ReplayMatchRepository(IDbContextFactory<HeroesReplaysDbContext> dbContextFactory)
            : base(dbContextFactory)
        {
        }

        public void Add(ReplayMatch replayMatch)
        {
           // _context.Replays.Add(replayMatch).
        }
    }
}
