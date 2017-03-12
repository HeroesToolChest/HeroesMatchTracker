using HeroesMatchData.Data.Databases;

namespace HeroesMatchData.Data.Migrations.Replays
{
    internal class MigrationAddon1_v1_2_0_1 : MigrationMethods<ReplaysContext>, IMigrationAddon
    {
        public MigrationAddon1_v1_2_0_1()
            : base(Properties.Settings.Default.ReplaysConnNameDb)
        { }

        public void Execute()
        {
            AddColumnToTable("ReplayMatchPlayers", "PartyValue", "INTEGER DEFAULT 0");
        }
    }
}
