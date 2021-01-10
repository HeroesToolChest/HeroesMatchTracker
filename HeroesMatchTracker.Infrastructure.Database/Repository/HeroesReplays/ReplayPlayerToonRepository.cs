using HeroesMatchTracker.Core.Entities;
using HeroesMatchTracker.Core.Repositories;
using HeroesMatchTracker.Core.Repositories.HeroesReplays;
using HeroesMatchTracker.Infrastructure.Database.Contexts;
using System;
using System.Linq;

namespace HeroesMatchTracker.Infrastructure.Database.Repository.HeroesReplays
{
    public class ReplayPlayerToonRepository : RepositoryBase<HeroesReplaysDbContext>, IReplayPlayerToonRepository
    {
        public bool IsExists(IUnitOfWork unitOfWork, ReplayPlayerToon replayPlayerToon)
        {
            if (unitOfWork is null)
                throw new ArgumentNullException(nameof(unitOfWork));

            if (replayPlayerToon is null)
                throw new ArgumentNullException(nameof(replayPlayerToon));

            var context = UnitOfWorkHeroesReplaysDbContext(unitOfWork);

            return context.ReplayPlayerToons.Any(x => x.Region == replayPlayerToon.Region &&
                x.ProgramId == replayPlayerToon.ProgramId &&
                x.Realm == replayPlayerToon.Realm &&
                x.Id == replayPlayerToon.Id);
        }

        public long? GetPlayerId(IUnitOfWork unitOfWork, ReplayPlayerToon replayPlayerToon)
        {
            if (unitOfWork is null)
                throw new ArgumentNullException(nameof(unitOfWork));

            var context = UnitOfWorkHeroesReplaysDbContext(unitOfWork);

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
