using HeroesMatchTracker.Core.Database.HeroesReplays;
using HeroesMatchTracker.Infrastructure.Database.Contexts;
using System;
using System.Linq;

namespace HeroesMatchTracker.Infrastructure.Database.Repository.HeroesReplays
{
    public class ReplayMatchRepository : RepositoryBase<HeroesReplaysDbContext>, IReplayMatchRepository
    {
        public bool IsExists(HeroesReplaysDbContext context,  string hash)
        {
            if (context is null)
                throw new ArgumentNullException(nameof(context));

            if (string.IsNullOrWhiteSpace(hash))
                throw new ArgumentException($"'{nameof(hash)}' cannot be null or whitespace", nameof(hash));

            return context.Replays.Any(x => x.Hash == hash);
        }

        public bool IsExists(HeroesReplaysDbContext context, long replayId)
        {
            if (context is null)
                throw new ArgumentNullException(nameof(context));
            return context.Replays.Any(x => x.ReplayId == replayId);
        }

        public DateTime? GetLastestReplayTimeStamp(HeroesReplaysDbContext context)
        {
            if (context is null)
                throw new ArgumentNullException(nameof(context));

            return context.Replays.OrderByDescending(x => x.TimeStamp).FirstOrDefault()?.TimeStamp;
        }
    }
}
