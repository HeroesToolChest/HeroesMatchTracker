using HeroesMatchTracker.Core.Repositories;
using HeroesMatchTracker.Core.Repositories.HeroesReplays;
using HeroesMatchTracker.Infrastructure.Database.Contexts;
using HeroesMatchTracker.Shared.Entities;
using System;
using System.Linq;

namespace HeroesMatchTracker.Infrastructure.Database.Repository.HeroesReplays
{
    public class ReplayMatchRepository : RepositoryBase<HeroesReplaysDbContext>, IReplayMatchRepository
    {
        public void Add(IUnitOfWork unitOfWork, ReplayMatch replayMatch)
        {
            var context = UnitOfWorkHeroesReplaysDbContext(unitOfWork);

            context.Replays.Add(replayMatch);
        }

        public bool IsExists(IUnitOfWork unitOfWork, string hash)
        {
            if (string.IsNullOrWhiteSpace(hash))
                throw new ArgumentException($"'{nameof(hash)}' cannot be null or whitespace", nameof(hash));

            var context = UnitOfWorkHeroesReplaysDbContext(unitOfWork);

            return context.Replays.Any(x => x.Hash == hash);
        }

        public bool IsExists(IUnitOfWork unitOfWork, long replayId)
        {
            var context = UnitOfWorkHeroesReplaysDbContext(unitOfWork);

            return context.Replays.Any(x => x.ReplayId == replayId);
        }

        public DateTime? GetLastestReplayTimeStamp(IUnitOfWork unitOfWork)
        {
            var context = UnitOfWorkHeroesReplaysDbContext(unitOfWork);

            return context.Replays.OrderByDescending(x => x.TimeStamp).FirstOrDefault()?.TimeStamp;
        }
    }
}
