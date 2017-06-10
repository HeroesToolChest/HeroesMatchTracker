using HeroesMatchTracker.Data.Databases;
using HeroesMatchTracker.Data.Models.Settings;
using System.Collections.Generic;
using System.Linq;

namespace HeroesMatchTracker.Data.Queries.Settings
{
    public class FailedReplays
    {
        public void CreateFailedReplay(FailedReplay replay)
        {
            using (var db = new SettingsContext())
            {
                db.FailedReplays.Add(replay);
                db.SaveChanges();
            }
        }

        public List<FailedReplay> ReadAllReplays()
        {
            using (var db = new SettingsContext())
            {
                return db.FailedReplays.ToList();
            }
        }

        public int GetTotalReplaysCount()
        {
            using (var db = new SettingsContext())
            {
                return db.FailedReplays.Count();
            }
        }

        public void DeleteAllFailedReplays()
        {
            using (var db = new SettingsContext())
            {
                db.Database.ExecuteSqlCommand("DELETE FROM FailedReplays");
            }
        }

        public void DeleteFailedReplay(int id)
        {
            using (var db = new SettingsContext())
            {
                var replay = db.FailedReplays.FirstOrDefault(x => x.FailedReplayId == id);
                if (replay != null)
                {
                    db.FailedReplays.Remove(replay);
                    db.SaveChanges();
                }
            }
        }

        public bool IsExistingReplay(FailedReplay failedReplay)
        {
            using (var db = new SettingsContext())
            {
                var replay = db.FailedReplays.FirstOrDefault(x => x.Build == failedReplay.Build && x.FilePath == failedReplay.FilePath);
                if (replay != null)
                    return true;
                else
                    return false;
            }
        }
    }
}
