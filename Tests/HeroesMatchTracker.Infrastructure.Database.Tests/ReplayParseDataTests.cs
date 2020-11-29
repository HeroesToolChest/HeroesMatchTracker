using Heroes.StormReplayParser;
using Heroes.StormReplayParser.Replay;
using HeroesMatchTracker.Core.Database.HeroesReplays;
using HeroesMatchTracker.Core.Services;
using HeroesMatchTracker.Infrastructure.Database.Contexts;
using HeroesMatchTracker.Shared.Entities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using System.IO;
using System.Linq;

namespace HeroesMatchTracker.Infrastructure.Tests
{
    [TestClass]
    public class ReplayParseDataTests
    {

        public readonly IReplayMatchRepository _replayMatchRepository;
        public readonly IReplayPlayerToonRepository _replayPlayerToonRepository;
        public readonly IReplayPlayerRepository _replayPlayerRepository;



        public ReplayParseDataTests()
        {
            _replayMatchRepository = Substitute.For<IReplayMatchRepository>();
            _replayPlayerToonRepository = Substitute.For<IReplayPlayerToonRepository>();
            _replayPlayerRepository = Substitute.For<IReplayPlayerRepository>();
        }

        [TestMethod]
        public void AddReplayTest()
        {
            string fileName = "HanamuraTemple2_67679.StormR";
            string file = Path.Combine("TestReplays", fileName);

            HeroesReplaysDbContext context = DbServices.GetHeroesReplaysDbContext();

            StormReplayResult replayResult = StormReplay.Parse(file, new ParseOptions()
            {
                ShouldParseGameEvents = false,
                ShouldParseMessageEvents = false,
                ShouldParseTrackerEvents = false,
            });

            IReplayParseData replayParseData = new ReplayParseData(_replayMatchRepository, _replayPlayerToonRepository, _replayPlayerRepository);
            string replayHash = replayParseData.GetReplayHash(replayResult.Replay);

            replayParseData.AddReplay(context, replayResult.FileName, replayHash, replayResult.Replay);

            ReplayMatch replayMatch = context.Replays.Where(x => x.Hash == "asdf1234").FirstOrDefault()!;

            Assert.AreEqual(1, context.Replays.Count());
            Assert.AreEqual("HanamuraTemple2_67679", replayMatch.FileName);
            Assert.AreEqual(StormGameMode.QuickMatch, replayMatch.GameMode);
            Assert.AreEqual("2.37.0.67679", replayMatch.ReplayVersion);
            Assert.AreEqual("Hanamura", replayMatch.MapId);
            Assert.AreEqual("Hanamura Temple", replayMatch.MapName);
        }
    }
}
