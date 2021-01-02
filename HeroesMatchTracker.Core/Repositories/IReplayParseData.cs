using Heroes.StormReplayParser;

namespace HeroesMatchTracker.Core.Repositories
{
    public interface IReplayParseData
    {
        string GetReplayHash(StormReplay replay);

        bool IsReplayExists(IUnitOfWork unitOfWork, string hash);

        void AddReplay(IUnitOfWork unitOfWork, string fileName, string hash, StormReplay replay);
    }
}
