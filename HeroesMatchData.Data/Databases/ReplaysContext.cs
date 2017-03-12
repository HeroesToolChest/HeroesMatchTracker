namespace HeroesMatchData.Data.Databases
{
    using Models.Replays;
    using SQLite.CodeFirst;
    using System.Data.Entity;

    internal class ReplaysContext : StatTrackerDbContext
    {
        public ReplaysContext()
            : base($"name={Properties.Settings.Default.ReplaysConnNameDb}") { }

        public virtual DbSet<ReplayMatch> Replays { get; set; }
        public virtual DbSet<ReplayAllHotsPlayer> ReplayAllHotsPlayers { get; set; }
        public virtual DbSet<ReplayMatchMessage> ReplayMatchMessages { get; set; }
        public virtual DbSet<ReplayMatchTeamExperience> ReplayMatchTeamExperiences { get; set; }
        public virtual DbSet<ReplayMatchPlayer> ReplayMatchPlayers { get; set; }
        public virtual DbSet<ReplayMatchPlayerScoreResult> ReplayMatchPlayerScoreResults { get; set; }
        public virtual DbSet<ReplayMatchPlayerTalent> ReplayMatchPlayerTalents { get; set; }
        public virtual DbSet<ReplayMatchTeamBan> ReplayMatchTeamBans { get; set; }
        public virtual DbSet<ReplayMatchTeamLevel> ReplayMatchTeamLevels { get; set; }
        public virtual DbSet<ReplayMatchTeamObjective> ReplayMatchTeamObjectives { get; set; }
        public virtual DbSet<ReplayAllHotsPlayerHero> ReplayAllHotsPlayerHeroes { get; set; }
        public virtual DbSet<ReplayMatchAward> ReplayMatchAwards { get; set; }
        public virtual DbSet<ReplayRenamedPlayer> ReplayRenamedPlayers { get; set; }
        public virtual DbSet<ReplayHotsLogsUpload> ReplayHotsLogsUploads { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ReplayMatch>()
                .HasMany(e => e.ReplayMatchTeamExperiences)
                .WithRequired(e => e.Replay)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<ReplayMatch>()
                .HasMany(e => e.ReplayMatchPlayers)
                .WithRequired(e => e.Replay)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<ReplayMatch>()
                .HasMany(e => e.ReplayMatchPlayerScoreResults)
                .WithRequired(e => e.Replay)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<ReplayMatch>()
                .HasMany(e => e.ReplayMatchPlayerTalents)
                .WithRequired(e => e.Replay)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<ReplayMatch>()
                .HasOptional(e => e.ReplayMatchTeamBan)
                .WithRequired(e => e.Replay);

            modelBuilder.Entity<ReplayMatch>()
                .HasMany(e => e.ReplayMatchTeamLevels)
                .WithRequired(e => e.Replay)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<ReplayMatch>()
                .HasMany(e => e.ReplayMatchTeamObjectives)
                .WithRequired(e => e.Replay)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<ReplayMatch>()
                .HasMany(e => e.ReplayMatchMessage)
                .WithRequired(e => e.Replay)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<ReplayMatch>()
                .HasMany(e => e.ReplayMatchAward)
                .WithRequired(e => e.Replay)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<ReplayMatch>()
                .HasMany(e => e.ReplayHotsLogsUpload)
                .WithRequired(e => e.Replay)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<ReplayAllHotsPlayer>()
                .HasMany(e => e.ReplayMatchPlayers)
                .WithRequired(e => e.ReplayAllHotsPlayer)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<ReplayAllHotsPlayer>()
                .HasMany(e => e.ReplayMatchPlayerScoreResults)
                .WithRequired(e => e.ReplayAllHotsPlayer)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<ReplayAllHotsPlayer>()
                .HasMany(e => e.ReplayMatchPlayerTalents)
                .WithRequired(e => e.ReplayAllHotsPlayer)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<ReplayAllHotsPlayer>()
                .HasMany(e => e.ReplayAllHotsPlayerHeroes)
                .WithRequired(e => e.ReplayAllHotsPlayer)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<ReplayAllHotsPlayer>()
                .HasMany(e => e.ReplayMatchAwards)
                .WithRequired(e => e.ReplayAllHotsPlayer)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<ReplayMatchMessage>()
                .Property(e => e.Message)
                .IsUnicode(false);

            Database.SetInitializer(new SqliteCreateDatabaseIfNotExists<ReplaysContext>(modelBuilder));
        }
    }
}
