using System;

namespace HeroesMatchTracker.Data.Queries
{
    [Serializable]
    public class QueryException : Exception
    {
        public QueryException(string message, Exception ex)
            : base(message, ex)
        { }

        public QueryException(string message)
            : base(message)
        { }
    }
}
