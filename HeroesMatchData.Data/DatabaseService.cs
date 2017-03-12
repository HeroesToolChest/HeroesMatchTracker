using HeroesMatchData.Data.Queries.ReleaseNotes;
using HeroesMatchData.Data.Queries.Replays;
using HeroesMatchData.Data.Queries.Settings;

namespace HeroesMatchData.Data
{
    public class DatabaseService : IDatabaseService
    {
        public ReplaysDb ReplaysDb() => new ReplaysDb();
        public SettingsDb SettingsDb() => new SettingsDb();
        public ReleaseNotesDB ReleaseNotesDb() => new ReleaseNotesDB();
    }
}
