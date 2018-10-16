using HeroesMatchTracker.Data.Databases;

namespace HeroesMatchTracker.Data.Migrations.Replays
{
    internal class MigrationAddon12_v2_11_0_1 : MigrationMethods<ReplaysContext>, IMigrationAddon
    {
        public void Execute()
        {
            AddColumnToTable("ReplayMatchPlayerScoreResults", "SpellDamage", "INTEGER DEFAULT NULL");
            AddColumnToTable("ReplayMatchPlayerScoreResults", "PhysicalDamage", "INTEGER DEFAULT NULL");
        }
    }
}
