namespace HeroesMatchTracker.Data.Databases
{
    using Models.Settings;
    using SQLite.CodeFirst;
    using System.Data.Entity;

    internal class SettingsContext : MatchDataDbContext
    {
        public SettingsContext()
            : base($"name={Properties.Settings.Default.SettingsConnNameDb}") { }

        public virtual DbSet<UserSetting> UserSettings { get; set; }
        public virtual DbSet<FailedReplay> FailedReplays { get; set; }
        public virtual DbSet<UserProfile> UserProfiles { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            Database.SetInitializer(new SqliteCreateDatabaseIfNotExists<SettingsContext>(modelBuilder));
        }
    }
}
