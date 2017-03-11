using HeroesStatTracker.Core.HotsLogs;

namespace HeroesStatTracker.Core.Services
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
