using HeroesStatTracker.Data.Queries.Replays;
using HeroesStatTracker.Data.Queries.Settings;

namespace HeroesStatTracker.Data
{
    public interface IDatabaseService
    {
        ReplaysDb ReplaysDb();
        SettingsDb SettingsDb();
    }
}
