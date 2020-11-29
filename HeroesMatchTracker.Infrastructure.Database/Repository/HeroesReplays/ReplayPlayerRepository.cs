using HeroesMatchTracker.Core.Database.HeroesReplays;
using HeroesMatchTracker.Infrastructure.Database.Contexts;
using HeroesMatchTracker.Shared.Entities;
using System;
using System.Linq;

namespace HeroesMatchTracker.Infrastructure.Database.Repository.HeroesReplays
{
    public class ReplayPlayerRepository : RepositoryBase<HeroesReplaysDbContext>, IReplayPlayerRepository
    {
        public ReplayPlayer? GetPlayer(HeroesReplaysDbContext context, long playerId)
        {
            if (context is null)
                throw new ArgumentNullException(nameof(context));

            return context.ReplayPlayers.Find(playerId);
        }

        public bool IsExists(HeroesReplaysDbContext context, ReplayPlayer replayPlayer)
        {
            if (context is null)
                throw new ArgumentNullException(nameof(context));

            if (replayPlayer is null)
                throw new ArgumentNullException(nameof(replayPlayer));

            if (!string.IsNullOrWhiteSpace(replayPlayer.BattleTagName))
                return context.ReplayPlayers.Any(x => x.BattleTagName == replayPlayer.BattleTagName);
            else
                return context.ReplayPlayers.Any(x => x.ShortcutId == replayPlayer.ShortcutId);
        }
    }
}
