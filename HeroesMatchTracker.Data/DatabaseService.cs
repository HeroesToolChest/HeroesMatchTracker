using HeroesMatchTracker.Data.Queries.ReleaseNotes;
using HeroesMatchTracker.Data.Queries.Replays;
using HeroesMatchTracker.Data.Queries.Settings;

namespace HeroesMatchTracker.Data
{
    public class DatabaseService : IDatabaseService
    {
        public ReplaysDb ReplaysDb() => new ReplaysDb();
        public SettingsDb SettingsDb() => new SettingsDb();
        public ReleaseNotesDB ReleaseNotesDb() => new ReleaseNotesDB();
    }
}
