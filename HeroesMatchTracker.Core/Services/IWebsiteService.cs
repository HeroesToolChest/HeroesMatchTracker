using HeroesMatchTracker.Core.HotsLogs;

namespace HeroesMatchTracker.Core.Services
{
    public interface IWebsiteService
    {
        IHotsLogsService HotsLogs { get; }
    }
}
