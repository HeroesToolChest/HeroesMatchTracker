using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HeroesStatTracker.Core.HotsLogs
{
    [Serializable]
    public class MaintenanceException : Exception
    {
        public MaintenanceException(string message, Exception ex)
            : base(message, ex)
        { }

        public MaintenanceException(string message)
            : base(message)
        { }
    }
}
