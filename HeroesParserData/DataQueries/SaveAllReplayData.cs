using Heroes.ReplayParser;
using HeroesParserData.DataQueries.ReplayData;
using HeroesParserData.Models.DbModels;
using System;
using System.Collections.Generic;
using System.Linq;
using static Heroes.ReplayParser.DataParser;

namespace HeroesParserData.DataQueries
{
    public static class SaveAllReplayData
    {
        public static ReplayParseResult Save(Heroes.ReplayParser.Replay replay, string fileName, out DateTime parsedDateTime)
        {
            using (var db = new HeroesParserDataContext())
            {
                using (var dbTransaction = db.Database.BeginTransaction())
                {
                    try
                    {
                        #region Save basic data
                        Models.DbModels.Replay replayData = new Models.DbModels.Replay
                        {
                            Frames = replay.Frames,
                            GameMode = replay.GameMode,
                            GameSpeed = replay.GameSpeed.ToString(),
                            IsGameEventsParsed = replay.IsGameEventsParsedSuccessfully,
                            MapName = replay.Map,
                            RandomValue = replay.RandomValue,
                            ReplayBuild = replay.ReplayBuild,
                            ReplayLength = replay.ReplayLength,
                            ReplayVersion = replay.ReplayVersion,
                            TeamSize = replay.TeamSize,
                            TimeStamp = replay.Timestamp,
                            FileName = fileName
                        };

                        parsedDateTime = replay.Timestamp;

                        // check if replay was added to database already
                        if (Query.Replay.IsExistingReplay(replayData, db))
                        {
                            parsedDateTime = new DateTime();
                            return ReplayParseResult.Duplicate;
                        }

                        long replayId = Query.Replay.CreateRecord(db, replayData);
                        #endregion Save basic data

                        SavePlayerRelatedData(db, replay, replayId);
                        SaveMatchTeamBans(db, replay, replayId);
                        SaveMatchTeamLevels(db, replay, replayId);
                        SaveMatchTeamExperience(db, replay, replayId);
                        SaveMatchMessage(db, replay, replayId);
                        SaveMatchObjectives(db, replay, replayId);

                        dbTransaction.Commit();

                        return ReplayParseResult.Saved;
                    }
                    catch (Exception)
                    {
                        dbTransaction.Rollback();
                        throw;
                    }
                }
            }
        }

        private static void SavePlayerRelatedData(HeroesParserDataContext db, Heroes.ReplayParser.Replay replay, long replayId)
        {
            int i = 0;

            Player[] players;

            if (replay.ReplayBuild > 39445)
                players = replay.ClientListByUserID;
            else
                players = replay.Players;

            foreach (var player in players)
            {
                if (player == null)
                    break;

                ReplayAllHotsPlayer hotsPlayer = new ReplayAllHotsPlayer
                {
                    BattleTagName = Utilities.GetBattleTagName(player.Name, player.BattleTag),
                    BattleNetId = player.BattleNetId,
                    BattleNetRegionId = player.BattleNetRegionId,
                    BattleNetSubId = player.BattleNetSubId,
                    Seen = 1
                };

                long playerId;

                if (Query.HotsPlayer.IsExistingHotsPlayer(db, hotsPlayer))
                    playerId = Query.HotsPlayer.UpdateRecord(db, hotsPlayer);
                else
                    playerId = Query.HotsPlayer.CreateRecord(db, hotsPlayer);

                if (player.Character == null && replay.GameMode == GameMode.Custom)
                {
                    player.Team = 4;
                    player.Character = "None";
                    SaveMatchPlayers(db, replayId, playerId, -1, player);
                }
                else
                {
                    SaveMatchPlayers(db, playerId, replayId, i, player);
                    SaveMatchPlayerScoreResults(db, replayId, playerId, player);
                    SaveMatchPlayerTalents(db, replayId, playerId, player);

                    i++;
                }
            }
        }

