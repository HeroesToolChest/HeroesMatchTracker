using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Drawing;

namespace Heroes.Icons.Tests
{
    [TestClass]
    public class HomescreensTests : HeroesIconsBase
    {
        [TestMethod]
        public void HeroesHomescreensTest()
        {
            List<string> assertMessages = new List<string>();

            foreach (var homescreen in HeroesIcons.Homescreens().GetHomescreensList())
            {
                if (HeroesIcons.Homescreens().GetHomescreen(homescreen) == null)
                    assertMessages.Add($"No homescreen stream found for {homescreen}");
            }

            AssertFailMessage(assertMessages);
        }

        [TestMethod]
        public void GetHomescreenTest()
        {
            Assert.IsNotNull(HeroesIcons.Homescreens().GetHomescreen("Nexus"));
            Assert.IsNull(HeroesIcons.Homescreens().GetHomescreen("asdf"));
        }

        [TestMethod]
        public void GetHomescreenFontGlowColorTest()
        {
            Assert.IsFalse(HeroesIcons.Homescreens().GetHomescreenFontGlowColor("Nexus") == Color.Black);
            Assert.IsTrue(HeroesIcons.Homescreens().GetHomescreenFontGlowColor("asdf") == Color.Black);
        }
    }
}
