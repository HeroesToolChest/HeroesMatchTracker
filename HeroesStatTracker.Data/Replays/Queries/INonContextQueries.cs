using HeroesStatTracker.Data.Databases;
using HeroesStatTracker.Data.Replays.Models;

namespace HeroesStatTracker.Data.Replays.Queries
{
    /// <summary>
    /// Methods that require ReplaysContext to be passed
    /// </summary>
    /// <typeparam name="T">IReplayModelDataTable</typeparam>
    internal interface INonContextQueries<T> where T : IReplayModelDataTable
    {
        long CreateRecord(ReplaysContext db, T model);
        long UpdateRecord(ReplaysContext db, T model);
        bool IsExistingRecord(ReplaysContext db, T model);

    }
}
