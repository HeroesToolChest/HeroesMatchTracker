using Heroes.StormReplayParser;
using Heroes.StormReplayParser.Replay;
using HeroesMatchTracker.Core.Database.HeroesReplays;
using HeroesMatchTracker.Core.Services;
using HeroesMatchTracker.Infrastructure.Database.Contexts;
using HeroesMatchTracker.Infrastructure.Database.Repository.HeroesReplays;
using HeroesMatchTracker.Infrastructure.ReplayParser;
using HeroesMatchTracker.Shared.Entities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;
using System.Linq;

namespace HeroesMatchTracker.Infrastructure.Tests
{
    [TestClass]
    public class ReplayParseDataTests
    {

        private readonly IReplayMatchRepository _replayMatchRepository;
        private readonly IReplayPlayerToonRepository _replayPlayerToonRepository;
        private readonly IReplayPlayerRepository _replayPlayerRepository;

        public ReplayParseDataTests()
        {
            _replayMatchRepository = new ReplayMatchRepository();
            _replayPlayerToonRepository = new ReplayPlayerToonRepository();
            _replayPlayerRepository = new ReplayPlayerRepository();
        }

        [TestMethod]
        public void AddReplayTest()
        {
            string fileName = "HanamuraTemple2_67679.StormR";
            string file = Path.Combine("TestReplays", fileName);

            using HeroesReplaysDbContext context = DbServices.GetHeroesReplaysDbContext();

            StormReplayResult replayResult = StormReplay.Parse(file, new ParseOptions()
            {
                ShouldParseGameEvents = true,
                ShouldParseMessageEvents = true,
                ShouldParseTrackerEvents = true,
            });

            IReplayParseData replayParseData = new ReplayParseData(_replayMatchRepository, _replayPlayerToonRepository, _replayPlayerRepository);
            string replayHash = replayParseData.GetReplayHash(replayResult.Replay);

            Assert.AreEqual(replayHash, "2db896de6a3a97dd10a5bbf68f57dcd146524de8");

            replayParseData.AddReplay(context, replayResult.FileName, replayHash, replayResult.Replay);

            ReplayMatch replayMatchDb = context.Replays.Where(x => x.Hash == replayHash).FirstOrDefault()!;

            Assert.AreEqual(1, context.Replays.Count());
            Assert.AreEqual(fileName, replayMatchDb.FileName);
            Assert.AreEqual(StormGameMode.QuickMatch, replayMatchDb.GameMode);
            Assert.AreEqual("2.37.0.67679", replayMatchDb.ReplayVersion);
            Assert.AreEqual("Hanamura", replayMatchDb.MapId);
            Assert.AreEqual("Hanamura Temple", replayMatchDb.MapName);
            Assert.AreEqual(10, replayMatchDb.ReplayMatchPlayers!.Count);
        }

        [TestMethod]
        public void IsReplayExistsTest()
        {
            string fileName = "HanamuraTemple2_67679.StormR";
            string file = Path.Combine("TestReplays", fileName);

            using HeroesReplaysDbContext context = DbServices.GetHeroesReplaysDbContext();

            StormReplayResult replayResult = StormReplay.Parse(file, new ParseOptions()
            {
                ShouldParseGameEvents = true,
                ShouldParseMessageEvents = true,
                ShouldParseTrackerEvents = true,
            });

            IReplayParseData replayParseData = new ReplayParseData(_replayMatchRepository, _replayPlayerToonRepository, _replayPlayerRepository);
            string replayHash = replayParseData.GetReplayHash(replayResult.Replay);

            Assert.AreEqual(replayHash, "2db896de6a3a97dd10a5bbf68f57dcd146524de8");
            Assert.IsFalse(replayParseData.IsReplayExists(context, replayHash));

            replayParseData.AddReplay(context, replayResult.FileName, replayHash, replayResult.Replay);

            ReplayMatch replayMatchDb = context.Replays.Where(x => x.Hash == replayHash).FirstOrDefault()!;

            Assert.AreEqual(1, context.Replays.Count());
            Assert.IsTrue(replayParseData.IsReplayExists(context, replayHash));
        }

        [TestMethod]
        public void AddReplayExistingPlayerTest()
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
        public void AddReplayExistingPlayerNewBattleTagTest()
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
        public void AddReplayExistingPlayerNewBattleTagTwiceTest()
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
    }
}
