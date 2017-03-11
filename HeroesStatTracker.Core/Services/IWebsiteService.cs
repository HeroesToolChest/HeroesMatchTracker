using HeroesStatTracker.Core.HotsLogs;

namespace HeroesStatTracker.Core.Services
{
    public interface IWebsiteService
    {
        IHotsLogsService HotsLogs { get; }
    }
}
