using Heroes.Icons.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace Heroes.Icons.Tests
{
    [TestClass]
    public class MatchAwardsTests : HeroesIconsBase
    {
        [TestMethod]
        public void GetMVPScreenAwardTest()
        {
            List<string> assertMessages = new List<string>();

            foreach (var map in HeroesIcons.MatchAwards().GetMatchAwardsList())
            {
                if (HeroesIcons.MatchAwards().GetMVPScreenAward(map, MVPScreenColor.Blue, out string awardNameBlue) == null || string.IsNullOrEmpty(awardNameBlue))
                    assertMessages.Add($"[Screen Award] [{map}] (blue) Stream is null or no award name");

                if (HeroesIcons.MatchAwards().GetMVPScreenAward(map, MVPScreenColor.Red, out string awardNameRed) == null || string.IsNullOrEmpty(awardNameRed))
                    assertMessages.Add($"[Screen Award] [{map}] (red) Stream is null or no award name");

                if (HeroesIcons.MatchAwards().GetMVPScreenAward(map, MVPScreenColor.Gold, out string awardNameGold) == null || string.IsNullOrEmpty(awardNameGold))
                    assertMessages.Add($"[Screen Award] [{map}] (gold) Stream is null or no award name");
            }

            if (HeroesIcons.MatchAwards().GetMVPScreenAward("asdf", MVPScreenColor.Blue, out string awardName) == null || string.IsNullOrEmpty(awardName))
                assertMessages.Add($"[Screen Award] [asdf] (blue) Stream is null or no award name");

            AssertFailMessage(assertMessages);
        }

        [TestMethod]
        public void GetMVPScoreScreenAwardTest()
        {
            List<string> assertMessages = new List<string>();

            foreach (var map in HeroesIcons.MatchAwards().GetMatchAwardsList())
            {
                if (HeroesIcons.MatchAwards().GetMVPScoreScreenAward(map, MVPScoreScreenColor.Blue, out string awardNameBlue) == null || string.IsNullOrEmpty(awardNameBlue))
                    assertMessages.Add($"[Score Screen Award] [{map}] (blue) Stream is null or no award name");

                if (HeroesIcons.MatchAwards().GetMVPScoreScreenAward(map, MVPScoreScreenColor.Red, out string awardNameRed) == null || string.IsNullOrEmpty(awardNameRed))
                    assertMessages.Add($"[Score Screen Award] [{map}] (red) Stream is null or no award name");
            }

            if (HeroesIcons.MatchAwards().GetMVPScoreScreenAward("asdf", MVPScoreScreenColor.Blue, out string awardName) == null || string.IsNullOrEmpty(awardName))
                assertMessages.Add($"[Score Screen Award] [asdf] (blue) Stream is null or no award name");

            AssertFailMessage(assertMessages);
        }

        [TestMethod]
        public void GetMatchAwardDescriptionTest()
        {
            List<string> assertMessages = new List<string>();

            foreach (var map in HeroesIcons.MatchAwards().GetMatchAwardsList())
            {
                if (string.IsNullOrEmpty(HeroesIcons.MatchAwards().GetMatchAwardDescription(map)))
                    assertMessages.Add($"[{map}] description is null or empty");
            }

            if (!string.IsNullOrEmpty(HeroesIcons.MatchAwards().GetMatchAwardDescription("asdf")))
                assertMessages.Add($"[asdf] description is NOT empty");

            AssertFailMessage(assertMessages);
        }
    }
}
