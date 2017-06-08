namespace HeroesMatchTracker.Data.Migrations.Replays
{
    internal class ReplaysContextMigrator : ContextMigrator
    {
        public ReplaysContextMigrator()
        {
            // add new migration commands here
            IMigrationList.Add(new Migration1_v1_2_0());
            IMigrationList.Add(new Migration2_v1_3_0());
            IMigrationList.Add(new Migration3_v1_4_0());
            IMigrationList.Add(new Migration4_v2_0_0());
            IMigrationList.Add(new Migration5_v2_0_0());
            IMigrationList.Add(new Migration6_v2_1_0());
            IMigrationList.Add(new Migration7_v2_2_0());
            ExecuteMigrationCommands();
        }
    }
}
