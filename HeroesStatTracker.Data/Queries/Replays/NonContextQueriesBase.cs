using HeroesStatTracker.Data.Databases;
using HeroesStatTracker.Data.Models;

namespace HeroesStatTracker.Data.Queries.Replays
{
    public abstract class NonContextQueriesBase<T> : QueriesBase where T : INonContextModels
    {
        internal NonContextQueriesBase() { }

        internal abstract long CreateRecord(ReplaysContext db, T model);
        internal abstract long UpdateRecord(ReplaysContext db, T model);
        internal abstract bool IsExistingRecord(ReplaysContext db, T model);
    }
}
