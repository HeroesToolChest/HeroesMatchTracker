using HeroesStatTracker.Data.Databases;
using HeroesStatTracker.Data.Replays.Models;

namespace HeroesStatTracker.Data.Replays.Queries
{
    public abstract class ReplayDataTablesBase<T> where T : IReplayModelDataTable
    {
        internal ReplayDataTablesBase() { }

        protected bool LikeOperatorInputCheck(string operand, string input)
        {
            if (operand == "LIKE" && (input.Length == 1 || (input.Length >= 2 && input[0] != '%' && input[input.Length - 1] != '%')))
                return true;
            else
                return false;
        }

        internal abstract long CreateRecord(ReplaysContext db, T model);
        internal abstract long UpdateRecord(ReplaysContext db, T model);
        internal abstract bool IsExistingRecord(ReplaysContext db, T model);
    }
}
