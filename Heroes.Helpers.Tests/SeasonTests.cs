using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Heroes.Helpers.Tests
{
    [TestClass]
    public class SeasonTests
    {
        [TestMethod]
        public void SeasonListTest()
        {
            var seasonList = HeroesHelpers.Seasons.GetSeasonList();
            Assert.IsTrue(seasonList.Contains("Lifetime"));
            Assert.IsTrue(seasonList.Contains("Preseason"));

            foreach (string season in seasonList)
            {
                if (season.Contains("season") && season != "Preseason")
                {
                    var parts = season.Split(' ');
                    Assert.IsTrue(parts.Length == 3);

                    Assert.IsTrue(int.TryParse(parts[0], out int result));
                    Assert.IsTrue(int.TryParse(parts[2], out result));
                    Assert.IsTrue(parts[1] == "Season");
                }
            }
        }
    }
}
