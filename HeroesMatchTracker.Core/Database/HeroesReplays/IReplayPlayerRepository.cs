using HeroesMatchTracker.Infrastructure.Database.Contexts;
using HeroesMatchTracker.Shared.Entities;

namespace HeroesMatchTracker.Core.Database.HeroesReplays
{
    public interface IReplayPlayerRepository
    {
        ReplayPlayer? GetPlayer(HeroesReplaysDbContext context, long playerId);

        bool IsExists(HeroesReplaysDbContext context, ReplayPlayer replayPlayer);
    }
}
