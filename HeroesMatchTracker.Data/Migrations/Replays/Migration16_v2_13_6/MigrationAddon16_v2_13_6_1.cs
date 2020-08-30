using Heroes.ReplayParser;
using HeroesMatchTracker.Data.Databases;
using HeroesMatchTracker.Data.Generic;
using HeroesMatchTracker.Data.Models.Replays;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace HeroesMatchTracker.Data.Migrations.Replays
{
    internal class MigrationAddon16_v2_13_6_1 : MigrationMethods<ReplaysContext>, IMigrationAddon
    {
        public void Execute()
        {
            using (ReplaysContext db = new ReplaysContext())
            {
                List<ReplayMatch> records = db.Replays
                    .Include(x => x.ReplayMatchPlayers.Select(p => p.ReplayAllHotsPlayer))
                    .ToList();

                foreach (ReplayMatch record in records)
                {
                    IEnumerable<Player> players = record.ReplayMatchPlayers.Select(x => new Player()
                    {
                        Character = x.Character,
                    });

                    record.Hash = ReplayHasher.HashReplay(record, players);
                }

                db.SaveChanges();
            }
        }
    }
}
