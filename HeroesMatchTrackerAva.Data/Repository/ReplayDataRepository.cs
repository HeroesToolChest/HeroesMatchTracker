using HeroesMatchTracker.Core.Entities;
using HeroesMatchTracker.Infrastructure.Database.Contexts;
using System;

namespace HeroesMatchTracker.Infrastructure.Database.Repository
{
    public class ReplayDataRepository
    {
        private readonly HeroesReplaysDbContext _context;

        public ReplayDataRepository(HeroesReplaysDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public void Add(ReplayMatch replayMatch)
        {
           // _context.Replays.Add(replayMatch).
        }
    }
}
