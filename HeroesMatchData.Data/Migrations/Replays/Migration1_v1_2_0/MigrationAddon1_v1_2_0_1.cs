using HeroesMatchData.Data.Databases;

namespace HeroesMatchData.Data.Migrations.Replays
{
    internal class MigrationAddon1_v1_2_0_1 : MigrationMethods<ReplaysContext>, IMigrationAddon
    {
        public void Execute()
        {
            AddColumnToTable("ReplayMatchPlayers", "PartyValue", "INTEGER DEFAULT 0");
        }
    }
}
