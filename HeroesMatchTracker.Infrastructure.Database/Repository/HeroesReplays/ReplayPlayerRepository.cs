using HeroesMatchTracker.Core.Repositories;
using HeroesMatchTracker.Core.Repositories.HeroesReplays;
using HeroesMatchTracker.Infrastructure.Database.Contexts;
using HeroesMatchTracker.Core.Entities;
using System;
using System.Linq;

namespace HeroesMatchTracker.Infrastructure.Database.Repository.HeroesReplays
{
    public class ReplayPlayerRepository : RepositoryBase<HeroesReplaysDbContext>, IReplayPlayerRepository
    {
        public ReplayPlayer? GetPlayer(IUnitOfWork unitOfWork, long playerId)
        {
            var context = UnitOfWorkHeroesReplaysDbContext(unitOfWork);

            return context.ReplayPlayers.Find(playerId);
        }

        public bool IsExists(IUnitOfWork unitOfWork, ReplayPlayer replayPlayer)
        {
            if (replayPlayer is null)
                throw new ArgumentNullException(nameof(replayPlayer));

            var context = UnitOfWorkHeroesReplaysDbContext(unitOfWork);

            if (!string.IsNullOrWhiteSpace(replayPlayer.BattleTagName))
                return context.ReplayPlayers.Any(x => x.BattleTagName == replayPlayer.BattleTagName);
            else
                return context.ReplayPlayers.Any(x => x.ShortcutId == replayPlayer.ShortcutId);
        }
    }
}
