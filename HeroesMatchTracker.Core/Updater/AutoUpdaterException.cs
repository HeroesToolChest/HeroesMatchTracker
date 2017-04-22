using System;

namespace HeroesMatchTracker.Core.Updater
{
    [Serializable]
    public class AutoUpdaterException : Exception
    {
        public AutoUpdaterException(string message, Exception ex)
            : base(message, ex)
        { }

        public AutoUpdaterException(string message)
            : base(message)
        { }
    }
}
