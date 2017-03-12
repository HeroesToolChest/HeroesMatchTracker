using HeroesMatchData.Data.Databases;

namespace HeroesMatchData.Data.Migrations.Replays
{
    internal class MigrationAddon2_v1_3_0_1 : MigrationMethods<ReplaysContext>, IMigrationAddon
    {
        public MigrationAddon2_v1_3_0_1()
            : base(Properties.Settings.Default.ReplaysConnNameDb)
        { }

        public void Execute()
        {
            AddColumnToTable("ReplayAllHotsPlayers", "BattleNetTId", "NVARCHAR");
        }
    }
}
