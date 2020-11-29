using HeroesMatchTracker.Core.Database.HeroesReplays;
using HeroesMatchTracker.Infrastructure.Database.Contexts;
using HeroesMatchTracker.Shared.Entities;
using System;
using System.Linq;

namespace HeroesMatchTracker.Infrastructure.Database.Repository.HeroesReplays
{
    public class ReplayPlayerToonRepository : RepositoryBase<HeroesReplaysDbContext>, IReplayPlayerToonRepository
    {
        public bool IsExists(HeroesReplaysDbContext context, ReplayPlayerToon replayPlayerToon)
        {
            if (context is null)
                throw new ArgumentNullException(nameof(context));

            if (replayPlayerToon is null)
                throw new ArgumentNullException(nameof(replayPlayerToon));

            return context.ReplayPlayerToons.Any(x => x.Region == replayPlayerToon.Region &&
                x.ProgramId == replayPlayerToon.ProgramId &&
                x.Realm == replayPlayerToon.Realm &&
                x.Id == replayPlayerToon.Id);
        }

        public long? GetPlayerId(HeroesReplaysDbContext context, ReplayPlayerToon replayPlayerToon)
        {
            if (context is null)
                throw new ArgumentNullException(nameof(context));

            ReplayPlayerToon? result = context.ReplayPlayerToons.FirstOrDefault(x => x.Region == replayPlayerToon.Region &&
                x.ProgramId == replayPlayerToon.ProgramId &&
                x.Realm == replayPlayerToon.Realm &&
                x.Id == replayPlayerToon.Id);

            if (result is null)
                return null;
            else
                return result.PlayerId;
        }
    }
}
