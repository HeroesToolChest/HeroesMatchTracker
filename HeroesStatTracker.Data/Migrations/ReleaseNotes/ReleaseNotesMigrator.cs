using HeroesStatTracker.Data.Databases;

namespace HeroesStatTracker.Data.Migrations.ReleaseNotes
{
    internal class ReleaseNotesMigrator : MigratorBase<ReleaseNotesContext, ReleaseNotesContextMigrator>, IMigrator
    {
        public ReleaseNotesMigrator(string dbName, bool databaseExists, int requiredDatabaseVersion)
            : base(dbName, databaseExists, requiredDatabaseVersion)
        {
        }
    }
}
