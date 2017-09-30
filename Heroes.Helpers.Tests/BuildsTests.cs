using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace Heroes.Helpers.Tests
{
    [TestClass]
    public class BuildsTests
    {
        [TestMethod]
        public void GetReplayBuildsFromSeasonTest()
        {
            foreach (Season season in Enum.GetValues(typeof(Season)))
            {
                Assert.IsNotNull(HeroesHelpers.Builds.GetReplayBuildsFromSeason(season));
            }
        }
    }
}
