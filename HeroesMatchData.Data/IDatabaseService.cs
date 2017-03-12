using HeroesMatchData.Data.Queries.ReleaseNotes;
using HeroesMatchData.Data.Queries.Replays;
using HeroesMatchData.Data.Queries.Settings;

namespace HeroesMatchData.Data
{
    public interface IDatabaseService
    {
        ReplaysDb ReplaysDb();
        SettingsDb SettingsDb();
        ReleaseNotesDB ReleaseNotesDb();
    }
}
