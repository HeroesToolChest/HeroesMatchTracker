using HeroesMatchData.Data.Databases;

namespace HeroesMatchData.Data.Migrations.Replays
{
    internal class MigrationAddon2_v1_3_0_2 : MigrationMethods<ReplaysContext>, IMigrationAddon
    {
        public void Execute()
        {
            AddColumnToTable("ReplayRenamedPlayers", "BattleNetTId", "NVARCHAR");
        }
    }
}
