using HeroesStatTracker.Data.Queries.Replays;
using HeroesStatTracker.Data.Queries.Settings;

namespace HeroesStatTracker.Data
{
    public class DatabaseService : IDatabaseService
    {
        public ReplaysDb ReplaysDb() => new ReplaysDb();
        public SettingsDb SettingsDb() => new SettingsDb();
    }
}
