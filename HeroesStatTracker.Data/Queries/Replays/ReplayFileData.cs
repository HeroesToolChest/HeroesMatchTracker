using Heroes.Helpers;
using Heroes.Icons;
using Heroes.ReplayParser;
using HeroesStatTracker.Data.Databases;
using HeroesStatTracker.Data.Models.Replays;
using System;
using System.Collections.Generic;
using System.Linq;

namespace HeroesStatTracker.Data.Queries.Replays
{
    public class ReplayFileData : IDisposable
    {
        private ReplaysContext ReplaysContext;
        private Replay Replay;
        private HeroesIcons HeroesIcons;
        private bool DisposedValue = false;

        public ReplayFileData(Replay replay, HeroesIcons heroesIcons = null)
        {
            ReplaysContext = new ReplaysContext();
            Replay = replay;
            HeroesIcons = heroesIcons;
        }

        public long ReplayId { get; private set; }
        public DateTime ReplayTimeStamp { get; private set; }

        public ReplayResult SaveAllData(string fileName)
        {
            using (ReplaysContext)
            {
                using (var dbTransaction = ReplaysContext.Database.BeginTransaction())
                {
                    try
                    {
                        if (BasicData(fileName))
                            return ReplayResult.Duplicate;

                        PlayerRelatedData();
                        MatchTeamBans();
                        MatchTeamLevels();
                        MatchTeamExperience();
                        MatchMessages();
                        MatchObjectives();

                        dbTransaction.Commit();

                        return ReplayResult.Saved;
                    }
                    catch (Exception)
                    {
                        dbTransaction.Rollback();
                        throw;
                    }
                }
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!DisposedValue)
            {
                if (disposing)
                {
                    ((IDisposable)ReplaysContext).Dispose();
                }

                DisposedValue = true;
            }
        }

        // returns true if replay already exists in database
        private bool BasicData(string fileName)
        {
            string mapName;
            if (!HeroesIcons.MapNameTranslation(Replay.Map, out mapName))
                throw new TranslationException(RetrieveAllMapAndHeroNames());

            ReplayMatch replayMatch = new ReplayMatch
            {
                Frames = Replay.Frames,
                GameMode = Replay.GameMode,
                GameSpeed = Replay.GameSpeed.ToString(),
                IsGameEventsParsed = Replay.IsGameEventsParsedSuccessfully,
                MapName = mapName,
                RandomValue = Replay.RandomValue,
                ReplayBuild = Replay.ReplayBuild,
                ReplayLength = Replay.ReplayLength,
                ReplayVersion = Replay.ReplayVersion,
                TeamSize = Replay.TeamSize,
                TimeStamp = Replay.Timestamp,
                FileName = fileName,
            };

            ReplayTimeStamp = replayMatch.TimeStamp.Value;

            // check if replay was added to database already
            if (QueryDb.ReplaysDb.MatchReplay.IsExistingRecord(ReplaysContext, replayMatch))
            {
                ReplayId = QueryDb.ReplaysDb.MatchReplay.ReadReplayIdByRandomValue(replayMatch);
                return true;
            }
            else
            {
                ReplayId = QueryDb.ReplaysDb.MatchReplay.CreateRecord(ReplaysContext, replayMatch);
                return false;
            }
        }

