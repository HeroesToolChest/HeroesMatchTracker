using HeroesMatchTracker.Data.Databases;
using System.Linq;

namespace HeroesMatchTracker.Data.Migrations.Replays
{
    internal class MigrationAddon9_v2_7_0_3 : MigrationMethods<ReplaysContext>, IMigrationAddon
    {
        public void Execute()
        {
            using (ReplaysContext db = new ReplaysContext())
            {
                var query = (from r in db.Replays.AsNoTracking()
                            join mp in db.ReplayMatchPlayers on r.ReplayId equals mp.ReplayId
                            where mp.PartyValue != 0
                            group mp by new
                            {
                                mp.ReplayId,
                                mp.PartyValue,
                            }
                            into grp
                            where grp.Count() > 0
                            let partySize = grp.Count()
                            select new
                            {
                                grp.Key.ReplayId,
                                grp.Key.PartyValue,
                                PartySize = partySize,
                            }).Distinct();

                foreach (var item in query)
                {
                    var matches = db.ReplayMatchPlayers.Where(x => x.ReplayId == item.ReplayId && x.PartyValue == item.PartyValue);
                    foreach (var match in matches)
                    {
                        match.PartySize = item.PartySize;
                    }
                }

                db.SaveChanges();
            }
        }
    }
}
