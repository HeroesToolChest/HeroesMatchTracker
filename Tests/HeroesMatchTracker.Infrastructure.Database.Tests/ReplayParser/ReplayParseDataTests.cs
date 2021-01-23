using Heroes.StormReplayParser;
using Heroes.StormReplayParser.Replay;
using HeroesMatchTracker.Core.Entities;
using HeroesMatchTracker.Core.Repositories;
using HeroesMatchTracker.Core.Repositories.HeroesReplays;
using HeroesMatchTracker.Infrastructure.Database.Contexts;
using HeroesMatchTracker.Infrastructure.Database.Repository.HeroesReplays;
using HeroesMatchTracker.Tests;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace HeroesMatchTracker.Infrastructure.ReplayParser.Tests
{
    [TestClass]
    public class ReplayParseDataTests
    {
        private readonly IReplayMatchRepository _replayMatchRepository;
        private readonly IReplayPlayerToonRepository _replayPlayerToonRepository;
        private readonly IReplayPlayerRepository _replayPlayerRepository;

        private readonly string _hanamuraTemple267679FileName = "HanamuraTemple2_67679.StormR";
        private readonly StormReplayResult _hanamura267679ReplayResult;

        private readonly string _testReplaysFolder = "TestReplays";

        public ReplayParseDataTests()
        {
            _replayMatchRepository = new ReplayMatchRepository();
            _replayPlayerToonRepository = new ReplayPlayerToonRepository();
            _replayPlayerRepository = new ReplayPlayerRepository();

            _hanamura267679ReplayResult = StormReplay.Parse(Path.Combine(_testReplaysFolder, _hanamuraTemple267679FileName), new ParseOptions()
            {
                ShouldParseGameEvents = true,
                ShouldParseMessageEvents = true,
                ShouldParseTrackerEvents = true,
            });
        }

        [TestMethod]
        public void AddReplay_AddingNewReplay_ReplayDataAdded()
        {
            using HeroesReplaysDbContext context = DbServices.GetHeroesReplaysDbContext();

            IReplayParseData replayParseData = new ReplayParseData(_replayMatchRepository, _replayPlayerToonRepository, _replayPlayerRepository);
            string replayHash = replayParseData.GetReplayHash(_hanamura267679ReplayResult.Replay);

            Assert.AreEqual(replayHash, "2db896de6a3a97dd10a5bbf68f57dcd146524de8");

            replayParseData.AddReplay(context, _hanamura267679ReplayResult.FileName, replayHash, _hanamura267679ReplayResult.Replay);

            ReplayMatch replayMatchDb = context.Replays.Where(x => x.Hash == replayHash).FirstOrDefault()!;

            Assert.AreEqual(1, context.Replays.Count());
            Assert.AreEqual(Path.Combine(_testReplaysFolder, _hanamuraTemple267679FileName), replayMatchDb.ReplayFilePath);
            Assert.AreEqual(StormGameMode.QuickMatch, replayMatchDb.GameMode);
            Assert.AreEqual("2.37.0.67679", replayMatchDb.ReplayVersion);
            Assert.AreEqual("Hanamura", replayMatchDb.MapId);
            Assert.AreEqual("Hanamura Temple", replayMatchDb.MapName);
            Assert.AreEqual(10, replayMatchDb.ReplayMatchPlayers!.Count);
            Assert.AreEqual(1, replayMatchDb.OwnerPlayerId);
            Assert.AreEqual("T:27620522#221", replayMatchDb.OwnerReplayPlayer!.ShortcutId);
        }

        [TestMethod]
        public void IsReplayExists_AddedNewReplay_ReplayExists()
        {
            using HeroesReplaysDbContext context = DbServices.GetHeroesReplaysDbContext();

            IReplayParseData replayParseData = new ReplayParseData(_replayMatchRepository, _replayPlayerToonRepository, _replayPlayerRepository);
            string replayHash = replayParseData.GetReplayHash(_hanamura267679ReplayResult.Replay);

            Assert.AreEqual(replayHash, "2db896de6a3a97dd10a5bbf68f57dcd146524de8");
            Assert.IsFalse(replayParseData.IsReplayExists(context, replayHash));

            replayParseData.AddReplay(context, _hanamura267679ReplayResult.FileName, replayHash, _hanamura267679ReplayResult.Replay);

            ReplayMatch replayMatchDb = context.Replays.Where(x => x.Hash == replayHash).FirstOrDefault()!;

            Assert.AreEqual(1, context.Replays.Count());
            Assert.IsTrue(replayParseData.IsReplayExists(context, replayHash));
        }

        [TestMethod]
        public void AddReplay_ExistingPlayer_ExistingPlayerDataUpdated()
        {
            string fileName1 = "HanamuraTemple1_75132.StormR";
            string fileName2 = "VolskayaFoundry1_77548.StormR";
            string fileName3 = "TowersofDoom1_72649.StormR";
            string file1 = Path.Combine("TestReplays", fileName1);
            string file2 = Path.Combine("TestReplays", fileName2);
            string file3 = Path.Combine("TestReplays", fileName3);

            using HeroesReplaysDbContext context = DbServices.GetHeroesReplaysDbContext();

            StormReplayResult replayResult1 = StormReplay.Parse(file1, new ParseOptions()
            {
                ShouldParseGameEvents = true,
                ShouldParseMessageEvents = true,
                ShouldParseTrackerEvents = true,
            });

            StormReplayResult replayResult2 = StormReplay.Parse(file2, new ParseOptions()
            {
                ShouldParseGameEvents = true,
                ShouldParseMessageEvents = true,
                ShouldParseTrackerEvents = true,
            });

            StormReplayResult replayResult3 = StormReplay.Parse(file3, new ParseOptions()
            {
                ShouldParseGameEvents = true,
                ShouldParseMessageEvents = true,
                ShouldParseTrackerEvents = true,
            });

            IReplayParseData replayParseData = new ReplayParseData(_replayMatchRepository, _replayPlayerToonRepository, _replayPlayerRepository);
            string replayHash1 = replayParseData.GetReplayHash(replayResult1.Replay);
            string replayHash2 = replayParseData.GetReplayHash(replayResult2.Replay);
            string replayHash3 = replayParseData.GetReplayHash(replayResult3.Replay);

            // add first replay
            replayParseData.AddReplay(context, replayResult1.FileName, replayHash1, replayResult1.Replay);

            ReplayPlayer player = context.ReplayPlayers.First(x => x.ShortcutId == "T:56372890#167");

            Assert.AreEqual(2319, player.AccountLevel);

            // add second replay
            replayParseData.AddReplay(context, replayResult2.FileName, replayHash2, replayResult2.Replay);

            ReplayPlayer updatedPlayer = context.ReplayPlayers.First(x => x.ShortcutId == "T:56372890#167");

            Assert.AreEqual(2423, updatedPlayer.AccountLevel);
            Assert.IsNull(updatedPlayer.ReplayOldPlayerInfos);

            // add third replay
            replayParseData.AddReplay(context, replayResult3.FileName, replayHash3, replayResult3.Replay);

            ReplayPlayer updatedPlayerNoAccountLevelChange = context.ReplayPlayers.First(x => x.ShortcutId == "T:56372890#167");

            Assert.AreEqual(2423, updatedPlayerNoAccountLevelChange.AccountLevel);
            Assert.IsNull(updatedPlayerNoAccountLevelChange.ReplayOldPlayerInfos);
        }

        [TestMethod]
        public void AddReplay_ReExistingPlayerWithNewBattleTag_PlayerBattleTagStaysSame()
        {
            string fileName1 = "Hanamura1_54339.StormR";
            string fileName2 = "Braxis Outpost1_82624.StormR";
            string file1 = Path.Combine("TestReplays", fileName1);
            string file2 = Path.Combine("TestReplays", fileName2);

            using HeroesReplaysDbContext context = DbServices.GetHeroesReplaysDbContext();

            StormReplayResult replayResult1 = StormReplay.Parse(file1, new ParseOptions()
            {
                ShouldParseGameEvents = true,
                ShouldParseMessageEvents = true,
                ShouldParseTrackerEvents = true,
            });

            StormReplayResult replayResult2 = StormReplay.Parse(file2, new ParseOptions()
            {
                ShouldParseGameEvents = true,
                ShouldParseMessageEvents = true,
                ShouldParseTrackerEvents = true,
            });

            IReplayParseData replayParseData = new ReplayParseData(_replayMatchRepository, _replayPlayerToonRepository, _replayPlayerRepository);
            string replayHash1 = replayParseData.GetReplayHash(replayResult1.Replay);
            string replayHash2 = replayParseData.GetReplayHash(replayResult2.Replay);

            // add first replay
            replayParseData.AddReplay(context, replayResult1.FileName, replayHash1, replayResult1.Replay);

            ReplayPlayer player = context.ReplayPlayers.First(x => x.ShortcutId == "T:60500215#653");

            Assert.IsTrue(player.BattleTagName!.StartsWith("GYF", StringComparison.Ordinal));

            // add second replay
            replayParseData.AddReplay(context, replayResult2.FileName, replayHash2, replayResult2.Replay);

            ReplayPlayer updatedPlayer = context.ReplayPlayers.First(x => x.ShortcutId == "T:60500215#653");

            Assert.IsTrue(updatedPlayer.BattleTagName!.StartsWith("VALuuuuu", StringComparison.Ordinal));
            Assert.IsTrue(updatedPlayer.ReplayOldPlayerInfos!.First().BattleTagName!.StartsWith("GYF", StringComparison.Ordinal));

            // add first replay again
            replayParseData.AddReplay(context, replayResult1.FileName, "asdfasdfasdf", replayResult1.Replay);

            ReplayPlayer updatedPlayerAgain = context.ReplayPlayers.First(x => x.ShortcutId == "T:60500215#653");

            Assert.IsTrue(updatedPlayerAgain.BattleTagName!.StartsWith("VALuuuuu", StringComparison.Ordinal)); // should not change since replay is older
            Assert.AreEqual(1, updatedPlayerAgain.ReplayOldPlayerInfos!.Count);

            var oldPlayerInfo = updatedPlayerAgain.ReplayOldPlayerInfos.ToList();
            Assert.IsTrue(oldPlayerInfo[0].BattleTagName!.StartsWith("GYF", StringComparison.Ordinal));
        }

        [TestMethod]
        public void AddReplay_ExistingPlayerWithNewBattleTag_ExistingPlayerBattleTagUpdated()
        {
            string fileName1 = "Hanamura1_54339.StormR";
            string fileName2 = "Braxis Outpost1_82624.StormR";
            string file1 = Path.Combine("TestReplays", fileName1);
            string file2 = Path.Combine("TestReplays", fileName2);

            using HeroesReplaysDbContext context = DbServices.GetHeroesReplaysDbContext();

            StormReplayResult replayResult1 = StormReplay.Parse(file1, new ParseOptions()
            {
                ShouldParseGameEvents = true,
                ShouldParseMessageEvents = true,
                ShouldParseTrackerEvents = true,
            });

            StormReplayResult replayResult2 = StormReplay.Parse(file2, new ParseOptions()
            {
                ShouldParseGameEvents = true,
                ShouldParseMessageEvents = true,
                ShouldParseTrackerEvents = true,
            });

            IReplayParseData replayParseData = new ReplayParseData(_replayMatchRepository, _replayPlayerToonRepository, _replayPlayerRepository);
            string replayHash1 = replayParseData.GetReplayHash(replayResult1.Replay);
            string replayHash2 = replayParseData.GetReplayHash(replayResult2.Replay);

            // add first replay
            replayParseData.AddReplay(context, replayResult1.FileName, replayHash1, replayResult1.Replay);

            ReplayPlayer player = context.ReplayPlayers.First(x => x.ShortcutId == "T:60500215#653");

            Assert.IsTrue(player.BattleTagName!.StartsWith("GYF", StringComparison.Ordinal));

            // add second fake battletag
            context.ReplayOldPlayerInfos.Add(new ReplayOldPlayerInfo()
            {
                BattleTagName = "somenewname#3333",
                DateAdded = new DateTime(2018, 1, 1),
                PlayerId = 0,
                ReplayPlayer = context.ReplayPlayers.First(x => x.ShortcutId == "T:60500215#653"),
            });

            context.SaveChanges();

            // add second replay (with new "third" battletag)
            replayParseData.AddReplay(context, replayResult2.FileName, replayHash2, replayResult2.Replay);

            ReplayPlayer updatedPlayer = context.ReplayPlayers.First(x => x.ShortcutId == "T:60500215#653");

            Assert.IsTrue(updatedPlayer.BattleTagName!.StartsWith("VALuuuuu", StringComparison.Ordinal));
            Assert.IsTrue(updatedPlayer.ReplayOldPlayerInfos!.First().BattleTagName!.StartsWith("somenewname", StringComparison.Ordinal));
            Assert.AreEqual(2, updatedPlayer.ReplayOldPlayerInfos!.Count);
        }

        [TestMethod]
        public void AddReplay_AddHeroLevel_HeroLevelAdded()
        {
            string fileName1 = "Hanamura1_54339.StormR";
            string file1 = Path.Combine("TestReplays", fileName1);

            using HeroesReplaysDbContext context = DbServices.GetHeroesReplaysDbContext();

            StormReplayResult replayResult1 = StormReplay.Parse(file1, new ParseOptions()
            {
                ShouldParseGameEvents = true,
                ShouldParseMessageEvents = true,
                ShouldParseTrackerEvents = true,
            });

            IReplayParseData replayParseData = new ReplayParseData(_replayMatchRepository, _replayPlayerToonRepository, _replayPlayerRepository);
            string replayHash1 = replayParseData.GetReplayHash(replayResult1.Replay);

            replayParseData.AddReplay(context, replayResult1.FileName, replayHash1, replayResult1.Replay);

            ReplayPlayer player = context.ReplayPlayers.First(x => x.ShortcutId == "T:56372890#167");

            Assert.AreEqual(25, player.ReplayMatchPlayers!.First().HeroLevel);
        }

        [TestMethod]
        public void AddReplay_AddPlayerMatchData_PlayerMatchDataAdded()
        {
            using HeroesReplaysDbContext context = DbServices.GetHeroesReplaysDbContext();

            IReplayParseData replayParseData = new ReplayParseData(_replayMatchRepository, _replayPlayerToonRepository, _replayPlayerRepository);
            string replayHash = replayParseData.GetReplayHash(_hanamura267679ReplayResult.Replay);

            replayParseData.AddReplay(context, _hanamura267679ReplayResult.FileName, replayHash, _hanamura267679ReplayResult.Replay);

            ReplayMatchPlayer playerMuradin = context.ReplayMatchPlayers.First(x => x.HeroId == "Muradin");
            ReplayMatchPlayer playerIllidan = context.ReplayMatchPlayers.First(x => x.HeroId == "Illidan");
            ReplayMatchPlayer playerMephisto = context.ReplayMatchPlayers.First(x => x.HeroId == "Mephisto" && x.Team == StormTeam.Red);
            ReplayMatchPlayer playerVarian = context.ReplayMatchPlayers.First(x => x.HeroId == "Varian");

            Assert.AreEqual(5, playerMuradin.PartySize);
            Assert.AreEqual(78924826228, playerMuradin.PartyValue);
            Assert.AreEqual(5, playerIllidan.PartySize);
            Assert.AreEqual(78924826228, playerIllidan.PartyValue);
            Assert.AreEqual(1, playerMephisto.PartySize);
            Assert.IsNull(playerMephisto.PartyValue);
            Assert.AreEqual(2, playerVarian.PartySize);
            Assert.AreEqual(8141408165, playerVarian.PartyValue);
        }

        [TestMethod]
        public void AddReplay_AddPlayerHeroTalents_TalentsAdded()
        {
            using HeroesReplaysDbContext context = DbServices.GetHeroesReplaysDbContext();

            IReplayParseData replayParseData = new ReplayParseData(_replayMatchRepository, _replayPlayerToonRepository, _replayPlayerRepository);
            string replayHash = replayParseData.GetReplayHash(_hanamura267679ReplayResult.Replay);

            replayParseData.AddReplay(context, _hanamura267679ReplayResult.FileName, replayHash, _hanamura267679ReplayResult.Replay);

            ReplayMatchPlayer player = context.ReplayMatchPlayers.First(x => x.HeroId == "Muradin");

            Assert.AreEqual("MuradinStormboltPerfectStorm", player.ReplayMatchPlayerTalent!.TalentId1);
            Assert.AreEqual("MuradinMasteryThunderburn", player.ReplayMatchPlayerTalent!.TalentId4);
            Assert.AreEqual("MuradinCombatStyleSkullcracker", player.ReplayMatchPlayerTalent!.TalentId7);
            Assert.AreEqual("MuradinHeroicAbilityAvatar", player.ReplayMatchPlayerTalent!.TalentId10);
            Assert.AreEqual("MuradinBronzebeardRage", player.ReplayMatchPlayerTalent!.TalentId13);
            Assert.AreEqual("MuradinMasteryPassiveStoneform", player.ReplayMatchPlayerTalent!.TalentId16);
            Assert.AreEqual("MuradinMasteryAvatarUnstoppableForce", player.ReplayMatchPlayerTalent!.TalentId20);

            Assert.AreEqual(342500000, player.ReplayMatchPlayerTalent.TimeSpanSelected1!.Value.Ticks);
            Assert.AreEqual(342500000, player.ReplayMatchPlayerTalent!.TimeSpanSelected1Ticks);
            Assert.AreEqual(1799375000, player.ReplayMatchPlayerTalent.TimeSpanSelected4!.Value.Ticks);
            Assert.AreEqual(1799375000, player.ReplayMatchPlayerTalent!.TimeSpanSelected4Ticks);
            Assert.AreEqual(3245625000, player.ReplayMatchPlayerTalent.TimeSpanSelected7!.Value.Ticks);
            Assert.AreEqual(3245625000, player.ReplayMatchPlayerTalent!.TimeSpanSelected7Ticks);
            Assert.AreEqual(4631250000, player.ReplayMatchPlayerTalent.TimeSpanSelected10!.Value.Ticks);
            Assert.AreEqual(4631250000, player.ReplayMatchPlayerTalent!.TimeSpanSelected10Ticks);
            Assert.AreEqual(6568125000, player.ReplayMatchPlayerTalent.TimeSpanSelected13!.Value.Ticks);
            Assert.AreEqual(6568125000, player.ReplayMatchPlayerTalent!.TimeSpanSelected13Ticks);
            Assert.AreEqual(8472500000, player.ReplayMatchPlayerTalent.TimeSpanSelected16!.Value.Ticks);
            Assert.AreEqual(8472500000, player.ReplayMatchPlayerTalent!.TimeSpanSelected16Ticks);
            Assert.AreEqual(10498125000, player.ReplayMatchPlayerTalent.TimeSpanSelected20!.Value.Ticks);
            Assert.AreEqual(10498125000, player.ReplayMatchPlayerTalent!.TimeSpanSelected20Ticks);
        }

        [TestMethod]
        public void AddReplay_AddPlayerLoadout_LoadoutAdded()
        {
            using HeroesReplaysDbContext context = DbServices.GetHeroesReplaysDbContext();

            IReplayParseData replayParseData = new ReplayParseData(_replayMatchRepository, _replayPlayerToonRepository, _replayPlayerRepository);
            string replayHash = replayParseData.GetReplayHash(_hanamura267679ReplayResult.Replay);

            replayParseData.AddReplay(context, _hanamura267679ReplayResult.FileName, replayHash, _hanamura267679ReplayResult.Replay);

            ReplayMatchPlayer player = context.ReplayMatchPlayers.First(x => x.HeroId == "Muradin");

            Assert.AreEqual("NoAP", player.ReplayMatchPlayerLoadout!.AnnouncerPackAttributeId);
            Assert.AreEqual("NoAnnouncerPack", player.ReplayMatchPlayerLoadout.AnnouncerPackId);
            Assert.AreEqual("BN17", player.ReplayMatchPlayerLoadout.BannerAttributeId);
            Assert.AreEqual("BannerDefault", player.ReplayMatchPlayerLoadout.BannerId);
            Assert.AreEqual("Hhh1", player.ReplayMatchPlayerLoadout.MountAndMountTintAttributeId);
            Assert.AreEqual("HeadlessHorsemanHorse", player.ReplayMatchPlayerLoadout.MountAndMountTintId);
            Assert.AreEqual("Mur3", player.ReplayMatchPlayerLoadout.SkinAndSkinTintAttributeId);
            Assert.AreEqual("MuradinMagni", player.ReplayMatchPlayerLoadout.SkinAndSkinTintId);
            Assert.AreEqual("SYDF", player.ReplayMatchPlayerLoadout.SprayAttributeId);
            Assert.AreEqual("SprayStaticFluidDefault", player.ReplayMatchPlayerLoadout.SprayId);
            Assert.AreEqual("MR01", player.ReplayMatchPlayerLoadout.VoiceLineAttributeId);
            Assert.AreEqual("MuradinBase_VoiceLine01", player.ReplayMatchPlayerLoadout.VoiceLineId);
        }

        [TestMethod]
        public void AddReplay_AddPlayerMatchAward_MatchAwardsAdded()
        {
            using HeroesReplaysDbContext context = DbServices.GetHeroesReplaysDbContext();

            IReplayParseData replayParseData = new ReplayParseData(_replayMatchRepository, _replayPlayerToonRepository, _replayPlayerRepository);
            string replayHash = replayParseData.GetReplayHash(_hanamura267679ReplayResult.Replay);

            replayParseData.AddReplay(context, _hanamura267679ReplayResult.FileName, replayHash, _hanamura267679ReplayResult.Replay);

            ReplayMatchPlayer player = context.ReplayMatchPlayers.First(x => x.HeroId == "Muradin");

            Assert.AreEqual("MostTeamfightDamageTaken", player.ReplayMatchAward!.ToList()[0].AwardId);
        }

        [TestMethod]
        public void AddReplayTeamBansTest()
        {
            using HeroesReplaysDbContext context = DbServices.GetHeroesReplaysDbContext();

            IReplayParseData replayParseData = new ReplayParseData(_replayMatchRepository, _replayPlayerToonRepository, _replayPlayerRepository);
            string replayHash = replayParseData.GetReplayHash(_hanamura267679ReplayResult.Replay);

            replayParseData.AddReplay(context, _hanamura267679ReplayResult.FileName, replayHash, _hanamura267679ReplayResult.Replay);

            List<ReplayMatchTeamBan> replayMatchTeamBans = context.ReplayMatchTeamBans.Where(x => x.Replay.Hash == replayHash).ToList();

            Assert.AreEqual(string.Empty, replayMatchTeamBans[0].TeamBan);
            Assert.AreEqual(string.Empty, replayMatchTeamBans[1].TeamBan);
            Assert.AreEqual(string.Empty, replayMatchTeamBans[2].TeamBan);
            Assert.AreEqual(string.Empty, replayMatchTeamBans[3].TeamBan);
            Assert.AreEqual(string.Empty, replayMatchTeamBans[4].TeamBan);
            Assert.AreEqual(string.Empty, replayMatchTeamBans[5].TeamBan);

            var volskayaFoundryParseResult = StormReplay.Parse(Path.Combine(_testReplaysFolder, "VolskayaFoundry1_77548.StormR"), new ParseOptions()
            {
                ShouldParseGameEvents = true,
                ShouldParseMessageEvents = true,
                ShouldParseTrackerEvents = true,
            });

            IReplayParseData volskayaFoundryData = new ReplayParseData(_replayMatchRepository, _replayPlayerToonRepository, _replayPlayerRepository);
            string volskayaFoundryReplayHash = replayParseData.GetReplayHash(volskayaFoundryParseResult.Replay);

            replayParseData.AddReplay(context, volskayaFoundryParseResult.FileName, volskayaFoundryReplayHash, volskayaFoundryParseResult.Replay);

            List<ReplayMatchTeamBan> blueReplayMatchTeamBansWithBans = context.ReplayMatchTeamBans.Where(x => x.Replay.Hash == volskayaFoundryReplayHash && x.Team == StormTeam.Blue).OrderBy(x => x.Order).ToList();
            List<ReplayMatchTeamBan> redReplayMatchTeamBansWithBans = context.ReplayMatchTeamBans.Where(x => x.Replay.Hash == volskayaFoundryReplayHash && x.Team == StormTeam.Red).OrderBy(x => x.Order).ToList();

            Assert.AreEqual("Alar", blueReplayMatchTeamBansWithBans[0].TeamBan);
            Assert.AreEqual(StormTeam.Blue, blueReplayMatchTeamBansWithBans[0].Team);
            Assert.AreEqual(0, blueReplayMatchTeamBansWithBans[0].Order);
            Assert.AreEqual("Garr", blueReplayMatchTeamBansWithBans[1].TeamBan);
            Assert.AreEqual("Fire", blueReplayMatchTeamBansWithBans[2].TeamBan);
            Assert.AreEqual(StormTeam.Blue, blueReplayMatchTeamBansWithBans[2].Team);
            Assert.AreEqual(2, blueReplayMatchTeamBansWithBans[2].Order);

            Assert.AreEqual("Crus", redReplayMatchTeamBansWithBans[0].TeamBan);
            Assert.AreEqual("DEAT", redReplayMatchTeamBansWithBans[1].TeamBan);
            Assert.AreEqual("Malf", redReplayMatchTeamBansWithBans[2].TeamBan);
            Assert.AreEqual(StormTeam.Red, redReplayMatchTeamBansWithBans[2].Team);
            Assert.AreEqual(2, redReplayMatchTeamBansWithBans[2].Order);
        }

        [TestMethod]
        public void AddReplay_AddTeamDraftPicks_TeamDraftPicksAdded()
        {
            using HeroesReplaysDbContext context = DbServices.GetHeroesReplaysDbContext();

            IReplayParseData replayParseData = new ReplayParseData(_replayMatchRepository, _replayPlayerToonRepository, _replayPlayerRepository);
            string replayHash = replayParseData.GetReplayHash(_hanamura267679ReplayResult.Replay);

            replayParseData.AddReplay(context, _hanamura267679ReplayResult.FileName, replayHash, _hanamura267679ReplayResult.Replay);

            List<ReplayMatchDraftPick> replayDraftPicks = context.ReplayMatchDraftPicks.Where(x => x.Replay.Hash == replayHash).ToList();

            Assert.AreEqual(0, replayDraftPicks.Count);

            var volskayaFoundryParseResult = StormReplay.Parse(Path.Combine(_testReplaysFolder, "VolskayaFoundry1_77548.StormR"), new ParseOptions()
            {
                ShouldParseGameEvents = true,
                ShouldParseMessageEvents = true,
                ShouldParseTrackerEvents = true,
            });

            IReplayParseData volskayaFoundryReplayData = new ReplayParseData(_replayMatchRepository, _replayPlayerToonRepository, _replayPlayerRepository);
            string volskayaFoundryReplayHash = replayParseData.GetReplayHash(volskayaFoundryParseResult.Replay);

            replayParseData.AddReplay(context, volskayaFoundryParseResult.FileName, volskayaFoundryReplayHash, volskayaFoundryParseResult.Replay);

            List<ReplayMatchDraftPick> replayDraftPicks2 = context.ReplayMatchDraftPicks.Where(x => x.Replay.Hash == volskayaFoundryReplayHash).OrderBy(x => x.Order).ToList();

            Assert.AreEqual("Crusader", replayDraftPicks2[0].HeroSelected);
            Assert.AreEqual(0, replayDraftPicks2[0].Order);
            Assert.AreEqual(StormDraftPickType.Banned, replayDraftPicks2[0].PickType);
            Assert.AreEqual(StormTeam.Red, replayDraftPicks2[0].Team);

            Assert.AreEqual("Garrosh", replayDraftPicks2[3].HeroSelected);
            Assert.AreEqual(3, replayDraftPicks2[3].Order);
            Assert.AreEqual(StormDraftPickType.Banned, replayDraftPicks2[3].PickType);
            Assert.AreEqual(StormTeam.Blue, replayDraftPicks2[3].Team);

            Assert.AreEqual("Rehgar", replayDraftPicks2[8].HeroSelected);
            Assert.AreEqual(8, replayDraftPicks2[8].Order);
            Assert.AreEqual(StormDraftPickType.Picked, replayDraftPicks2[8].PickType);
            Assert.AreEqual(StormTeam.Red, replayDraftPicks2[8].Team);

            Assert.AreEqual("Auriel", replayDraftPicks2[15].HeroSelected);
            Assert.AreEqual(15, replayDraftPicks2[15].Order);
            Assert.AreEqual(StormDraftPickType.Picked, replayDraftPicks2[15].PickType);
            Assert.AreEqual(StormTeam.Blue, replayDraftPicks2[15].Team);
        }

        [TestMethod]
        public void AddReplay_AddTeamLevels_TeamLevelsAdded()
        {
            using HeroesReplaysDbContext context = DbServices.GetHeroesReplaysDbContext();

            IReplayParseData replayParseData = new ReplayParseData(_replayMatchRepository, _replayPlayerToonRepository, _replayPlayerRepository);
            string replayHash = replayParseData.GetReplayHash(_hanamura267679ReplayResult.Replay);

            replayParseData.AddReplay(context, _hanamura267679ReplayResult.FileName, replayHash, _hanamura267679ReplayResult.Replay);

            List<ReplayMatchTeamLevel> replayBlueTeamLevels = context.ReplayMatchTeamLevels.Where(x => x.Replay.Hash == replayHash).Where(x => x.Team == StormTeam.Blue).OrderBy(x => x.TeamTimeTicks).ToList();
            List<ReplayMatchTeamLevel> replayRedTeamLevels = context.ReplayMatchTeamLevels.Where(x => x.Replay.Hash == replayHash).Where(x => x.Team == StormTeam.Red).OrderBy(x => x.TeamTimeTicks).ToList();

            Assert.AreEqual(1, replayBlueTeamLevels[0].TeamLevel);
            Assert.AreEqual(32500000, replayBlueTeamLevels[0].TeamTimeTicks);

            Assert.AreEqual(24, replayBlueTeamLevels[23].TeamLevel);
            Assert.AreEqual(16250625000, replayBlueTeamLevels[23].TeamTimeTicks);

            Assert.AreEqual(1, replayRedTeamLevels[0].TeamLevel);
            Assert.AreEqual(32500000, replayRedTeamLevels[0].TeamTimeTicks);

            Assert.AreEqual(25, replayRedTeamLevels[24].TeamLevel);
            Assert.AreEqual(16675625000, replayRedTeamLevels[24].TeamTimeTicks);
        }

        [TestMethod]
        public void AddReplay_AddTeamExperience_TeamExperienceAdded()
        {
            using HeroesReplaysDbContext context = DbServices.GetHeroesReplaysDbContext();

            IReplayParseData replayParseData = new ReplayParseData(_replayMatchRepository, _replayPlayerToonRepository, _replayPlayerRepository);
            string replayHash = replayParseData.GetReplayHash(_hanamura267679ReplayResult.Replay);

            replayParseData.AddReplay(context, _hanamura267679ReplayResult.FileName, replayHash, _hanamura267679ReplayResult.Replay);

            List<ReplayMatchTeamExperience> replayBlueTeamXP = context.ReplayMatchTeamExperiences.Where(x => x.Replay.Hash == replayHash).Where(x => x.Team == StormTeam.Blue).OrderBy(x => x.TimeTicks).ToList();
            List<ReplayMatchTeamExperience> replayRedTeamXP = context.ReplayMatchTeamExperiences.Where(x => x.Replay.Hash == replayHash).Where(x => x.Team == StormTeam.Red).OrderBy(x => x.TimeTicks).ToList();

            Assert.AreEqual(981250000, replayBlueTeamXP[0].TimeTicks);
            Assert.AreEqual(0, replayBlueTeamXP[0].TeamCreepXP);
            Assert.AreEqual(300, replayBlueTeamXP[0].TeamHeroXP);
            Assert.AreEqual(872, replayBlueTeamXP[0].TeamMinionXP);
            Assert.AreEqual(687, replayBlueTeamXP[0].TeamPassiveXP);
            Assert.AreEqual(0, replayBlueTeamXP[0].TeamStructureXP);

            Assert.AreEqual(17067500000, replayBlueTeamXP[27].TimeTicks);
            Assert.AreEqual(5261, replayBlueTeamXP[27].TeamCreepXP);
            Assert.AreEqual(32576, replayBlueTeamXP[27].TeamHeroXP);
            Assert.AreEqual(27351, replayBlueTeamXP[27].TeamMinionXP);
            Assert.AreEqual(44715, replayBlueTeamXP[27].TeamPassiveXP);
            Assert.AreEqual(8650, replayBlueTeamXP[27].TeamStructureXP);

            Assert.AreEqual(981250000, replayRedTeamXP[0].TimeTicks);
            Assert.AreEqual(0, replayRedTeamXP[0].TeamCreepXP);
            Assert.AreEqual(0, replayRedTeamXP[0].TeamHeroXP);
            Assert.AreEqual(1352, replayRedTeamXP[0].TeamMinionXP);
            Assert.AreEqual(687, replayRedTeamXP[0].TeamPassiveXP);
            Assert.AreEqual(0, replayRedTeamXP[0].TeamStructureXP);

            Assert.AreEqual(17067500000, replayRedTeamXP[27].TimeTicks);
            Assert.AreEqual(5819, replayRedTeamXP[27].TeamCreepXP);
            Assert.AreEqual(39420, replayRedTeamXP[27].TeamHeroXP);
            Assert.AreEqual(35223, replayRedTeamXP[27].TeamMinionXP);
            Assert.AreEqual(44715, replayRedTeamXP[27].TeamPassiveXP);
            Assert.AreEqual(10700, replayRedTeamXP[27].TeamStructureXP);
        }
    }
}
