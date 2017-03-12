namespace HeroesMatchData.Data.Migrations.ReleaseNotes
{
    internal class ReleaseNotesContextMigrator : ContextMigrator
    {
        public ReleaseNotesContextMigrator()
        {
            // add new migration commands here
            IMigrationList.Add(new Migration1_v1_4_0());

            ExecuteMigrationCommands();
        }
    }
}
