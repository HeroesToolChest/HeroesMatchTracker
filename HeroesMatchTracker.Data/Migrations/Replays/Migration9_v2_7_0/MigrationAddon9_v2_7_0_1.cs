using HeroesMatchTracker.Data.Databases;

namespace HeroesMatchTracker.Data.Migrations.Replays
{
    internal class MigrationAddon9_v2_7_0_1 : MigrationMethods<ReplaysContext>, IMigrationAddon
    {
        public void Execute()
        {
            AddColumnToTable("ReplayMatchPlayers", "IsVoiceSilenced", "INTEGER NOT NULL DEFAULT 0");
            AddColumnToTable("ReplayMatchPlayers", "PartySize", "INTEGER NOT NULL DEFAULT 0");
            DropTable("ReplayAllHotsPlayerHeroes");
            DropTable("ReplayHotsLogsUploads");
        }
    }
}
