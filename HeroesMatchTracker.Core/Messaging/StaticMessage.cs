namespace HeroesMatchTracker.Core.Messaging
{
    public static class StaticMessage
    {
        public static string UpdateUserBattleTag => "UpdateUserBattleTag";
        public static string MatchSummaryClosed => "MatchSummaryClosed";
        public static string ReEnableMatchSummaryButton => "ReEnableMatchSummaryButton";
        public static string ChangeCurrentSelectedReplayMatchLeft => "ChangeCurrentSelectedReplayMatchLeft";
        public static string ChangeCurrentSelectedReplayMatchRight => "ChangeCurrentSelectedReplayMatchRight";
        public static string HotsLogsUploaderDisabled => "HotsLogsUploaderDisabled";
        public static string HotsApiUploaderDisabled => "HotsApiUploaderDisabled";
    }
}
