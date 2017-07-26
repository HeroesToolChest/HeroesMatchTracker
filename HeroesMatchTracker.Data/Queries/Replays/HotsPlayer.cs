using HeroesMatchTracker.Data.Databases;
using HeroesMatchTracker.Data.Models.Replays;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;

namespace HeroesMatchTracker.Data.Queries.Replays
{
    public class HotsPlayer : NonContextQueriesBase<ReplayAllHotsPlayer>, IRawDataQueries<ReplayAllHotsPlayer>
    {
        public IEnumerable<ReplayAllHotsPlayer> ReadAllRecords()
        {
            using (var db = new ReplaysContext())
            {
                return db.ReplayAllHotsPlayers.AsNoTracking().ToList();
            }
        }

        public IEnumerable<ReplayAllHotsPlayer> ReadLastRecords(int amount)
        {
            using (var db = new ReplaysContext())
            {
                return db.ReplayAllHotsPlayers.AsNoTracking().OrderByDescending(x => x.PlayerId).Take(amount).ToList();
            }
        }

        public IEnumerable<ReplayAllHotsPlayer> ReadRecordsCustomTop(int amount, string columnName, string orderBy)
        {
            if (string.IsNullOrEmpty(columnName) || string.IsNullOrEmpty(orderBy))
                return new List<ReplayAllHotsPlayer>();

            if (amount == 0)
                amount = 1;

            using (var db = new ReplaysContext())
            {
                return db.ReplayAllHotsPlayers.SqlQuery($"SELECT * FROM ReplayAllHotsPlayers ORDER BY {columnName} {orderBy} LIMIT {amount}").AsNoTracking().ToList();
            }
        }

        public IEnumerable<ReplayAllHotsPlayer> ReadRecordsWhere(string columnName, string operand, string input)
        {
            if (string.IsNullOrEmpty(columnName) || string.IsNullOrEmpty(operand))
                return new List<ReplayAllHotsPlayer>();

            if (LikeOperatorInputCheck(operand, input))
                input = $"%{input}%";
            else if (input == null)
                input = string.Empty;

            using (var db = new ReplaysContext())
            {
                return db.ReplayAllHotsPlayers.SqlQuery($"SELECT * FROM ReplayAllHotsPlayers WHERE {columnName} {operand} @Input", new SQLiteParameter("@Input", input)).AsNoTracking().ToList();
            }
        }

        public IEnumerable<ReplayAllHotsPlayer> ReadTopRecords(int amount)
        {
            using (var db = new ReplaysContext())
            {
                return db.ReplayAllHotsPlayers.AsNoTracking().Take(amount).ToList();
            }
        }

        public ReplayAllHotsPlayer ReadRecordFromPlayerId(long playerId)
        {
            using (var db = new ReplaysContext())
            {
                return db.ReplayAllHotsPlayers.AsNoTracking().Where(x => x.PlayerId == playerId).FirstOrDefault();
            }
        }

        public long ReadPlayerIdFromBattleTagName(string battleTagName, int battleNetRegionId)
        {
            using (var db = new ReplaysContext())
            {
                // battleNetId is not unique, player can change their battletag and their battleNetId stays the same
                var result = db.ReplayAllHotsPlayers.AsNoTracking().SingleOrDefault(x => x.BattleTagName == battleTagName && x.BattleNetRegionId == battleNetRegionId);
                if (result != null)
                    return result.PlayerId;
                else
                    return 0;
            }
        }

        internal override long CreateRecord(ReplaysContext db, ReplayAllHotsPlayer model)
        {
            db.ReplayAllHotsPlayers.Add(model);
            db.SaveChanges();

            return model.PlayerId;
        }

