using HeroesMatchTracker.Data.Databases;
using HeroesMatchTracker.Data.Models.Replays;
using System.Linq;

namespace HeroesMatchTracker.Data.Migrations.Replays
{
    internal class MigrationAddon1_v1_2_0_2 : MigrationMethods<ReplaysContext>, IMigrationAddon
    {
        public void Execute()
        {
            using (ReplaysContext db = new ReplaysContext())
            {
                var records = db.Database.SqlQuery<ReplayAllHotsPlayer>(@"SELECT *
                                                                        FROM ReplayAllHotsPlayers
                                                                        WHERE BattleNetId != 0
                                                                        GROUP BY BattleNetId
                                                                        HAVING COUNT(*) > 1").ToList();

                foreach (var record in records)
                {
                    var samePlayerRecords = db.ReplayAllHotsPlayers.Where(x => x.BattleNetId == record.BattleNetId).OrderBy(x => x.LastSeen);
                    var listSamePlayerRecords = samePlayerRecords.ToList();

                    long originalPlayerId = listSamePlayerRecords[0].PlayerId;
                    listSamePlayerRecords[0].Seen = samePlayerRecords.Sum(x => x.Seen);

                    // copy the records over to the RenamedPlayer table
                    for (int i = 1; i < listSamePlayerRecords.Count; i++)
                    {
                        var player = listSamePlayerRecords[i];
                        ReplayRenamedPlayer replayRenamedPlayer = new ReplayRenamedPlayer
                        {
                            PlayerId = originalPlayerId,
                            BattleTagName = player.BattleTagName,
                            BattleNetId = player.BattleNetId,
                            BattleNetRegionId = player.BattleNetRegionId,
                            BattleNetSubId = player.BattleNetSubId,
                            DateAdded = player.LastSeen,
                        };

                        db.ReplayRenamedPlayers.Add(replayRenamedPlayer);

                        long oldPlayerId = listSamePlayerRecords[i].PlayerId;

                        // update ReplayMatchPlayers
                        db.ReplayMatchPlayers.Where(x => x.PlayerId == oldPlayerId)
                                             .ToList()
                                             .ForEach(x => x.PlayerId = originalPlayerId);

                        // update ReplayMatchPlayerTalents
                        db.ReplayMatchPlayerTalents.Where(x => x.PlayerId == oldPlayerId)
                                                   .ToList()
                                                   .ForEach(x => x.PlayerId = originalPlayerId);

                        // update ReplayMatchPlayerScoreResults
                        db.ReplayMatchPlayerScoreResults.Where(x => x.PlayerId == oldPlayerId)
                                                        .ToList()
                                                        .ForEach(x => x.PlayerId = originalPlayerId);

                        // update ReplayMatchTeamObjectives
                        db.ReplayMatchTeamObjectives.Where(x => x.PlayerId == oldPlayerId)
                                                    .ToList()
                                                    .ForEach(x => x.PlayerId = originalPlayerId);

                        // update ReplayMatchAwards
                        db.ReplayMatchAwards.Where(x => x.PlayerId == oldPlayerId)
                                            .ToList()
                                            .ForEach(x => x.PlayerId = originalPlayerId);

                        // remove from ReplayAllHotsPlayer
                        var allHotsPlayer = db.ReplayAllHotsPlayers.Where(x => x.PlayerId == oldPlayerId);
                        db.ReplayAllHotsPlayers.RemoveRange(allHotsPlayer);

                        db.SaveChanges();
                    }
                }
            }
        }
    }
}
