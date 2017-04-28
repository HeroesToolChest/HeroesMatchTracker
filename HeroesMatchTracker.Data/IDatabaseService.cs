using HeroesMatchTracker.Data.Queries.ReleaseNotes;
using HeroesMatchTracker.Data.Queries.Replays;
using HeroesMatchTracker.Data.Queries.Settings;

namespace HeroesMatchTracker.Data
{
    public interface IDatabaseService
    {
        ReplaysDb ReplaysDb();
        SettingsDb SettingsDb();
        ReleaseNotesDB ReleaseNotesDb();
    }
}
