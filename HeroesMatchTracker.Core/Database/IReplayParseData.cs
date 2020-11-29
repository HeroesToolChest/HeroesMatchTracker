using Heroes.StormReplayParser;
using HeroesMatchTracker.Infrastructure.Database.Contexts;

namespace HeroesMatchTracker.Core.Services
{
    public interface IReplayParseData
    {
        string GetReplayHash(StormReplay replay);

        bool IsReplayExists(HeroesReplaysDbContext context, string hash);

        void AddReplay(HeroesReplaysDbContext context, string fileName, string hash, StormReplay replay);
    }
}
