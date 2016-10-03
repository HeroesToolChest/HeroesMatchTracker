namespace HeroesParserData.Models.DbModels
{
    using Database;
    using NLog;
    using SQLite.CodeFirst;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Validation;
    using System.Linq;
    using System.Threading.Tasks;

    public partial class HeroesParserDataContext : DbContext
    {
        public static int RequiredDatabaseVersion = 1;

        public HeroesParserDataContext()
            : base("name=HeroesParserData") { }

        public async Task Initialize(Logger logger)
        {
            try
            {
                using (HeroesParserDataContext db = new HeroesParserDataContext())
                {
                    int currentVersion = 0;
                    if (db.SchemaInfo.Count() > 0)
                    {
                        currentVersion = db.SchemaInfo.Max(x => x.Version);
                        logger.Log(LogLevel.Info, $"Current Version: {currentVersion}");
                        logger.Log(LogLevel.Info, $"Required Version: {RequiredDatabaseVersion}");
                    }

                    if (currentVersion >= RequiredDatabaseVersion)
                    {
                        logger.Log(LogLevel.Info, "No migration required");
                        return;
                    }

                    HeroesParserDataContextMigrator contextMigrator = new HeroesParserDataContextMigrator();

                    while (currentVersion < RequiredDatabaseVersion)
                    {
                        currentVersion++;

                        logger.Log(LogLevel.Info, $"Migrating to version {currentVersion}");

                        foreach (string migration in contextMigrator.Migrations[currentVersion])
                        {
                            await db.Database.ExecuteSqlCommandAsync(migration);
                        }
                        foreach (IMigrationAddon migration in contextMigrator.MigrationAddons[currentVersion])
                        {
                            await migration.Execute();
                        }

                        db.SchemaInfo.Add(new SchemaInfo() { Version = currentVersion });
                        await db.SaveChangesAsync();

                        logger.Log(LogLevel.Info, $"Migration version {currentVersion} completed");
                    }

                    logger.Log(LogLevel.Info, $"Migration completed");
                }          
            }
            catch (Exception)
            {
                throw;
            }
        }

        public override int SaveChanges()
        {
            try
            {
                return base.SaveChanges();
            }
            catch (DbEntityValidationException ex)
            {
                throw new DbEntityValidationException(CustomErrorMessage(ex), ex.EntityValidationErrors);
            }
        }

        public override Task<int> SaveChangesAsync()
        {
            try
            {
                return base.SaveChangesAsync();
            }
            catch (DbEntityValidationException ex)
            {
                throw new DbEntityValidationException(CustomErrorMessage(ex), ex.EntityValidationErrors);
            }
        }

        public virtual DbSet<SchemaInfo> SchemaInfo { get; set; }
        public virtual DbSet<Replay> Replays { get; set; }
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

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Replay>()
                .HasMany(e => e.ReplayMatchTeamExperiences)
                .WithRequired(e => e.Replay)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Replay>()
                .HasMany(e => e.ReplayMatchPlayers)
                .WithRequired(e => e.Replay)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Replay>()
                .HasMany(e => e.ReplayMatchPlayerScoreResults)
                .WithRequired(e => e.Replay)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Replay>()
                .HasMany(e => e.ReplayMatchPlayerTalents)
                .WithRequired(e => e.Replay)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Replay>()
                .HasOptional(e => e.ReplayMatchTeamBan)
                .WithRequired(e => e.Replay);

            modelBuilder.Entity<Replay>()
                .HasMany(e => e.ReplayMatchTeamLevels)
                .WithRequired(e => e.Replay)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Replay>()
                .HasMany(e => e.ReplayMatchTeamObjectives)
                .WithRequired(e => e.Replay)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Replay>()
                .HasMany(e => e.ReplayMatchMessage)
                .WithRequired(e => e.Replay)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Replay>()
                .HasMany(e => e.ReplayMatchAward)
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

            Database.SetInitializer(new SqliteCreateDatabaseIfNotExists<HeroesParserDataContext>(modelBuilder));
        }

        private string CustomErrorMessage(DbEntityValidationException ex)
        {
            // Retrieve the error messages as a list of strings.
            var errorMessages = ex.EntityValidationErrors
                    .SelectMany(x => x.ValidationErrors)
                    .Select(x => x.ErrorMessage);

            // Join the list to a single string.
            var fullErrorMessage = string.Join("; ", errorMessages);

            // Combine the original exception message with the new one.
            return string.Concat(ex.Message, " The validation errors are: ", fullErrorMessage);
        }
    }
}
