using HeroesMatchTracker.Data.Databases;

namespace HeroesMatchTracker.Data.Migrations.Replays
{
    internal class MigrationAddon7_v2_2_0_1 : MigrationMethods<ReplaysContext>, IMigrationAddon
    {
        public void Execute()
        {
            AddColumnToTable("ReplayMatchPlayers", "AccountLevel", "INTEGER NOT NULL DEFAULT 0");
            AddColumnToTable("ReplayAllHotsPlayers", "AccountLevel", "INTEGER NOT NULL DEFAULT 0");
        }
    }
}
