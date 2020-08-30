using HeroesMatchTracker.Data.Databases;
using HeroesMatchTracker.Data.Generic;
using System.Linq;

namespace HeroesMatchTracker.Data.Migrations.Replays
{
    internal class MigrationAddon7_v2_2_0_1 : MigrationMethods<ReplaysContext>, IMigrationAddon
    {
        public void Execute()
        {
            AddColumnToTable("Replays", "Hash", "TEXT NOT NULL DEFAULT ''");
            AddColumnToTable("ReplayAllHotsPlayers", "LastSeenBefore", "DATETIME");
            AddColumnToTable("ReplayMatchPlayers", "AccountLevel", "INTEGER NOT NULL DEFAULT 0");
            AddColumnToTable("ReplayAllHotsPlayers", "AccountLevel", "INTEGER NOT NULL DEFAULT 0");

            using (ReplaysContext db = new ReplaysContext())
            {
                var records = db.Replays.ToList();

                foreach (var record in records)
                {
                    record.Hash = ReplayHasher.HashReplayOld(record);
                }

                db.SaveChanges();
            }

            using (ReplaysContext db = new ReplaysContext())
            {
                var records = db.ReplayAllHotsPlayers.ToList();

                foreach (var record in records)
                {
                    record.LastSeenBefore = record.LastSeen;
                }

                db.SaveChanges();
            }
        }
    }
}
