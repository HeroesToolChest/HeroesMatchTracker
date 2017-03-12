using HeroesMatchData.Core.HotsLogs;

namespace HeroesMatchData.Core.Services
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