        private static void SaveMatchPlayers(HeroesParserDataContext db, long replayId, long playerId, int playerNumber, Player player)
        {
            ReplayMatchPlayer replayPlayer = new ReplayMatchPlayer
            {
                ReplayId = replayId,
                PlayerId = playerId,
                Character = player.Character,
                CharacterLevel = player.CharacterLevel,
                Difficulty = player.Difficulty.ToString(),
                Handicap = player.Handicap,
                IsAutoSelect = player.IsAutoSelect,
                IsSilenced = player.IsSilenced,
                IsWinner = player.IsWinner,
                MountAndMountTint = player.MountAndMountTint,
                PlayerNumber = playerNumber,
                SkinAndSkinTint = player.SkinAndSkinTint,
                Team = player.Team
            };

            Query.MatchPlayer.CreateRecord(db, replayPlayer);
        }

        private static void SaveMatchPlayerScoreResults(HeroesParserDataContext db, long replayId, long playerId, Player player)
        {
            ScoreResult sr = player.ScoreResult;

            ReplayMatchPlayerScoreResult playerScore = new ReplayMatchPlayerScoreResult
            {
                ReplayId = replayId,
                Assists = sr.Assists,
                PlayerId = playerId,
                CreepDamage = sr.CreepDamage,
                DamageTaken = sr.DamageTaken,
                Deaths = sr.Deaths,
                ExperienceContribution = sr.ExperienceContribution,
                Healing = sr.Healing,
                HeroDamage = sr.HeroDamage,
                MercCampCaptures = sr.MercCampCaptures,
                MetaExperience = sr.MetaExperience,
                MinionDamage = sr.MinionDamage,
                SelfHealing = sr.SelfHealing,
                SiegeDamage = sr.SiegeDamage,
                SoloKills = sr.SoloKills,
                StructureDamage = sr.StructureDamage,
                SummonDamage = sr.SummonDamage,
                TakeDowns = sr.Takedowns,
                TimeCCdEnemyHeroes = sr.TimeCCdEnemyHeroes.HasValue ? sr.TimeCCdEnemyHeroes.Value.Ticks : (long?)null,
                TimeSpentDead = sr.TimeSpentDead,
                TownKills = sr.TownKills,
                WatchTowerCaptures = sr.WatchTowerCaptures
            };

            Query.MatchPlayerScoreResult.CreateRecord(db, playerScore);
        }

        private static void SaveMatchPlayerTalents(HeroesParserDataContext db, long replayId, long playerId, Player player)
        {
            Talent[] talents = player.Talents;
            Talent[] talentArray = new Talent[7]; // hold all 7 talents

            // add known talents
            for (int i = 0; i < talents.Count(); i++)
            {
                talentArray[i] = new Talent();
                talentArray[i].TalentID = talents[i].TalentID;
                talentArray[i].TalentName = talents[i].TalentName;
                talentArray[i].TimeSpanSelected = talents[i].TimeSpanSelected;
            }
            // make the rest null
            for (int i = talents.Count(); i < 7; i++)
            {
                talentArray[i] = new Talent();
                talentArray[i].TalentID = null;
                talentArray[i].TalentName = null;
                talentArray[i].TimeSpanSelected = null;
            }

            ReplayMatchPlayerTalent replayTalent = new ReplayMatchPlayerTalent
            {
                ReplayId = replayId,
                PlayerId = playerId,
                Character = player.Character,
                TalentId1 = talentArray[0].TalentID,
                TalentName1 = talentArray[0].TalentName,
                TimeSpanSelected1 = talentArray[0].TimeSpanSelected,
                TalentId4 = talentArray[1].TalentID,
                TalentName4 = talentArray[1].TalentName,
                TimeSpanSelected4 = talentArray[1].TimeSpanSelected,
                TalentId7 = talentArray[2].TalentID,
                TalentName7 = talentArray[2].TalentName,
                TimeSpanSelected7 = talentArray[2].TimeSpanSelected,
                TalentId10 = talentArray[3].TalentID,
                TalentName10 = talentArray[3].TalentName,
                TimeSpanSelected10 = talentArray[3].TimeSpanSelected,
                TalentId13 = talentArray[4].TalentID,
                TalentName13 = talentArray[4].TalentName,
                TimeSpanSelected13 = talentArray[4].TimeSpanSelected,
                TalentId16 = talentArray[5].TalentID,
                TalentName16 = talentArray[5].TalentName,
                TimeSpanSelected16 = talentArray[5].TimeSpanSelected,
                TalentId20 = talentArray[6].TalentID,
                TalentName20 = talentArray[6].TalentName,
                TimeSpanSelected20 = talentArray[6].TimeSpanSelected,
            };

            Query.MatchPlayerTalent.CreateRecord(db, replayTalent);
        }

