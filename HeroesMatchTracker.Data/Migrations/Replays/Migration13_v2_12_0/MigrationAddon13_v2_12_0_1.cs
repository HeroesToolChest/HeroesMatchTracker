using HeroesMatchTracker.Data.Databases;

namespace HeroesMatchTracker.Data.Migrations.Replays
{
    internal class MigrationAddon13_v2_12_0_1 : MigrationMethods<ReplaysContext>, IMigrationAddon
    {
        public void Execute()
        {
            AddColumnToTable("ReplayMatchPlayers", "HasActiveBoost", "INTEGER DEFAULT 0");
        }
    }
}
