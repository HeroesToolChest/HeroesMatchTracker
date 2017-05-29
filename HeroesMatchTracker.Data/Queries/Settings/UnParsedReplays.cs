using HeroesMatchTracker.Data.Databases;
using HeroesMatchTracker.Data.Models.Settings;
using System.Collections.Generic;
using System.Linq;

namespace HeroesMatchTracker.Data.Queries.Settings
{
    public class UnParsedReplays
    {
        public void CreateUnParsedReplay(UnParsedReplay replay)
        {
            using (var db = new SettingsContext())
            {
                db.UnParsedReplays.Add(replay);
                db.SaveChanges();
            }
        }

        public List<UnParsedReplay> ReadAllReplays()
        {
            using (var db = new SettingsContext())
            {
                return db.UnParsedReplays.ToList();
            }
        }

        public void DeleteAllUnParsedReplays()
        {
            using (var db = new SettingsContext())
            {
                db.Database.ExecuteSqlCommand("TRUNCATE TABLE UnParasedReplays");
            }
        }

        public void DeleteUnParsedReplay(long id)
        {
            using (var db = new SettingsContext())
            {
                var replay = db.UnParsedReplays.FirstOrDefault(x => x.UnParsedReplaysId == id);
                if (replay != null)
                {
                    db.UnParsedReplays.Remove(replay);
                    db.SaveChanges();
                }
            }
        }
    }
}
