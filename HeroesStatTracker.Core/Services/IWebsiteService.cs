using HeroesMatchData.Core.HotsLogs;

namespace HeroesMatchData.Core.Services
{
    public interface IWebsiteService
    {
        IHotsLogsService HotsLogs { get; }
    }
}
