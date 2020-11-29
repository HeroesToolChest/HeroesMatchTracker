using Heroes.StormReplayParser;
using Heroes.StormReplayParser.Player;
using HeroesMatchTracker.Core.Database.HeroesReplays;
using HeroesMatchTracker.Core.Services;
using HeroesMatchTracker.Infrastructure.Database.Contexts;
using HeroesMatchTracker.Shared;
using HeroesMatchTracker.Shared.Entities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace HeroesMatchTracker.Infrastructure
{
    public class ReplayParseData : IReplayParseData
    {
        private readonly IReplayMatchRepository _replayMatchRepository;
        private readonly IReplayPlayerToonRepository _replayPlayerToonRepository;
        private readonly IReplayPlayerRepository _replayPlayerRepository;

        public ReplayParseData(
            IReplayMatchRepository replayMatchRepository,
            IReplayPlayerToonRepository replayPlayerToonRepository,
            IReplayPlayerRepository replayPlayerRepository)
        {
            _replayMatchRepository = replayMatchRepository;
            _replayPlayerToonRepository = replayPlayerToonRepository;
            _replayPlayerRepository = replayPlayerRepository;
        }

        public string GetReplayHash(StormReplay replay)
        {
            if (replay is null)
                throw new ArgumentNullException(nameof(replay));

            return ReplayHasher.HashReplay(replay);
        }

        public bool IsReplayExists(HeroesReplaysDbContext context, string hash)
        {
            if (string.IsNullOrWhiteSpace(hash))
                throw new ArgumentException($"'{nameof(hash)}' cannot be null or whitespace", nameof(hash));

            return _replayMatchRepository.IsExists(context, hash);
        }

        public void AddReplay(HeroesReplaysDbContext context, string fileName, string hash, StormReplay replay)
        {
            if (context is null)
                throw new ArgumentNullException(nameof(context));

            if (string.IsNullOrWhiteSpace(fileName))
                throw new ArgumentException($"'{nameof(fileName)}' cannot be null or whitespace", nameof(fileName));

            if (string.IsNullOrWhiteSpace(hash))
                throw new ArgumentException($"'{nameof(hash)}' cannot be null or whitespace", nameof(hash));

            if (replay is null)
                throw new ArgumentNullException(nameof(replay));

            ReplayMatch replayMatch = new ();

            SetReplayData(replay, replayMatch, fileName, hash);
            SetMatchPlayers(context, replay, replayMatch);

            context.Replays.Add(replayMatch);

            _replayMatchRepository.SaveChanges(context);

            if (replay is null)
                throw new ArgumentNullException(nameof(replay));
        }

        private static void SetReplayData(StormReplay replay, ReplayMatch replayMatch, string fileName, string hash)
        {
            replayMatch.FileName = Path.GetFileName(fileName) ?? string.Empty;
            replayMatch.GameMode = replay.GameMode;
            replayMatch.MapId = replay.MapInfo.MapId;
            replayMatch.MapName = replay.MapInfo.MapName;
            replayMatch.RandomValue = replay.RandomValue;
            replayMatch.ReplayLength = replay.ReplayLength;
            replayMatch.ReplayVersion = replay.ReplayVersion.ToString();
            replayMatch.TimeStamp = replay.Timestamp;
            replayMatch.Hash = hash;

            // TODO: CheckForSpecialMaps()
        }

        private static void CheckForSpecialMaps(string mapName)
        {
            return;

            // pull party map (chromie / stitches
        }

        private void SetMatchPlayers(HeroesReplaysDbContext context, StormReplay replay, ReplayMatch replayMatch)
        {
            replayMatch.ReplayMatchPlayers = new List<ReplayMatchPlayer>(replay.PlayersCount + replay.PlayersObserversCount);

            int playerNum = 0;
            foreach (StormPlayer player in replay.StormPlayers)
            {
                if (player is null)
                    continue;

                ReplayMatchPlayer replayMatchPlayer = new ReplayMatchPlayer()
                {
                    AccountLevel = player.AccountLevel,
                    Difficulty = player.PlayerDifficulty.ToString(),
                    HasActiveBoost = player.HasActiveBoost,
                    HeroId = player.PlayerHero?.HeroId,
                    HeroLevel = player.PlayerHero?.HeroLevel,
                    HeroName = player.PlayerHero?.HeroName,
                    IsAutoSelect = player.IsAutoSelect,
                    IsBlizzardStaff = player.IsBlizzardStaff,
                    IsSilenced = player.IsSilenced,
                    IsVoiceSilenced = player.IsVoiceSilenced,
                    IsWinner = player.IsWinner,
                    PartySize = 0,
                    PartyValue = player.PartyValue,
                    Team = player.Team,
                    PlayerNumber = playerNum,
                    PlayerType = player.PlayerType,
                };

                replayMatchPlayer.ReplayPlayer = new ReplayPlayer()
                {
                    AccountLevel = player.AccountLevel ?? 0,
                    ShortcutId = player.ToonHandle?.ShortcutId,
                    BattleTagName = player.BattleTagName,
                    LastSeen = replay.Timestamp,
                    LastSeenBefore = null,
                    Seen = 1,
                };

                if (player.ToonHandle is not null)
                {
                    replayMatchPlayer.ReplayPlayer.ReplayPlayerToon = new ReplayPlayerToon()
                    {
                        Id = player.ToonHandle.Id,
                        ProgramId = player.ToonHandle.ProgramId,
                        Realm = player.ToonHandle.Realm,
                        Region = player.ToonHandle.Region,
                    };
                }

                UpdateOrAddPlayer(context, replay.Timestamp, replayMatchPlayer);

                replayMatch.ReplayMatchPlayers.Add(replayMatchPlayer);
            }
        }

        private void UpdateOrAddPlayer(HeroesReplaysDbContext context, DateTime replayTimestamp, ReplayMatchPlayer replayMatchPlayer)
        {
            if (replayMatchPlayer is null)
                throw new ArgumentNullException(nameof(replayMatchPlayer));

            if (replayMatchPlayer.ReplayPlayer is null)
                throw new ArgumentNullException(nameof(replayMatchPlayer));

            if (replayMatchPlayer.ReplayPlayer.ReplayPlayerToon != null)
            {
                long? playerId = _replayPlayerToonRepository.GetPlayerId(context, replayMatchPlayer.ReplayPlayer.ReplayPlayerToon);

                // existing player?
                if (playerId is not null)
                {
                    // existing player data
                    ReplayPlayer? existingReplayPlayer = _replayPlayerRepository.GetPlayer(context, playerId.Value);

                    if (existingReplayPlayer is null)
                        throw new InvalidOperationException($"'{nameof(existingReplayPlayer)}' is null, though it should not be as {nameof(playerId)}: {playerId.Value} exists.");

                    DateTime? latestReplayDateTime = _replayMatchRepository.GetLastestReplayTimeStamp(context);

                    if (existingReplayPlayer.BattleTagName is not null && !existingReplayPlayer.BattleTagName.Equals(replayMatchPlayer.BattleTagName, StringComparison.Ordinal))
                    {
                        // if the new battletag of the player is different check if we have it already in the db for the player
                        ReplayOldPlayerInfo? oldPlayerInfo = existingReplayPlayer.ReplayOldPlayerInfos!.FirstOrDefault(x => x.BattleTagName == existingReplayPlayer.BattleTagName && x.DateAdded == replayTimestamp);

                        // not found
                        if (oldPlayerInfo is null)
                        {
                            existingReplayPlayer.ReplayOldPlayerInfos!.Add(new ReplayOldPlayerInfo()
                            {
                                BattleTagName = replayMatchPlayer.BattleTagName,
                                DateAdded = replayTimestamp,
                            });
                        }
                    }

                    // we only want to update the battletag if the current replay is newer than what we have in the db
                    if (latestReplayDateTime is null || replayTimestamp > latestReplayDateTime)
                    {
                        existingReplayPlayer.BattleTagName = replayMatchPlayer.BattleTagName;
                    }

                    existingReplayPlayer.Seen += 1;
                    existingReplayPlayer.LastSeenBefore = existingReplayPlayer.LastSeen;
                    existingReplayPlayer.LastSeen = replayTimestamp;

                    // update the account level if it is higher
                    if (replayMatchPlayer.AccountLevel.HasValue && replayMatchPlayer.AccountLevel > existingReplayPlayer.AccountLevel)
                    {
                        existingReplayPlayer.AccountLevel = replayMatchPlayer.AccountLevel.Value;
                    }
                }
            }
        }

        private void AddPlayerScoreResults(StormPlayer player, ReplayMatchPlayer replayMatchPlayer)
        {
            //player.
            ReplayMatchPlayerScoreResult replayMatchPlayerScoreResult = new ReplayMatchPlayerScoreResult()
            {

            };

            //replayMatchPlayer.
        }
    }
}
