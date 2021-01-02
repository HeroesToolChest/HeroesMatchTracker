using HeroesMatchTracker.Shared.Entities;

namespace HeroesMatchTracker.Core.Repositories.HeroesReplays
{
    public interface IReplayPlayerToonRepository
    {
        bool IsExists(IUnitOfWork unitOfWork, ReplayPlayerToon replayPlayerToon);

        long? GetPlayerId(IUnitOfWork unitOfWork, ReplayPlayerToon replayPlayerToon);
    }
}
