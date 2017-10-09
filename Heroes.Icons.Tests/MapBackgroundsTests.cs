using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;

namespace Heroes.Icons.Tests
{
    [TestClass]
    public class MapBackgroundsTests : HeroesIconsBase
    {
        [TestMethod]
        public void HeroesMapBackgroundsTest()
        {
            List<string> assertMessages = new List<string>();

            Assert.AreEqual(HeroesIcons.MapBackgrounds().TotalCountOfMaps(), HeroesIcons.MapBackgrounds().GetMapsList().Count, "Number of maps in _AllMapBackgrounds.xml is not equal to number of MapBackgrounds");
            Assert.AreEqual(HeroesIcons.MapBackgrounds().TotalCountOfMaps(), Directory.GetFiles(Path.Combine("Xml", "MapBackgrounds")).Count() - 1, "Number of maps in _AllMapBackgrounds.xml is not equal to number of files in Xml\\MapBackgrounds");

            foreach (var map in HeroesIcons.MapBackgrounds().GetMapsList())
            {
                if (HeroesIcons.MapBackgrounds().GetMapBackground(map) == null)
                    assertMessages.Add($"No map stream found for {map}");
            }

            AssertFailMessage(assertMessages);
        }

        [TestMethod]
        public void GetMapBackgroundTest()
        {
            Assert.IsNotNull(HeroesIcons.MapBackgrounds().GetMapBackground("Battlefield of Eternity"));
            Assert.IsNull(HeroesIcons.MapBackgrounds().GetMapBackground("asdf"));
        }

        [TestMethod]
        public void GetMapBackgroundFontGlowColorTest()
        {
            Assert.IsFalse(HeroesIcons.MapBackgrounds().GetMapBackgroundFontGlowColor("Battlefield of Eternity") == Color.Black);
            Assert.IsTrue(HeroesIcons.MapBackgrounds().GetMapBackgroundFontGlowColor("asdf") == Color.Black);
        }
    }
}
