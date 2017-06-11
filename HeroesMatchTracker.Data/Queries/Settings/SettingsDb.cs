namespace HeroesMatchTracker.Data.Queries.Settings
{
    public class SettingsDb
    {
        public UserSettings UserSettings => new UserSettings();
        public FailedReplays FailedReplays => new FailedReplays();
        public UserProfiles UserProfiles => new UserProfiles();
    }
}
