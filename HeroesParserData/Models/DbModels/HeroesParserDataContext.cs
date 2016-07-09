namespace HeroesParserData.Models.DbModels
{
    using System.Data.Entity;
    using System.Data.Entity.Validation;
    using System.Linq;

    public partial class HeroesParserDataContext : DbContext
    {
        public HeroesParserDataContext()
            : base("name=HeroesParserData")
        {
        }

        public override int SaveChanges()
        {
            try
            {
                return base.SaveChanges();
            }
            catch (DbEntityValidationException ex)
            {
                // Retrieve the error messages as a list of strings.
                var errorMessages = ex.EntityValidationErrors
                        .SelectMany(x => x.ValidationErrors)
                        .Select(x => x.ErrorMessage);

                // Join the list to a single string.
                var fullErrorMessage = string.Join("; ", errorMessages);

                // Combine the original exception message with the new one.
                var exceptionMessage = string.Concat(ex.Message, " The validation errors are: ", fullErrorMessage);

                // Throw a new DbEntityValidationException with the improved exception message.
                throw new DbEntityValidationException(exceptionMessage, ex.EntityValidationErrors);
            }
        }
        public virtual DbSet<Replay> Replays { get; set; }
        public virtual DbSet<ReplayAllHotsPlayer> ReplayAllHotsPlayers { get; set; }
        public virtual DbSet<ReplayMatchChat> ReplayMatchChats { get; set; }
        public virtual DbSet<ReplayMatchTeamExperience> ReplayMatchTeamExperiences { get; set; }
        public virtual DbSet<ReplayMatchPlayer> ReplayMatchPlayers { get; set; }
        public virtual DbSet<ReplayMatchPlayerScoreResult> ReplayMatchPlayerScoreResults { get; set; }
        public virtual DbSet<ReplayMatchPlayerTalent> ReplayMatchPlayerTalents { get; set; }
        public virtual DbSet<ReplayMatchTeamBan> ReplayMatchTeamBans { get; set; }
        public virtual DbSet<ReplayMatchTeamLevel> ReplayMatchTeamLevels { get; set; }
        public virtual DbSet<ReplayMatchTeamObjective> ReplayMatchTeamObjectives { get; set; }

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
                .HasMany(e => e.ReplayChats)
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

            modelBuilder.Entity<ReplayMatchChat>()
                .Property(e => e.Message)
                .IsUnicode(false);
        }
    }
}
