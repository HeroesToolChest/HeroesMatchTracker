using Heroes.Helpers;
using HeroesMatchTracker.Data.Databases;
using HeroesMatchTracker.Data.Models.Replays;
using HeroesMatchTracker.Data.Queries.Settings;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;

namespace HeroesMatchTracker.Data.Queries.Replays
{
    public class MatchPlayer : NonContextQueriesBase<ReplayMatchPlayer>, IRawDataQueries<ReplayMatchPlayer>
    {
        private UserSettings UserSettings = new UserSettings();

        public IEnumerable<ReplayMatchPlayer> ReadAllRecords()
        {
            using (var db = new ReplaysContext())
            {
                return db.ReplayMatchPlayers.AsNoTracking().ToList();
            }
        }

        public IEnumerable<ReplayMatchPlayer> ReadLastRecords(int amount)
        {
            using (var db = new ReplaysContext())
            {
                return db.ReplayMatchPlayers.AsNoTracking().OrderByDescending(x => x.ReplayId).Take(amount).ToList();
            }
        }

        public IEnumerable<ReplayMatchPlayer> ReadRecordsCustomTop(int amount, string columnName, string orderBy)
        {
            if (string.IsNullOrEmpty(columnName) || string.IsNullOrEmpty(orderBy))
                return new List<ReplayMatchPlayer>();

            if (amount == 0)
                amount = 1;

            using (var db = new ReplaysContext())
            {
                return db.ReplayMatchPlayers.SqlQuery($"SELECT * FROM ReplayMatchPlayers ORDER BY {columnName} {orderBy} LIMIT {amount}").AsNoTracking().ToList();
            }
        }

        public IEnumerable<ReplayMatchPlayer> ReadRecordsWhere(string columnName, string operand, string input)
        {
            if (string.IsNullOrEmpty(columnName) || string.IsNullOrEmpty(operand))
                return new List<ReplayMatchPlayer>();

            if (columnName.Contains("Is"))
            {
                if (input.ToUpperInvariant() == "TRUE")
                    input = "1";
                else if (input.ToUpperInvariant() == "FALSE")
                    input = "0";
            }
            else if (LikeOperatorInputCheck(operand, input))
            {
                input = $"%{input}%";
            }
            else if (input == null)
            {
                input = string.Empty;
            }

            using (var db = new ReplaysContext())
            {
                return db.ReplayMatchPlayers.SqlQuery($"SELECT * FROM ReplayMatchPlayers WHERE {columnName} {operand} @Input", new SQLiteParameter("@Input", input)).AsNoTracking().ToList();
            }
        }

        public IEnumerable<ReplayMatchPlayer> ReadTopRecords(int amount)
        {
            using (var db = new ReplaysContext())
            {
                return db.ReplayMatchPlayers.AsNoTracking().Take(amount).ToList();
            }
        }

        public int ReadHighestLevelOfHero(string character, Season season)
        {
            var replayBuild = HeroesHelpers.Builds.GetReplayBuildsFromSeason(season);

            using (var db = new ReplaysContext())
            {
                var query = from mp in db.ReplayMatchPlayers.AsNoTracking()
                            join r in db.Replays.AsNoTracking() on mp.ReplayId equals r.ReplayId
                            where mp.Character == character &&
                                  mp.PlayerId == UserSettings.UserPlayerId &&
                                  r.ReplayBuild >= replayBuild.Item1 && r.ReplayBuild < replayBuild.Item2
                            orderby mp.CharacterLevel descending
                            select mp.CharacterLevel;

                return query.FirstOrDefault();
            }
        }

        internal void SetPlayerPartyCountsForMatch(ReplaysContext db, long replayId)
        {
            var query = (from mp in db.ReplayMatchPlayers
                        where mp.ReplayId == replayId && mp.PartyValue != 0
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
                var matchPlayers = db.ReplayMatchPlayers.Where(x => x.ReplayId == item.ReplayId && x.PartyValue == item.PartyValue);
                foreach (var player in matchPlayers)
                {
                    player.PartySize = item.PartySize;
                }
            }

            db.SaveChanges();
        }

        internal override long CreateRecord(ReplaysContext db, ReplayMatchPlayer model)
        {
            db.ReplayMatchPlayers.Add(model);
            db.SaveChanges();

            return model.MatchPlayerId;
        }

        internal override long UpdateRecord(ReplaysContext db, ReplayMatchPlayer model)
        {
            throw new NotImplementedException();
        }

        internal override bool IsExistingRecord(ReplaysContext db, ReplayMatchPlayer model)
        {
            throw new NotImplementedException();
        }
    }
}
