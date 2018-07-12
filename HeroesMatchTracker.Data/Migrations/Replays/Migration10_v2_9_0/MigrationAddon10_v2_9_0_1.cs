using HeroesMatchTracker.Data.Databases;

namespace HeroesMatchTracker.Data.Migrations.Replays
{
    internal class MigrationAddon10_v2_9_0_1 : MigrationMethods<ReplaysContext>, IMigrationAddon
    {
        public void Execute()
        {
            AddColumnToTable("ReplayMatchTeamBans", "Team0Ban2", "TEXT");
            AddColumnToTable("ReplayMatchTeamBans", "Team1Ban2", "TEXT");
        }
    }
}
