using System;

namespace HeroesMatchData.Core.HotsLogs
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
