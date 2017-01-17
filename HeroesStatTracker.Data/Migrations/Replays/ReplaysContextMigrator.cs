namespace HeroesStatTracker.Data.Migrations.Replays
{
    internal class ReplaysContextMigrator : ContextMigrator
    {
        public ReplaysContextMigrator()
        {
            /* add new migration commands here
             IMigrationList.Add(new Migration1_v2_0_0()); */

            ExecuteMigrationCommands();
        }
    }
}