        private void PlayerRelatedData()
        {
            int playerNum = 0;

            Player[] players = GetPlayers();

            foreach (var player in players)
            {
                if (player == null)
                    continue;

                ReplayAllHotsPlayer hotsPlayer = new ReplayAllHotsPlayer
                {
                    BattleTagName = HeroesHelpers.BattleTags.GetBattleTagName(player.Name, player.BattleTag),
                    BattleNetId = player.BattleNetId,
                    BattleNetRegionId = player.BattleNetRegionId,
                    BattleNetSubId = player.BattleNetSubId,
                    BattleNetTId = player.BattleNetTId,
                    LastSeen = Replay.Timestamp,
                    Seen = 1,
                };

                long playerId;

                // check if player is already in the database, update if found, otherwise add a new record
                if (QueryDb.ReplaysDb.HotsPlayer.IsExistingRecord(ReplaysContext, hotsPlayer))
                    playerId = QueryDb.ReplaysDb.HotsPlayer.UpdateRecord(ReplaysContext, hotsPlayer);
                else
                    playerId = QueryDb.ReplaysDb.HotsPlayer.CreateRecord(ReplaysContext, hotsPlayer);

                if (player.Character == null && Replay.GameMode == GameMode.Custom)
                {
                    player.Team = 4;
                    player.Character = "None";

                    ReplayMatchPlayer replayPlayer = new ReplayMatchPlayer
                    {
                        ReplayId = ReplayId,
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
                        Team = player.Team,
                    };

                    QueryDb.ReplaysDb.MatchPlayer.CreateRecord(ReplaysContext, replayPlayer);
                }
                else
                {
                    string character;
                    if (!HeroesIcons.HeroNameTranslation(player.Character, out character))
                    {
                        if (!AttemptAutoTranslateHeroNameByTalent(player.Talents, out character))
                            throw new TranslationException(RetrieveAllMapAndHeroNames());
                    }

                    ReplayMatchPlayer replayPlayer = new ReplayMatchPlayer
                    {
                        ReplayId = ReplayId,
                        PlayerId = playerId,
                        Character = character,
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
                        Team = player.Team,
                    };

                    QueryDb.ReplaysDb.MatchPlayer.CreateRecord(ReplaysContext, replayPlayer);

                    AddScoreResults(player.ScoreResult, playerId);
                    AddPlayerTalents(player.Talents, playerId, character);
                    AddMatchAwards(player.ScoreResult.MatchAwards, playerId);

                    playerNum++;
                }

                AddPlayerHeroes(player.SkinsDictionary, playerId);
            } // end foreach loop for players
        }

        private void AddScoreResults(ScoreResult sr, long playerId)
        {
            ReplayMatchPlayerScoreResult playerScore = new ReplayMatchPlayerScoreResult
            {
                ReplayId = ReplayId,
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
                WatchTowerCaptures = sr.WatchTowerCaptures,
            };

            QueryDb.ReplaysDb.MatchPlayerScoreResult.CreateRecord(ReplaysContext, playerScore);
        }

        private void AddPlayerTalents(Talent[] talents, long playerId, string playerCharacter)
        {
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
                ReplayId = ReplayId,
                PlayerId = playerId,
                Character = playerCharacter,
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

            QueryDb.ReplaysDb.MatchPlayerTalent.CreateRecord(ReplaysContext, replayTalent);
        }

        private void AddMatchAwards(List<MatchAwardType> playerAwards, long playerId)
        {
            foreach (var award in playerAwards)
            {
                ReplayMatchAward matchAward = new ReplayMatchAward
                {
                    ReplayId = ReplayId,
                    PlayerId = playerId,
                    Award = award.ToString(),
                };
                QueryDb.ReplaysDb.MatchAward.CreateRecord(ReplaysContext, matchAward);
            }
        }

        private void AddPlayerHeroes(Dictionary<string, bool> playersHeroes, long playerId)
        {
            if (HeroesIcons != null)
            {
                ReplayAllHotsPlayerHero playersHero = new ReplayAllHotsPlayerHero();
                foreach (var hero in playersHeroes)
                {
                    if (HeroesIcons.HeroExists(hero.Key, false))
                    {
                        playersHero.PlayerId = playerId;
                        playersHero.HeroName = hero.Key;
                        playersHero.IsUsable = hero.Value;
                        playersHero.LastUpdated = Replay.Timestamp;

                        if (QueryDb.ReplaysDb.HotsPlayerHero.IsExistingRecord(ReplaysContext, playersHero))
                            QueryDb.ReplaysDb.HotsPlayerHero.UpdateRecord(ReplaysContext, playersHero);
                        else
                            QueryDb.ReplaysDb.HotsPlayerHero.CreateRecord(ReplaysContext, playersHero);
                    }
                }
            }
        }

