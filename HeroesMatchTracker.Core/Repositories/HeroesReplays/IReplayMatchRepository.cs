using HeroesMatchTracker.Shared.Entities;
using System;

namespace HeroesMatchTracker.Core.Repositories.HeroesReplays
{
    public interface IReplayMatchRepository
    {
        void Add(IUnitOfWork unitOfWork, ReplayMatch replayMatch);

        bool IsExists(IUnitOfWork unitOfWork, string hash);

        bool IsExists(IUnitOfWork unitOfWork, long replayId);

        DateTime? GetLastestReplayTimeStamp(IUnitOfWork unitOfWork);
    }
}
