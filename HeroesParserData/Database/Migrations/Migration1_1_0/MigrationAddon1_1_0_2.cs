using HeroesParserData.Models.DbModels;
using System.Linq;
using System.Threading.Tasks;

namespace HeroesParserData.Database.Migrations
{
    public class MigrationAddon1_1_0_2 : IMigrationAddon
    {
        public async Task Execute()
        {
            using (HeroesParserDataContext db = new HeroesParserDataContext())
            {
                var records = await db.Database.SqlQuery<ReplayAllHotsPlayer>(@"SELECT *
                                                                                FROM ReplayAllHotsPlayers
                                                                                WHERE BattleNetId != 0
                                                                                GROUP BY BattleNetId
                                                                                HAVING COUNT(*) > 1").ToListAsync();

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
                            DateAdded = player.LastSeen
                        };

                        db.ReplayRenamedPlayers.Add(replayRenamedPlayer);

                        long oldPlayerId = listSamePlayerRecords[i].PlayerId;

                        // remove from ReplayAllHotsPlayerHero
                        var allHotsPlayerHero = db.ReplayAllHotsPlayerHeroes.Where(x => x.PlayerId == oldPlayerId);
                        db.ReplayAllHotsPlayerHeroes.RemoveRange(allHotsPlayerHero);

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

                        await db.SaveChangesAsync();
                    }
                }
            }
        }
    }
}