        private void MatchTeamBans()
        {
            if (Replay.GameMode == GameMode.UnrankedDraft || Replay.GameMode == GameMode.HeroLeague || Replay.GameMode == GameMode.TeamLeague || Replay.GameMode == GameMode.Custom)
            {
                if (Replay.TeamHeroBans != null)
                {
                    ReplayMatchTeamBan replayTeamBan = new ReplayMatchTeamBan
                    {
                        ReplayId = ReplayId,
                        Team0Ban0 = Replay.TeamHeroBans[0][0],
                        Team0Ban1 = Replay.TeamHeroBans[0][1],
                        Team1Ban0 = Replay.TeamHeroBans[1][0],
                        Team1Ban1 = Replay.TeamHeroBans[1][1],
                    };

                    if (replayTeamBan.Team0Ban0 != null || replayTeamBan.Team0Ban1 != null || replayTeamBan.Team1Ban0 != null || replayTeamBan.Team1Ban1 != null)
                        QueryDb.ReplaysDb.MatchTeamBan.CreateRecord(ReplaysContext, replayTeamBan);
                }
            }
        }

        private void MatchTeamLevels()
        {
            Dictionary<int, TimeSpan?> team0 = Replay.TeamLevels[0];
            Dictionary<int, TimeSpan?> team1 = Replay.TeamLevels[1];

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
                        ReplayId = ReplayId,
                        TeamTime0 = team0[level],
                        Team0Level = team0[level].HasValue ? level : (int?)null,

                        TeamTime1 = team1[level],
                        Team1Level = team1[level].HasValue ? level : (int?)null,
                    };

