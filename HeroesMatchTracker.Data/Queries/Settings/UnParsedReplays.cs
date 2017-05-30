using HeroesMatchTracker.Data.Databases;
using HeroesMatchTracker.Data.Models.Settings;
using System.Collections.Generic;
using System.Linq;

namespace HeroesMatchTracker.Data.Queries.Settings
{
    public class UnparsedReplays
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

        public int GetTotalReplaysCount()
        {
            using (var db = new SettingsContext())
            {
                return db.UnParsedReplays.Count();
            }
        }

        public void DeleteAllUnParsedReplays()
        {
            using (var db = new SettingsContext())
            {
                db.Database.ExecuteSqlCommand("DELETE FROM UnParsedReplays");
            }
        }

        public void DeleteUnParsedReplay(int id)
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

        public bool IsExistingReplay(UnParsedReplay unParsedReplay)
        {
            using (var db = new SettingsContext())
            {
                var replay = db.UnParsedReplays.FirstOrDefault(x => x.Build == unParsedReplay.Build && x.FilePath == unParsedReplay.FilePath);
                if (replay != null)
                    return true;
                else
                    return false;
            }
        }
    }
}
