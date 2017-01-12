using HeroesStatTracker.Data.Replays.Queries;
using HeroesStatTracker.Data.Settings.Queries;

namespace HeroesStatTracker.Data
{
    /// <summary>
    /// Query a database
    /// </summary>
    public static class QueryDb
    {
        public static class ReplaysDb
        {
            public static MatchReplay MatchReplay => new MatchReplay();
            public static HotsPlayer HotsPlayer => new HotsPlayer();
            public static MatchPlayer MatchPlayer => new MatchPlayer();
            public static HotsPlayerHero HotsPlayerHero => new HotsPlayerHero();
            public static MatchPlayerScoreResult MatchPlayerScoreResult => new MatchPlayerScoreResult();
            public static MatchPlayerTalent MatchPlayerTalent => new MatchPlayerTalent();
            public static MatchAward MatchAward => new MatchAward();
            public static MatchTeamBan MatchTeamBan => new MatchTeamBan();
            public static MatchTeamLevel MatchTeamLevel => new MatchTeamLevel();
            public static MatchTeamExperience MatchTeamExperience => new MatchTeamExperience();
            public static MatchMessage MatchMessage => new MatchMessage();
            public static MatchTeamObjective MatchTeamObjective => new MatchTeamObjective();
            public static RenamedPlayer RenamedPlayer => new RenamedPlayer();
        }

        public static class SettingsDb
        {
            public static UserSettings UserSettings => new UserSettings();
        }
    }
}
