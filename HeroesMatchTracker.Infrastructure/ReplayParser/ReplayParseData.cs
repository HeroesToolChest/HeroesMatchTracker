﻿using Heroes.StormReplayParser;
using Heroes.StormReplayParser.Player;
using Heroes.StormReplayParser.Replay;
using HeroesMatchTracker.Core;
using HeroesMatchTracker.Core.Entities;
using HeroesMatchTracker.Core.Repositories;
using HeroesMatchTracker.Core.Repositories.HeroesReplays;
using System;
using System.Collections.Generic;
using System.Linq;

namespace HeroesMatchTracker.Infrastructure.ReplayParser
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

        public bool IsReplayExists(IUnitOfWork unitOfWork, string hash)
        {
            if (string.IsNullOrWhiteSpace(hash))
                throw new ArgumentException($"'{nameof(hash)}' cannot be null or whitespace", nameof(hash));

            return _replayMatchRepository.IsExists(unitOfWork, hash);
        }

        public void AddReplay(IUnitOfWork unitOfWork, string? filePath, string hash, StormReplay replay)
        {
            if (unitOfWork is null)
                throw new ArgumentNullException(nameof(unitOfWork));

            if (string.IsNullOrWhiteSpace(filePath))
                throw new ArgumentException($"'{nameof(filePath)}' cannot be null or whitespace", nameof(filePath));

            if (string.IsNullOrWhiteSpace(hash))
                throw new ArgumentException($"'{nameof(hash)}' cannot be null or whitespace", nameof(hash));

            if (replay is null)
                throw new ArgumentNullException(nameof(replay));

            ReplayMatch replayMatch = new();

            SetReplayData(replay, replayMatch, filePath, hash);
            SetMatchPlayers(unitOfWork, replay, replayMatch);
            SetMatchTeamBans(replay, replayMatch);
            SetMatchDraftPicks(replay, replayMatch);

            _replayMatchRepository.Add(unitOfWork, replayMatch);

            unitOfWork.SaveChanges();

            if (replay is null)
                throw new ArgumentNullException(nameof(replay));
        }

        // set basic replay properties
        private static void SetReplayData(StormReplay replay, ReplayMatch replayMatch, string? filePath, string hash)
        {
            replayMatch.ReplayFilePath = filePath;
            replayMatch.GameMode = replay.GameMode;
            replayMatch.MapId = replay.MapInfo.MapId;
            replayMatch.MapName = replay.MapInfo.MapName;
            replayMatch.RandomValue = replay.RandomValue;
            replayMatch.ReplayLength = replay.ReplayLength;
            replayMatch.ReplayVersion = replay.ReplayVersion.ToString();
            replayMatch.TimeStamp = replay.Timestamp;
            replayMatch.Hash = hash;
            replayMatch.HasAI = replay.HasAI;
            replayMatch.HasObservers = replay.HasObservers;
            replayMatch.WinningTeam = replay.WinningTeam;
            replayMatch.Region = replay.Region;

            // TODO: CheckForSpecialMaps()
        }

        private static void CheckForSpecialMaps(string mapName)
        {
            return;

            // pull party map (chromie / stitches)
        }

        private static void AddPlayerScoreResults(StormPlayer player, ReplayMatchPlayer replayMatchPlayer)
        {
            ScoreResult? result = player.ScoreResult;

            if (result is not null)
            {
                replayMatchPlayer.ReplayMatchPlayerScoreResult = new ReplayMatchPlayerScoreResult()
                {
                    Assists = result.Assists,
                    ClutchHealsPerformed = result.ClutchHealsPerformed,
                    CreepDamage = result.CreepDamage,
                    DamageSoaked = result.DamageSoaked,
                    DamageTaken = result.DamageTaken,
                    Deaths = result.Deaths,
                    EscapesPerformed = result.EscapesPerformed,
                    ExperienceContribution = result.ExperienceContribution,
                    Healing = result.Healing,
                    HeroDamage = result.HeroDamage,
                    HighestKillStreak = result.HighestKillStreak,
                    MercCampCaptures = result.MercCampCaptures,
                    MetaExperience = result.MetaExperience,
                    MinionDamage = result.MinionDamage,
                    MinionKills = result.MinionKills,
                    Multikill = result.Multikill,
                    OnFireTimeonFire = result.OnFireTimeonFire,
                    OutnumberedDeaths = result.OutnumberedDeaths,
                    PhysicalDamage = result.PhysicalDamage,
                    ProtectionGivenToAllies = result.ProtectionGivenToAllies,
                    RegenGlobes = result.RegenGlobes,
                    SelfHealing = result.SelfHealing,
                    SiegeDamage = result.SiegeDamage,
                    SoloKills = result.SoloKills,
                    SpellDamage = result.SpellDamage,
                    StructureDamage = result.StructureDamage,
                    SummonDamage = result.SummonDamage,
                    TakeDowns = result.Takedowns,
                    TeamfightDamageTaken = result.TeamfightDamageTaken,
                    TeamfightEscapesPerformed = result.TeamfightEscapesPerformed,
                    TeamfightHealingDone = result.TeamfightHealingDone,
                    TeamfightHeroDamage = result.TeamfightHeroDamage,
                    TimeCCdEnemyHeroes = result.TimeCCdEnemyHeroes,
                    TimeRootingEnemyHeroes = result.TimeRootingEnemyHeroes,
                    TimeSpentDead = result.TimeSpentDead,
                    TimeStunningEnemyHeroes = result.TimeStunningEnemyHeroes,
                    TownKills = result.TownKills,
                    VengeancesPerformed = result.VengeancesPerformed,
                    WatchTowerCaptures = result.WatchTowerCaptures,
                };
            }
        }

        private static void AddPlayerTalents(StormPlayer player, ReplayMatchPlayer replayMatchPlayer)
        {
            if (player.Talents.Count > 0)
            {
                replayMatchPlayer.ReplayMatchPlayerTalent = new ReplayMatchPlayerTalent();

                if (player.Talents.Count > 6)
                {
                    replayMatchPlayer.ReplayMatchPlayerTalent.TalentId20 = player.Talents[6].TalentNameId;
                    replayMatchPlayer.ReplayMatchPlayerTalent.TimeSpanSelected20 = player.Talents[6].Timestamp;
                }

                if (player.Talents.Count > 5)
                {
                    replayMatchPlayer.ReplayMatchPlayerTalent.TalentId16 = player.Talents[5].TalentNameId;
                    replayMatchPlayer.ReplayMatchPlayerTalent.TimeSpanSelected16 = player.Talents[5].Timestamp;
                }

                if (player.Talents.Count > 4)
                {
                    replayMatchPlayer.ReplayMatchPlayerTalent.TalentId13 = player.Talents[4].TalentNameId;
                    replayMatchPlayer.ReplayMatchPlayerTalent.TimeSpanSelected13 = player.Talents[4].Timestamp;
                }

                if (player.Talents.Count > 3)
                {
                    replayMatchPlayer.ReplayMatchPlayerTalent.TalentId10 = player.Talents[3].TalentNameId;
                    replayMatchPlayer.ReplayMatchPlayerTalent.TimeSpanSelected10 = player.Talents[3].Timestamp;
                }

                if (player.Talents.Count > 2)
                {
                    replayMatchPlayer.ReplayMatchPlayerTalent.TalentId7 = player.Talents[2].TalentNameId;
                    replayMatchPlayer.ReplayMatchPlayerTalent.TimeSpanSelected7 = player.Talents[2].Timestamp;
                }

                if (player.Talents.Count > 1)
                {
                    replayMatchPlayer.ReplayMatchPlayerTalent.TalentId4 = player.Talents[1].TalentNameId;
                    replayMatchPlayer.ReplayMatchPlayerTalent.TimeSpanSelected4 = player.Talents[1].Timestamp;
                }

                if (player.Talents.Count > 0)
                {
                    replayMatchPlayer.ReplayMatchPlayerTalent.TalentId1 = player.Talents[0].TalentNameId;
                    replayMatchPlayer.ReplayMatchPlayerTalent.TimeSpanSelected1 = player.Talents[0].Timestamp;
                }
            }
        }

        private static void AddPlayerLoadout(StormPlayer player, ReplayMatchPlayer replayMatchPlayer)
        {
            replayMatchPlayer.ReplayMatchPlayerLoadout = new ReplayMatchPlayerLoadout()
            {
                AnnouncerPackAttributeId = player.PlayerLoadout.AnnouncerPackAttributeId,
                AnnouncerPackId = player.PlayerLoadout.AnnouncerPack,
                BannerAttributeId = player.PlayerLoadout.BannerAttributeId,
                BannerId = player.PlayerLoadout.Banner,
                MountAndMountTintAttributeId = player.PlayerLoadout.MountAndMountTintAttributeId,
                MountAndMountTintId = player.PlayerLoadout.MountAndMountTint,
                SkinAndSkinTintAttributeId = player.PlayerLoadout.SkinAndSkinTintAttributeId,
                SkinAndSkinTintId = player.PlayerLoadout.SkinAndSkinTint,
                SprayAttributeId = player.PlayerLoadout.SprayAttributeId,
                SprayId = player.PlayerLoadout.Spray,
                VoiceLineAttributeId = player.PlayerLoadout.VoiceLineAttributeId,
                VoiceLineId = player.PlayerLoadout.VoiceLine,
            };
        }

        private static void AddPlayerMatchAwards(StormPlayer player, ReplayMatchPlayer replayMatchPlayer)
        {
            if (player.MatchAwards is null)
                return;

            replayMatchPlayer.ReplayMatchAward = new List<ReplayMatchAward>();

            foreach (MatchAwardType matchAward in player.MatchAwards)
            {
                replayMatchPlayer.ReplayMatchAward.Add(new ReplayMatchAward()
                {
                    AwardId = matchAward.ToString(),
                });
            }
        }

        private static void AdjustHeroLevelFromMasterTiers(StormPlayer player, ReplayMatchPlayer replayMatchPlayer)
        {
            if (player.PlayerHero is not null && player.IsAutoSelect is false)
            {
                if (player.HeroMasteryTiers.ToDictionary(x => x.HeroAttributeId, x => x.TierLevel).TryGetValue(player.PlayerHero.HeroAttributeId, out int tierLevel))
                {
                    if (tierLevel == 2 && player.PlayerHero.HeroLevel < 25)
                        replayMatchPlayer.HeroLevel = 25;
                    else if (tierLevel == 3 && player.PlayerHero.HeroLevel < 50)
                        replayMatchPlayer.HeroLevel = 50;
                    else if (tierLevel == 4 && player.PlayerHero.HeroLevel < 75)
                        replayMatchPlayer.HeroLevel = 75;
                    else if (tierLevel == 5 && player.PlayerHero.HeroLevel < 100)
                        replayMatchPlayer.HeroLevel = 100;
                }
            }
        }

        private static void SetPlayerPartyCounts(ReplayMatch replayMatch)
        {
            var query = (from mp in replayMatch.ReplayMatchPlayers
                         where mp.PartyValue is not null
                         group mp by new
                         {
                             mp.PartyValue,
                         }
                         into grp
                         let partySize = grp.Count()
                         select new
                         {
                             grp.Key.PartyValue,
                             PartySize = partySize,
                         }).Distinct();

            foreach (var item in query)
            {
                var matchPlayers = replayMatch.ReplayMatchPlayers!.Where(x => x.PartyValue == item.PartyValue);

                foreach (ReplayMatchPlayer player in matchPlayers)
                {
                    player.PartySize = item.PartySize;
                }
            }
        }

        private static void SetMatchTeamBans(StormReplay replay, ReplayMatch replayMatch)
        {
            IReadOnlyList<string?> blueBans = replay.GetTeamBans(StormTeam.Blue);
            IReadOnlyList<string?> redBans = replay.GetTeamBans(StormTeam.Red);

            replayMatch.ReplayMatchTeamBan = new ReplayMatchTeamBan()
            {
                Team0Ban0 = blueBans[0],
                Team0Ban1 = blueBans[1],
                Team0Ban2 = blueBans[2],
                Team1Ban0 = redBans[0],
                Team1Ban1 = redBans[1],
                Team1Ban2 = redBans[2],
            };
        }

        private static void SetMatchDraftPicks(StormReplay replay, ReplayMatch replayMatch)
        {
            IReadOnlyList<StormDraftPick> draftPicks = replay.DraftPicks;

            if (draftPicks.Count < 1)
                return;

            replayMatch.ReplayMatchDraftPicks = new List<ReplayMatchDraftPick>();

            for (int i = 0; i < draftPicks.Count; i++)
            {
                StormDraftPick draftPick = draftPicks[i];

                replayMatch.ReplayMatchDraftPicks.Add(new ReplayMatchDraftPick()
                {
                    Order = i,
                    HeroSelected = draftPick.HeroSelected,
                    PickType = draftPick.PickType,
                    Team = draftPick.Team,
                    ReplayMatchPlayer = draftPick.Player is null ? null : replayMatch.ReplayMatchPlayers!.First(x => x.ReplayPlayer!.ReplayPlayerToon!.Id == draftPick.Player.ToonHandle!.Id),
                });
            }
        }

        private void SetMatchPlayers(IUnitOfWork unitOfWork, StormReplay replay, ReplayMatch replayMatch)
        {
            replayMatch.ReplayMatchPlayers = new List<ReplayMatchPlayer>(replay.PlayersCount + replay.PlayersObserversCount);

            IEnumerable<StormPlayer> allPlayers = replay.StormPlayers.Concat(replay.StormObservers);
            int playerNum = 0;

            foreach (StormPlayer player in allPlayers)
            {
                if (player is null)
                    continue;

                ReplayMatchPlayer replayMatchPlayer = new()
                {
                    AccountLevel = player.AccountLevel,
                    Difficulty = player.PlayerDifficulty.ToString(),
                    HasActiveBoost = player.HasActiveBoost,
                    HeroId = player.PlayerHero?.HeroId,
                    HeroLevel = player.PlayerHero?.HeroLevel,
                    HeroName = player.PlayerHero?.HeroName,
                    HeroUnitId = player.PlayerHero?.HeroUnitId,
                    HeroAttributeId = player.PlayerHero?.HeroAttributeId,
                    IsAutoSelect = player.IsAutoSelect,
                    IsBlizzardStaff = player.IsBlizzardStaff,
                    IsSilenced = player.IsSilenced,
                    IsVoiceSilenced = player.IsVoiceSilenced,
                    IsWinner = player.IsWinner,
                    PartySize = replay.BattleLobbyPlayerInfoParsed ? 1 : null,
                    PartyValue = player.PartyValue,
                    Team = player.Team,
                    PlayerNumber = playerNum,
                    PlayerType = player.PlayerType,
                };

                AdjustHeroLevelFromMasterTiers(player, replayMatchPlayer);

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

                    // check if this player is the owner
                    if (replay.Owner is not null && player.ToonHandle.Equals(replay.Owner.ToonHandle))
                    {
                        replayMatch.OwnerReplayPlayer = replayMatchPlayer.ReplayPlayer;
                    }
                }

                UpdateOrAddPlayer(unitOfWork, replay.Timestamp, replayMatchPlayer);

                AddPlayerScoreResults(player, replayMatchPlayer);
                AddPlayerTalents(player, replayMatchPlayer);
                AddPlayerLoadout(player, replayMatchPlayer);
                AddPlayerMatchAwards(player, replayMatchPlayer);

                replayMatch.ReplayMatchPlayers.Add(replayMatchPlayer);
            }

            // set all the player's party count
            SetPlayerPartyCounts(replayMatch);
        }

        private void UpdateOrAddPlayer(IUnitOfWork unitOfWork, DateTime replayTimestamp, ReplayMatchPlayer replayMatchPlayer)
        {
            if (replayMatchPlayer is null)
                throw new ArgumentNullException(nameof(replayMatchPlayer));

            if (replayMatchPlayer.ReplayPlayer is null)
                throw new ArgumentNullException(nameof(replayMatchPlayer));

            if (replayMatchPlayer.ReplayPlayer.ReplayPlayerToon != null)
            {
                long? playerId = _replayPlayerToonRepository.GetPlayerId(unitOfWork, replayMatchPlayer.ReplayPlayer.ReplayPlayerToon);

                // existing player?
                if (playerId is not null)
                {
                    // existing player data
                    ReplayPlayer? existingReplayPlayer = _replayPlayerRepository.GetPlayer(unitOfWork, playerId.Value);

                    if (existingReplayPlayer is null)
                        throw new InvalidOperationException($"'{nameof(existingReplayPlayer)}' is null, though it should not be as {nameof(playerId)}: {playerId.Value} exists.");

                    DateTime? latestReplayDateTime = _replayMatchRepository.GetLastestReplayTimeStamp(unitOfWork);

                    if (existingReplayPlayer.BattleTagName is not null && !existingReplayPlayer.BattleTagName.Equals(replayMatchPlayer.ReplayPlayer.BattleTagName, StringComparison.Ordinal))
                    {
                        if (existingReplayPlayer.ReplayOldPlayerInfos is not null)
                        {
                            // if the new battletag of the player is different check if we have it already in the db for the player
                            ReplayOldPlayerInfo? oldPlayerInfo = existingReplayPlayer.ReplayOldPlayerInfos.FirstOrDefault(x => x.BattleTagName is not null && x.BattleTagName.Equals(replayMatchPlayer.ReplayPlayer.BattleTagName, StringComparison.Ordinal));

                            // not found
                            if (oldPlayerInfo is null)
                            {
                                existingReplayPlayer.ReplayOldPlayerInfos.Add(new ReplayOldPlayerInfo()
                                {
                                    BattleTagName = existingReplayPlayer.BattleTagName,
                                    DateAdded = replayTimestamp,
                                });
                            }
                            else
                            {
                                // check date, if the replay is older update the dateAdded we have in the db
                                if (replayTimestamp < oldPlayerInfo.DateAdded)
                                {
                                    oldPlayerInfo.DateAdded = replayTimestamp;
                                }
                            }
                        }
                        else
                        {
                            // replayOldPlayerInfo does not exists yet, so we create it and add the existingReplayPlayer battle tag to it
                            existingReplayPlayer.ReplayOldPlayerInfos = new List<ReplayOldPlayerInfo>()
                            {
                                new()
                                {
                                    BattleTagName = existingReplayPlayer.BattleTagName,
                                    DateAdded = replayTimestamp,
                                },
                            };
                        }

                        // we only want to update the battletag if the current replay is newer than what we have in the db
                        if (latestReplayDateTime is null || replayTimestamp > latestReplayDateTime)
                        {
                            existingReplayPlayer.BattleTagName = replayMatchPlayer.ReplayPlayer.BattleTagName;
                        }
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
    }
}
