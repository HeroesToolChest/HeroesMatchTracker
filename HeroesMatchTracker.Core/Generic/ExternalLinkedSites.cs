using NLog;
using System;

namespace HeroesMatchTracker.Core
{
    public static class ExternalLinkedSites
    {
        private static Logger WarningLog = LogManager.GetLogger(LogFileNames.WarningLogFileName);

        public static bool IsApprovedSite(Uri uri)
        {
            switch (uri.Host)
            {
                case "github.com":
                    return true;
                default:
                    WarningLog.Log(LogLevel.Warn, $"Attempted to access external link: {uri.AbsolutePath}");
                    return false;
            }
        }
    }
}