        internal override long UpdateRecord(ReplaysContext db, ReplayAllHotsPlayer model)
        {
            ReplayAllHotsPlayer currentRecord;

            if (model.BattleNetId > 0)
            {
                currentRecord = db.ReplayAllHotsPlayers.SingleOrDefault(x => x.BattleNetId == model.BattleNetId &&
                                                                      x.BattleNetRegionId == model.BattleNetRegionId &&
                                                                      x.BattleNetSubId == model.BattleNetSubId);
            }
            else // if its an observer with no battlenetid
            {
                if (!string.IsNullOrEmpty(model.BattleNetTId))
                    currentRecord = db.ReplayAllHotsPlayers.SingleOrDefault(x => x.BattleNetTId == model.BattleNetTId);
                else
                    currentRecord = db.ReplayAllHotsPlayers.SingleOrDefault(x => x.BattleTagName == model.BattleTagName && x.BattleNetRegionId == model.BattleNetRegionId);
            }

            if (currentRecord != null)
            {
                // existing observer record, update the info
                if (currentRecord.BattleNetId < 1)
                {
                    currentRecord.BattleNetId = model.BattleNetId;
                    currentRecord.BattleNetRegionId = model.BattleNetRegionId;
                    currentRecord.BattleNetSubId = model.BattleNetSubId;
                    currentRecord.BattleNetTId = model.BattleNetTId;
                    currentRecord.LastSeen = model.LastSeen;
                    currentRecord.LastSeenBefore = model.LastSeenBefore;
                }

                if (model.LastSeen > currentRecord.LastSeen)
                {
                    if (model.BattleTagName != currentRecord.BattleTagName)
                    {
                        ReplayRenamedPlayer samePlayer = new ReplayRenamedPlayer
                        {
                            PlayerId = currentRecord.PlayerId,
                            BattleNetId = currentRecord.BattleNetId,
                            BattleTagName = currentRecord.BattleTagName,
                            BattleNetRegionId = currentRecord.BattleNetRegionId,
                            BattleNetSubId = currentRecord.BattleNetSubId,
                            BattleNetTId = currentRecord.BattleNetTId,
                            DateAdded = model.LastSeen,
                        };

                        new RenamedPlayer().CreateRecord(db, samePlayer);
                    }

                    currentRecord.BattleTagName = model.BattleTagName; // update the player's battletag, it may have changed
                    currentRecord.BattleNetTId = model.BattleNetTId;
                    currentRecord.LastSeenBefore = currentRecord.LastSeen; // important
                    currentRecord.LastSeen = model.LastSeen;
                }

                if (model.AccountLevel > currentRecord.AccountLevel)
                {
                    currentRecord.AccountLevel = model.AccountLevel;
                }

                currentRecord.Seen += 1;

                db.SaveChanges();
            }

            return currentRecord.PlayerId;
        }

        internal override bool IsExistingRecord(ReplaysContext db, ReplayAllHotsPlayer model)
        {
            if (model.BattleNetId > 0)
            {
                // battleNetId is not unique, player can change their battletag and their battleNetId stays the same
                return db.ReplayAllHotsPlayers.AsNoTracking().Any(x => x.BattleNetId == model.BattleNetId &&
                                                                       x.BattleNetRegionId == model.BattleNetRegionId &&
                                                                       x.BattleNetSubId == model.BattleNetSubId);
            }
            else if (!string.IsNullOrEmpty(model.BattleNetTId))
            {
                // has tag but no BattleNetId - observer
                return db.ReplayAllHotsPlayers.AsNoTracking().Any(x => x.BattleNetTId == model.BattleNetTId);
            }
            else
            {
                // only choice left is to search by BattleTagName
                return db.ReplayAllHotsPlayers.AsNoTracking().Any(x => x.BattleTagName == model.BattleTagName && x.BattleNetRegionId == model.BattleNetRegionId);
            }
        }

        internal long ReadPlayerIdFromBattleNetId(ReplaysContext db, int battleNetId, int battleNetRegionId, int battleNetSubId)
        {
            // battleNetId is not unique, player can change their battletag and their battleNetId stays the same
            return db.ReplayAllHotsPlayers.AsNoTracking().FirstOrDefault(x => x.BattleNetId == battleNetId &&
                                                               x.BattleNetRegionId == battleNetRegionId &&
                                                               x.BattleNetSubId == battleNetSubId).PlayerId;
        }
    }
}
