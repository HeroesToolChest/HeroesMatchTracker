namespace HeroesMatchData.Data.Databases
{
    using HeroesMatchData.Data.Models.ReleaseNotes;
    using SQLite.CodeFirst;
    using System.Data.Entity;

    internal class ReleaseNotesContext : MatchDataDbContext
    {
        public ReleaseNotesContext()
            : base($"name={Properties.Settings.Default.ReleaseNotesConnNameDb}") { }

        public virtual DbSet<ReleaseNote> ReleaseNotes { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            Database.SetInitializer(new SqliteCreateDatabaseIfNotExists<ReleaseNotesContext>(modelBuilder));
        }
    }
}
