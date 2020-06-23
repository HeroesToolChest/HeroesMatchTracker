using HeroesMatchTracker.Data.Databases;
using System.Linq;

namespace HeroesMatchTracker.Data.Migrations.Replays
{
    internal class MigrationAddon15_v2_13_5_1 : MigrationMethods<ReplaysContext>, IMigrationAddon
    {
        public void Execute()
        {
            using (ReplaysContext db = new ReplaysContext())
            {
                var query = db.Replays.Where(x => x.ReplayBuild == 79033);

                foreach (var item in query)
                {
                    db.ReplayMatchTeamObjectives.RemoveRange(db.ReplayMatchTeamObjectives.Where(x => x.ReplayId == item.ReplayId));
                    db.ReplayMatchTeamLevels.RemoveRange(db.ReplayMatchTeamLevels.Where(x => x.ReplayId == item.ReplayId));
                    db.ReplayMatchTeamExperiences.RemoveRange(db.ReplayMatchTeamExperiences.Where(x => x.ReplayId == item.ReplayId));
                    db.ReplayMatchTeamBans.RemoveRange(db.ReplayMatchTeamBans.Where(x => x.ReplayId == item.ReplayId));
                    db.ReplayMatchPlayerTalents.RemoveRange(db.ReplayMatchPlayerTalents.Where(x => x.ReplayId == item.ReplayId));
                    db.ReplayMatchPlayerScoreResults.RemoveRange(db.ReplayMatchPlayerScoreResults.Where(x => x.ReplayId == item.ReplayId));
                    db.ReplayMatchMessages.RemoveRange(db.ReplayMatchMessages.Where(x => x.ReplayId == item.ReplayId));
                    db.ReplayMatchDraftPicks.RemoveRange(db.ReplayMatchDraftPicks.Where(x => x.ReplayId == item.ReplayId));
                    db.ReplayMatchAwards.RemoveRange(db.ReplayMatchAwards.Where(x => x.ReplayId == item.ReplayId));
                    db.ReplayHotsApiUploads.RemoveRange(db.ReplayHotsApiUploads.Where(x => x.ReplayId == item.ReplayId));
                    db.ReplayMatchPlayers.RemoveRange(db.ReplayMatchPlayers.Where(x => x.ReplayId == item.ReplayId));
                }

                db.SaveChanges();
            }
        }
    }
}
