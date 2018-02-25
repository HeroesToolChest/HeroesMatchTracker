using HeroesMatchTracker.Data.Databases;

namespace HeroesMatchTracker.Data.Migrations.Replays
{
    internal class MigrationAddon8_v2_6_7_1 : MigrationMethods<ReplaysContext>, IMigrationAddon
    {
        public void Execute()
        {
            AddColumnToTable("ReplayMatchPlayers", "IsVoiceSilenced", "INTEGER NOT NULL DEFAULT 0");
        }
    }
}
