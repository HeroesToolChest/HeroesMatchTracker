using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Heroes.Helpers.Tests
{
    [TestClass]
    public class BattleTagsTests
    {
        [TestMethod]
        public void BattleTagNameTest()
        {
            Assert.AreEqual("name1234#998866", HeroesHelpers.BattleTags.GetBattleTagName("name1234", 998866));
            Assert.AreEqual(null, HeroesHelpers.BattleTags.GetBattleTagName("", 1111));
        }

        [TestMethod]
        public void NameFromBattleTagNameTest()
        {
            Assert.AreEqual("name", HeroesHelpers.BattleTags.GetNameFromBattleTagName("name#1234"));
            Assert.AreEqual(null, HeroesHelpers.BattleTags.GetNameFromBattleTagName("name"));
            Assert.AreEqual("name", HeroesHelpers.BattleTags.GetNameFromBattleTagName("name#"));
            Assert.AreEqual(null, HeroesHelpers.BattleTags.GetNameFromBattleTagName("#456"));
            Assert.AreEqual(null, HeroesHelpers.BattleTags.GetNameFromBattleTagName(""));
        }

        [TestMethod]
        public void TagFromBattleTagNameTest()
        {
            Assert.AreEqual(345435, HeroesHelpers.BattleTags.GetTagFromBattleTagName("name#345435"));
            Assert.AreEqual(0, HeroesHelpers.BattleTags.GetTagFromBattleTagName("name"));
            Assert.AreEqual(0, HeroesHelpers.BattleTags.GetTagFromBattleTagName("name#"));
            Assert.AreEqual(456, HeroesHelpers.BattleTags.GetTagFromBattleTagName("#456"));
            Assert.AreEqual(0, HeroesHelpers.BattleTags.GetTagFromBattleTagName(""));
        }
    }
}