        private static void SaveMatchTeamBans(HeroesParserDataContext db, Heroes.ReplayParser.Replay replay, long replayId)
        {
            if (replay.GameMode == GameMode.UnrankedDraft || replay.GameMode == GameMode.HeroLeague || replay.GameMode == GameMode.TeamLeague || replay.GameMode == GameMode.Custom)
            {
                if (replay.TeamHeroBans != null)
                {
                    ReplayMatchTeamBan replayTeamBan = new ReplayMatchTeamBan
                    {
                        ReplayId = replayId,
                        Team0Ban0 = replay.TeamHeroBans[0][0],
                        Team0Ban1 = replay.TeamHeroBans[0][1],
                        Team1Ban0 = replay.TeamHeroBans[1][0],
                        Team1Ban1 = replay.TeamHeroBans[1][1]
                    };

                    if (replayTeamBan.Team0Ban0 != null || replayTeamBan.Team0Ban1 != null || replayTeamBan.Team1Ban0 != null || replayTeamBan.Team1Ban1 != null)
                        Query.MatchTeamBan.CreateRecord(db, replayTeamBan);
                }
            }
        }

        private static void SaveMatchTeamLevels(HeroesParserDataContext db, Heroes.ReplayParser.Replay replay, long replayId)
        {
            Dictionary<int, TimeSpan?> team0 = replay.TeamLevels[0];
            Dictionary<int, TimeSpan?> team1 = replay.TeamLevels[1];

            if (team0 != null || team1 != null)
            {
                int levelDiff = team0.Count - team1.Count;
                if (levelDiff > 0)
                {
                    for (int i = team1.Count + 1; i <= team0.Count; i++)
                    {
                        team1.Add(i, null);
                    }
                }
                else if (levelDiff < 0)
                {
                    for (int i = team0.Count + 1; i <= team1.Count; i++)
                    {
                        team0.Add(i, null);
                    }
                }

                for (int level = 1; level <= team0.Count; level++)
                {
                    ReplayMatchTeamLevel replayTeamLevel = new ReplayMatchTeamLevel
                    {
                        ReplayId = replayId,
                        TeamTime0 = team0[level],
                        Team0Level = team0[level].HasValue ? level : (int?)null,

                        TeamTime1 = team1[level],
                        Team1Level = team1[level].HasValue ? level : (int?)null,

                    };

                    Query.MatchTeamLevel.CreateRecord(db, replayTeamLevel);
                }
            }
        }

        private static void SaveMatchTeamExperience(HeroesParserDataContext db, Heroes.ReplayParser.Replay replay, long replayId)
        {
            var xpTeam0 = replay.TeamPeriodicXPBreakdown[0];
            var xpTeam1 = replay.TeamPeriodicXPBreakdown[1];

            if (xpTeam0 == null && xpTeam1 == null)
                return;

            if (xpTeam0.Count != xpTeam1.Count)
            {
                throw new Exception("Teams don't have equal periodic xp gain.");
            }

            for (int i = 0; i < xpTeam0.Count; i++)
            {
                var x = xpTeam0[i];
                var y = xpTeam1[i];

                ReplayMatchTeamExperience xp = new ReplayMatchTeamExperience
                {
                    ReplayId = replayId,
                    Time = x.TimeSpan,

                    Team0TeamLevel = x.TeamLevel,
                    Team0CreepXP = x.CreepXP,
                    Team0HeroXP = x.HeroXP,
                    Team0MinionXP = x.MinionXP,
                    Team0StructureXP = x.StructureXP,
                    Team0TrickleXP = x.TrickleXP,

                    Team1TeamLevel = y.TeamLevel,
                    Team1CreepXP = y.CreepXP,
                    Team1HeroXP = y.HeroXP,
                    Team1MinionXP = y.MinionXP,
                    Team1StructureXP = y.StructureXP,
                    Team1TrickleXP = y.TrickleXP,
                };

                Query.MatchTeamExperience.CreateRecord(db, xp);
            }
        }

