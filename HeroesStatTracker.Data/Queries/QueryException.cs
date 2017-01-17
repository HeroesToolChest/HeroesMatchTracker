using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HeroesStatTracker.Data.Queries
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
