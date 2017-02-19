using Heroes.ReplayParser;
using HeroesIcons;
using HeroesParserData.Models.DbModels;
using System;
using System.Collections.Generic;
using System.Linq;
using static Heroes.ReplayParser.DataParser;

namespace HeroesParserData.DataQueries
{
    public static class SaveAllReplayData
    {
        public static ReplayParseResult SaveAllData(Heroes.ReplayParser.Replay replay, string fileName, HeroesInfo heroesInfo, out DateTime replayTimeStamp, out long replayId)
        {
            using (HeroesParserDataContext db = new HeroesParserDataContext())
            {
                using (var dbTransaction = db.Database.BeginTransaction())
                {
                    try
                    {
                        #region save basic data
                        string mapName;
                        if (!heroesInfo.MapNameTranslation(replay.Map, out mapName))
                            throw new Exception($"Unknown map name detected: {replay.Map}");

                        Models.DbModels.Replay replayData = new Models.DbModels.Replay
                        {
                            Frames = replay.Frames,
                            GameMode = replay.GameMode,
                            GameSpeed = replay.GameSpeed.ToString(),
                            IsGameEventsParsed = replay.IsGameEventsParsedSuccessfully,
                            MapName = mapName,
                            RandomValue = replay.RandomValue,
                            ReplayBuild = replay.ReplayBuild,
                            ReplayLength = replay.ReplayLength,
                            ReplayVersion = replay.ReplayVersion,
                            TeamSize = replay.TeamSize,
                            TimeStamp = replay.Timestamp,
                            FileName = fileName
                        };

                        // check if replay was added to database already
                        if (Query.Replay.IsExistingReplay(replayData, db))
                        {
                            replayTimeStamp = replayData.TimeStamp.Value;
                            replayId = Query.Replay.ReadReplayIdByRandomValue(replayData);

                            return ReplayParseResult.Duplicate;
                        }

                        replayId = Query.Replay.CreateRecord(db, replayData);
                        #endregion save basic data

                        #region save player related data
                        int playerNum = 0;

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
                                BattleNetTId = player.BattleNetTId,
                                LastSeen = replay.Timestamp,
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

                                #region save match players
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
                                    PartyValue = player.PartyValue,
                                    PlayerNumber = -1,
                                    SkinAndSkinTint = player.SkinAndSkinTint,
                                    Team = player.Team
                                };

                                Query.MatchPlayer.CreateRecord(db, replayPlayer);
                                #endregion save match players

                                #region save players heroes
                                var playersHeroes = player.SkinsDictionary;

                                ReplayAllHotsPlayerHero playersHero = new ReplayAllHotsPlayerHero();
                                foreach (var hero in playersHeroes)
                                {
                                    if (heroesInfo.HeroExists(hero.Key, false))
                                    {
                                        playersHero.PlayerId = playerId;
                                        playersHero.HeroName = hero.Key;
                                        playersHero.IsUsable = hero.Value;
                                        playersHero.LastUpdated = replay.Timestamp;

                                        if (Query.HotsPlayerHero.HeroRecordExists(db, playersHero))
                                            Query.HotsPlayerHero.UpdateRecord(db, playersHero);
                                        else
                                            Query.HotsPlayerHero.CreateRecord(db, playersHero);
                                    }
                                }
                                #endregion save players heroes
                            }
                            else
                            {
                                #region save match players
                                string characterName;
                                if (!heroesInfo.HeroNameTranslation(player.Character, out characterName))
                                    throw new Exception($"Unknown hero name detected: {player.Character}");

                                ReplayMatchPlayer replayPlayer = new ReplayMatchPlayer
                                {
                                    ReplayId = replayId,
                                    PlayerId = playerId,
                                    Character = characterName,
                                    CharacterLevel = player.CharacterLevel,
                                    Difficulty = player.Difficulty.ToString(),
                                    Handicap = player.Handicap,
                                    IsAutoSelect = player.IsAutoSelect,
                                    IsSilenced = player.IsSilenced,
                                    IsWinner = player.IsWinner,
                                    MountAndMountTint = player.MountAndMountTint,
                                    PartyValue = player.PartyValue,
                                    PlayerNumber = playerNum,
                                    SkinAndSkinTint = player.SkinAndSkinTint,
                                    Team = player.Team
                                };

                                Query.MatchPlayer.CreateRecord(db, replayPlayer);
                                #endregion save match players
                                #region save match players score results
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
                                #endregion save match players score results

                                #region save match players talents
                                Talent[] talents = player.Talents;
                                Talent[] talentArray = new Talent[7]; // hold all 7 talents

                                // add known talents
                                for (int j = 0; j < talents.Count(); j++)
                                {
                                    talentArray[j] = new Talent();
                                    talentArray[j].TalentID = talents[j].TalentID;
                                    talentArray[j].TalentName = talents[j].TalentName;
                                    talentArray[j].TimeSpanSelected = talents[j].TimeSpanSelected;
                                }
                                // make the rest null
                                for (int j = talents.Count(); j < 7; j++)
                                {
                                    talentArray[j] = new Talent();
                                    talentArray[j].TalentID = null;
                                    talentArray[j].TalentName = null;
                                    talentArray[j].TimeSpanSelected = null;
                                }

                                ReplayMatchPlayerTalent replayTalent = new ReplayMatchPlayerTalent
                                {
                                    ReplayId = replayId,
                                    PlayerId = playerId,
                                    Character = characterName,
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
                                #endregion save match players talents

                                #region save players heroes
                                var playersHeroes = player.SkinsDictionary;

                                ReplayAllHotsPlayerHero playersHero = new ReplayAllHotsPlayerHero();
                                foreach (var hero in playersHeroes)
                                {
                                    if (heroesInfo.HeroExists(hero.Key, false))
                                    {
                                        playersHero.PlayerId = playerId;
                                        playersHero.HeroName = hero.Key;
                                        playersHero.IsUsable = hero.Value;
                                        playersHero.LastUpdated = replay.Timestamp;

                                        if (Query.HotsPlayerHero.HeroRecordExists(db, playersHero))
                                            Query.HotsPlayerHero.UpdateRecord(db, playersHero);
                                        else
                                            Query.HotsPlayerHero.CreateRecord(db, playersHero);
                                    }
                                }
                                #endregion save players heroes

                                #region save match awards
                                List<MatchAwardType> playerAwards = player.ScoreResult.MatchAwards;

                                foreach (var award in playerAwards)
                                {
                                    ReplayMatchAward matchAward = new ReplayMatchAward
                                    {
                                        ReplayId = replayId,
                                        PlayerId = playerId,
                                        Award = award.ToString()
                                    };
                                    Query.MatchAward.CreateRecord(db, matchAward);
                                }
                                #endregion save match awards

                                playerNum++;
                            }
                        } // end foreach loop for players
                        #endregion save player related data

                        #region save match team bans
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
                        #endregion save match team bans

                        #region save match teams levels
                        Dictionary<int, TimeSpan?> team0 = replay.TeamLevels[0];
                        Dictionary<int, TimeSpan?> team1 = replay.TeamLevels[1];

                        if (team0 != null || team1 != null)
                        {
                            int levelDiff = team0.Count - team1.Count;
                            if (levelDiff > 0)
                            {
                                for (int j = team1.Count + 1; j <= team0.Count; j++)
                                {
                                    team1.Add(j, null);
                                }
                            }
                            else if (levelDiff < 0)
                            {
                                for (int j = team0.Count + 1; j <= team1.Count; j++)
                                {
                                    team0.Add(j, null);
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
                        #endregion save match team levels

                        #region save match team experience
                        var xpTeam0 = replay.TeamPeriodicXPBreakdown[0];
                        var xpTeam1 = replay.TeamPeriodicXPBreakdown[1];

                        if (xpTeam0 != null && xpTeam1 != null)
                        {
                            if (xpTeam0.Count != xpTeam1.Count)
                            {
                                throw new Exception("Teams don't have equal periodic xp gain.");
                            }

                            for (int j = 0; j < xpTeam0.Count; j++)
                            {
                                var x = xpTeam0[j];
                                var y = xpTeam1[j];

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
                        #endregion save match team experience

                        #region save match message
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
                                    CharacterName = player != null ? player.Character : string.Empty,
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
                        #endregion save match messag

                        #region save match objectives
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
                                    PlayerId = player != null ? Query.HotsPlayer.ReadPlayerIdFromBattleNetId(db, Utilities.GetBattleTagName(player.Name, player.BattleTag), player.BattleNetId) : (long?)null,
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
                        #endregion save match objectives

                        dbTransaction.Commit();

                        replayTimeStamp = replay.Timestamp;

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
    }
}