using HeroesMatchData.Data.Databases;

namespace HeroesMatchData.Data.Migrations.Replays
{
    internal class MigrationAddon2_v1_3_0_2 : MigrationMethods<ReplaysContext>, IMigrationAddon
    {
        public MigrationAddon2_v1_3_0_2()
            : base(Properties.Settings.Default.ReplaysConnNameDb)
        { }

        public void Execute()
        {
            AddColumnToTable("ReplayRenamedPlayers", "BattleNetTId", "NVARCHAR");
        }
    }
}
