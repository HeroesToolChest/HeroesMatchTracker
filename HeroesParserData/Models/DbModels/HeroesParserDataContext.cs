namespace HeroesParserData.Models.DbModels
{
    using SQLite.CodeFirst;
    using System.Data.Entity;
    using System.Data.Entity.Validation;
    using System.Linq;
    using System.Threading.Tasks;

    public partial class HeroesParserDataContext : DbContext
    {
        public HeroesParserDataContext()
            : base("name=HeroesParserData") { }

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
                .HasMany(e => e.ReplayAllHotsPlayerHeroes)
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
