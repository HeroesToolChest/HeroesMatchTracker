namespace HeroesMatchTracker.Data.Queries.Replays
{
    public class ReplaysDb
    {
        public MatchReplay MatchReplay => new MatchReplay();
        public HotsPlayer HotsPlayer => new HotsPlayer();
        public MatchPlayer MatchPlayer => new MatchPlayer();
        public HotsPlayerHero HotsPlayerHero => new HotsPlayerHero();
        public MatchPlayerScoreResult MatchPlayerScoreResult => new MatchPlayerScoreResult();
        public MatchPlayerTalent MatchPlayerTalent => new MatchPlayerTalent();
        public MatchAward MatchAward => new MatchAward();
        public MatchTeamBan MatchTeamBan => new MatchTeamBan();
        public MatchTeamLevel MatchTeamLevel => new MatchTeamLevel();
        public MatchTeamExperience MatchTeamExperience => new MatchTeamExperience();
        public MatchMessage MatchMessage => new MatchMessage();
        public MatchTeamObjective MatchTeamObjective => new MatchTeamObjective();
        public RenamedPlayer RenamedPlayer => new RenamedPlayer();
        public HotsLogsUpload HotsLogsUpload => new HotsLogsUpload();
        public HotsApiUpload HotsApiUpload => new HotsApiUpload();
        public Statistics Statistics => new Statistics();
    }
}
