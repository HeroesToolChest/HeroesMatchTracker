namespace HeroesMatchData.Data.Migrations.Settings
{
    internal class SettingsContextMigrator : ContextMigrator
    {
        public SettingsContextMigrator()
        {
            // add new migration commands here
            IMigrationList.Add(new Migration1_v1_3_0());

            ExecuteMigrationCommands();
        }
    }
}
