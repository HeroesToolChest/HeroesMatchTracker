using HeroesMatchTracker.Data.Databases;
using HeroesMatchTracker.Data.Models.Settings;
using System.Collections.Generic;
using System.Linq;

namespace HeroesMatchTracker.Data.Queries.Settings
{
    public class UnparsedReplays
    {
        public void CreateUnParsedReplay(UnparsedReplay replay)
        {
            using (var db = new SettingsContext())
            {
                db.UnparsedReplays.Add(replay);
                db.SaveChanges();
            }
        }

        public List<UnparsedReplay> ReadAllReplays()
        {
            using (var db = new SettingsContext())
            {
                return db.UnparsedReplays.ToList();
            }
        }

        public int GetTotalReplaysCount()
        {
            using (var db = new SettingsContext())
            {
                return db.UnparsedReplays.Count();
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
                var replay = db.UnparsedReplays.FirstOrDefault(x => x.UnparsedReplaysId == id);
                if (replay != null)
                {
                    db.UnparsedReplays.Remove(replay);
                    db.SaveChanges();
                }
            }
        }

        public bool IsExistingReplay(UnparsedReplay unparsedReplay)
        {
            using (var db = new SettingsContext())
            {
                var replay = db.UnparsedReplays.FirstOrDefault(x => x.Build == unparsedReplay.Build && x.FilePath == unparsedReplay.FilePath);
                if (replay != null)
                    return true;
                else
                    return false;
            }
        }
    }
}
