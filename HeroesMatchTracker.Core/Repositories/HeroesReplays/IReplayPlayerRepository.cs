using HeroesMatchTracker.Shared.Entities;

namespace HeroesMatchTracker.Core.Repositories.HeroesReplays
{
    public interface IReplayPlayerRepository
    {
        ReplayPlayer? GetPlayer(IUnitOfWork unitOfWork, long playerId);

        bool IsExists(IUnitOfWork unitOfWork, ReplayPlayer replayPlayer);
    }
}
