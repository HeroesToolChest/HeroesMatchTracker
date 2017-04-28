using HeroesMatchTracker.Core.HotsLogs;

namespace HeroesMatchTracker.Core.Services
{
    public class Website : IWebsiteService
    {
        public Website(IHotsLogsService hotsLogs)
        {
            HotsLogs = hotsLogs;
        }

        public IHotsLogsService HotsLogs { get; }
    }
}
