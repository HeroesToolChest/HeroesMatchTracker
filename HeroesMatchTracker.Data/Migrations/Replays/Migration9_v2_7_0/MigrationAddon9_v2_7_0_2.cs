using HeroesMatchTracker.Data.Databases;
using System.Linq;

namespace HeroesMatchTracker.Data.Migrations.Replays
{
    internal class MigrationAddon9_v2_7_0_2 : MigrationMethods<ReplaysContext>, IMigrationAddon
    {
        public void Execute()
        {
            using (ReplaysContext db = new ReplaysContext())
            {
                var pullPartyMaps = db.Replays.Where(x => x.MapName == "Pull Party").ToList();

                foreach (var map in pullPartyMaps)
                {
                    var characters = map.ReplayMatchPlayers.Select(x => x.Character);

                    if (characters.All(x => x == "Chromie"))
                    {
                        map.MapName = "Dodge-Brawl";
                    }
                }

                db.SaveChanges();
            }
        }
    }
}
