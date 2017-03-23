namespace HeroesMatchData.Data.Migrations.Settings
{
    internal class SettingsContextMigrator : ContextMigrator
    {
        public SettingsContextMigrator()
        {
            // add new migration commands here
            IMigrationList.Add(new Migration1_v1_3_0());
            IMigrationList.Add(new Migration2_v2_0_0());

            ExecuteMigrationCommands();
        }
    }
}
