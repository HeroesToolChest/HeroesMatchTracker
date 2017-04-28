using HeroesMatchTracker.Data.Databases;
using HeroesMatchTracker.Data.Models;

namespace HeroesMatchTracker.Data.Queries.Replays
{
    public abstract class NonContextQueriesBase<T> : QueriesBase
        where T : INonContextModels
    {
        internal NonContextQueriesBase() { }

        internal abstract long CreateRecord(ReplaysContext db, T model);
        internal abstract long UpdateRecord(ReplaysContext db, T model);
        internal abstract bool IsExistingRecord(ReplaysContext db, T model);
    }
}