                    QueryDb.ReplaysDb.MatchTeamLevel.CreateRecord(ReplaysContext, replayTeamLevel);
                }
            }
        }

        private void MatchTeamExperience()
        {
            var xpTeam0 = Replay.TeamPeriodicXPBreakdown[0];
            var xpTeam1 = Replay.TeamPeriodicXPBreakdown[1];

            if (xpTeam0 != null && xpTeam1 != null)
            {
                if (xpTeam0.Count != xpTeam1.Count)
                {
                    throw new QueryException("Teams don't have equal periodic xp gain.");
                }

                for (int j = 0; j < xpTeam0.Count; j++)
                {
                    var x = xpTeam0[j];
                    var y = xpTeam1[j];

                    ReplayMatchTeamExperience xp = new ReplayMatchTeamExperience
                    {
                        ReplayId = ReplayId,
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

                    QueryDb.ReplaysDb.MatchTeamExperience.CreateRecord(ReplaysContext, xp);
                }
            }
        }

        private void MatchMessages()
        {
            foreach (var message in Replay.Messages)
            {
                var messageEventType = message.MessageEventType;
                var player = message.MessageSender;

                if (messageEventType == ReplayMessageEvents.MessageEventType.SChatMessage)
                {
                    var chatMessage = message.ChatMessage;

                    ReplayMatchMessage chat = new ReplayMatchMessage
                    {
                        ReplayId = ReplayId,
                        CharacterName = player != null ? player.Character : string.Empty,
                        Message = chatMessage.Message,
                        MessageEventType = messageEventType.ToString(),
                        MessageTarget = chatMessage.MessageTarget.ToString(),
                        PlayerName = player != null ? player.Name : string.Empty,
                        TimeStamp = message.Timestamp,
                    };

                    QueryDb.ReplaysDb.MatchMessage.CreateRecord(ReplaysContext, chat);
                }
                else if (messageEventType == ReplayMessageEvents.MessageEventType.SPingMessage)
                {
                    var pingMessage = message.PingMessage;

                    ReplayMatchMessage ping = new ReplayMatchMessage
                    {
                        ReplayId = ReplayId,
                        CharacterName = player != null ? player.Character : string.Empty,
                        Message = "used a ping",
                        MessageEventType = messageEventType.ToString(),
                        MessageTarget = pingMessage.MessageTarget.ToString(),
                        PlayerName = player != null ? player.Name : string.Empty,
                        TimeStamp = message.Timestamp,
                    };

                    QueryDb.ReplaysDb.MatchMessage.CreateRecord(ReplaysContext, ping);
                }
                else if (messageEventType == ReplayMessageEvents.MessageEventType.SPlayerAnnounceMessage)
                {
                    var announceMessage = message.PlayerAnnounceMessage;

                    ReplayMatchMessage announce = new ReplayMatchMessage
                    {
                        ReplayId = ReplayId,
                        CharacterName = player != null ? player.Character : string.Empty,
                        Message = $"announce {announceMessage.AnnouncementType.ToString()}",
                        MessageEventType = messageEventType.ToString(),
                        MessageTarget = "Allies",
                        PlayerName = player != null ? player.Name : string.Empty,
                        TimeStamp = message.Timestamp,
                    };

                    QueryDb.ReplaysDb.MatchMessage.CreateRecord(ReplaysContext, announce);
                }
            }
        }

        private void MatchObjectives()
        {
            var objTeam0 = Replay.TeamObjectives[0];
            var objTeam1 = Replay.TeamObjectives[1];

            if (objTeam0.Count > 0 && objTeam0 != null)
            {
                foreach (var objCount in objTeam0)
                {
                    var player = objCount.Player;

                    ReplayMatchTeamObjective obj = new ReplayMatchTeamObjective
                    {
                        Team = 0,
                        PlayerId = player != null ? QueryDb.ReplaysDb.HotsPlayer.ReadPlayerIdFromBattleNetId(ReplaysContext, HeroesHelpers.BattleTags.GetBattleTagName(player.Name, player.BattleTag), player.BattleNetId) : (long?)null,
                        ReplayId = ReplayId,
                        TeamObjectiveType = objCount.TeamObjectiveType.ToString(),
                        TimeStamp = objCount.TimeSpan,
                        Value = objCount.Value,
                    };

                    QueryDb.ReplaysDb.MatchTeamObjective.CreateRecord(ReplaysContext, obj);
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
                        PlayerId = player != null ? QueryDb.ReplaysDb.HotsPlayer.ReadPlayerIdFromBattleNetId(ReplaysContext, HeroesHelpers.BattleTags.GetBattleTagName(player.Name, player.BattleTag), player.BattleNetId) : (long?)null,
                        ReplayId = ReplayId,
                        TeamObjectiveType = objCount.TeamObjectiveType.ToString(),
                        TimeStamp = objCount.TimeSpan,
                        Value = objCount.Value,
                    };

                    QueryDb.ReplaysDb.MatchTeamObjective.CreateRecord(ReplaysContext, obj);
                }
            }
        }

        private Player[] GetPlayers()
        {
            if (Replay.ReplayBuild > 39445)
                return Replay.ClientListByUserID;
            else
                return Replay.Players;
        }

        private string RetrieveAllMapAndHeroNames()
        {
            List<string> names = new List<string>();

            string mapName;
            if (HeroesIcons.MapNameTranslation(Replay.Map, out mapName))
                names.Add($"{Replay.Map}: {mapName} [Good]");
            else
                names.Add($"{Replay.Map}: ??? [Unknown]");

            foreach (var player in GetPlayers())
            {
                if (player == null)
                    continue;

                if (player.Character == null)
                {
                    names.Add($"No character");
                    continue;
                }

                string character;
                if (HeroesIcons.HeroNameTranslation(player.Character, out character))
                    names.Add($"{player.Character}: {character} [Good]");
                else if (AttemptAutoTranslateHeroNameByTalent(player.Talents, out character))
                    names.Add($"{player.Character}: {character} [Auto]");
                else
                    names.Add($"{player.Character}: ??? [Unknown]");
            }

            string output = "Unable to translate some or all of the following names";
            output += Environment.NewLine;

            foreach (var name in names)
            {
                output += name;
                output += Environment.NewLine;
            }

            return output;
        }

        private bool AttemptAutoTranslateHeroNameByTalent(Talent[] talents, out string character)
        {
            int talentCount = talents.Count();
            character = string.Empty;

            if (talentCount == 0)
            {
                return false;
            }
            else if (talentCount >= 4)
            {
                if (HeroesIcons.GetHeroNameFromTalentReferenceName(talents[3].TalentName, out character))
                    return true;
            }
            else if (talentCount <= 3)
            {
                if (HeroesIcons.GetHeroNameFromTalentReferenceName(talents[talentCount - 1].TalentName, out character))
                    return true;
            }

            return false;
        }
    }
}
