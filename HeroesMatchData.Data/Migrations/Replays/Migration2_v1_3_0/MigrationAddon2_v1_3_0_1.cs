using HeroesMatchData.Data.Databases;

namespace HeroesMatchData.Data.Migrations.Replays
{
    internal class MigrationAddon2_v1_3_0_1 : MigrationMethods<ReplaysContext>, IMigrationAddon
    {
        public void Execute()
        {
            AddColumnToTable("ReplayAllHotsPlayers", "BattleNetTId", "NVARCHAR");
        }
    }
}
