using HeroesMatchTracker.Data.Databases;
using System.Linq;

namespace HeroesMatchTracker.Data.Migrations.Replays
{
    internal class MigrationAddon14_v2_12_1_1 : MigrationMethods<ReplaysContext>, IMigrationAddon
    {
        public void Execute()
        {
            using (ReplaysContext db = new ReplaysContext())
            {
                var query = from r in db.Replays.AsNoTracking()
                            where r.ReplayBuild == 70200 && r.MapName == "Dead Man's Stand"
                            select r;

                foreach (var item in query)
                {
                    var incorrectMapNames = db.Replays.Where(x => x.ReplayId == item.ReplayId);
                    foreach (var match in incorrectMapNames)
                    {
                        if (match.FileName.Contains("Heroic"))
                            match.MapName = "Escape From Braxis (Heroic)";
                        else
                            match.MapName = "Escape From Braxis";
                    }
                }

                db.SaveChanges();
            }
        }
    }
}
