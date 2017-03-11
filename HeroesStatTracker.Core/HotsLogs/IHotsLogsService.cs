using System;
using System.Threading.Tasks;
using static Heroes.Helpers.HeroesHelpers.Regions;

namespace HeroesStatTracker.Core.HotsLogs
{
    public interface IHotsLogsService
    {
        Task<Uri> PlayerProfileAsync(Region region, string battleTag);
    }
}