        private static void SaveMatchMessage(HeroesParserDataContext db, Heroes.ReplayParser.Replay replay, long replayId)
        {
            foreach (var message in replay.Messages)
            {
                var messageEventType = message.MessageEventType;
                var player = message.MessageSender;

                if (messageEventType == ReplayMessageEvents.MessageEventType.SChatMessage)
                {
                    var chatMessage = message.ChatMessage;

                    ReplayMatchMessage chat = new ReplayMatchMessage
                    {
                        ReplayId = replayId,
                        CharacterName = player != null? player.Character : string.Empty,
                        Message = chatMessage.Message,
                        MessageEventType = messageEventType.ToString(),
                        MessageTarget = chatMessage.MessageTarget.ToString(),
                        PlayerName = player != null ? player.Name : string.Empty,
                        TimeStamp = message.Timestamp             
                    };

                    Query.MatchMessage.CreateRecord(db, chat);
                }
                else if (messageEventType == ReplayMessageEvents.MessageEventType.SPingMessage)
                {
                    var pingMessage = message.PingMessage;

                    ReplayMatchMessage ping = new ReplayMatchMessage
                    {
                        ReplayId = replayId,
                        CharacterName = player != null ? player.Character : string.Empty,
                        Message = "used a ping",
                        MessageEventType = messageEventType.ToString(),
                        MessageTarget = pingMessage.MessageTarget.ToString(),
                        PlayerName = player != null ? player.Name : string.Empty,
                        TimeStamp = message.Timestamp
                    };

                    Query.MatchMessage.CreateRecord(db, ping);
                }

                else if (messageEventType == ReplayMessageEvents.MessageEventType.SPlayerAnnounceMessage)
                {
                    var announceMessage = message.PlayerAnnounceMessage;

                    ReplayMatchMessage announce = new ReplayMatchMessage
                    {
                        ReplayId = replayId,
                        CharacterName = player != null ? player.Character : string.Empty,
                        Message = $"announce {announceMessage.AnnouncementType.ToString()}",
                        MessageEventType = messageEventType.ToString(),
                        MessageTarget = "Allies",
                        PlayerName = player != null ? player.Name : string.Empty,
                        TimeStamp = message.Timestamp
                    };

                    Query.MatchMessage.CreateRecord(db, announce);
                }          
            }
        }

        private static void SaveMatchObjectives(HeroesParserDataContext db, Heroes.ReplayParser.Replay replay, long replayId)
        {
            var objTeam0 = replay.TeamObjectives[0];
            var objTeam1 = replay.TeamObjectives[1];

            if (objTeam0.Count > 0 && objTeam0 != null)
            {
                foreach (var objCount in objTeam0)
                {
                    var player = objCount.Player;

                    ReplayMatchTeamObjective obj = new ReplayMatchTeamObjective
                    {
                        Team = 0,
                        PlayerId = player != null? Query.HotsPlayer.ReadPlayerIdFromBattleNetId(db, Utilities.GetBattleTagName(player.Name, player.BattleTag), player.BattleNetId) : (long?)null,
                        ReplayId = replayId,
                        TeamObjectiveType = objCount.TeamObjectiveType.ToString(),
                        TimeStamp = objCount.TimeSpan,
                        Value = objCount.Value
                    };

                    Query.MatchTeamObjective.CreateRecord(db, obj);
                }
            }

            if (objTeam1.Count > 0 && objTeam1 != null)
            {
                foreach (var objCount in objTeam1)
                {
                    var player = objCount.Player;

                    ReplayMatchTeamObjective obj = new ReplayMatchTeamObjective
                    {
                        Team = 1,
                        PlayerId = player != null ? Query.HotsPlayer.ReadPlayerIdFromBattleNetId(db, Utilities.GetBattleTagName(player.Name, player.BattleTag), player.BattleNetId) : (long?)null,
                        ReplayId = replayId,
                        TeamObjectiveType = objCount.TeamObjectiveType.ToString(),
                        TimeStamp = objCount.TimeSpan,
                        Value = objCount.Value
                    };

                    Query.MatchTeamObjective.CreateRecord(db, obj);
                }
            }
        }
    }
}
