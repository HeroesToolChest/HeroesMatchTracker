using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Media;

namespace Heroes.Icons.Tests
{
    [TestClass]
    public class MapBackgroundsTests : HeroesIconsBase
    {
        [TestMethod]
        public void HeroesMapBackgroundsTest()
        {
            List<string> assertMessages = new List<string>();

            Assert.AreEqual(HeroesIcons.MapBackgrounds().TotalCountOfMaps(), HeroesIcons.MapBackgrounds().GetMapsList().Count, "Number of awards in _AllMapBackgrounds.xml is not equal to number of MapBackgrounds");
            Assert.AreEqual(HeroesIcons.MapBackgrounds().TotalCountOfMaps(), Directory.GetFiles($@"Xml\MapBackgrounds").Count() - 1, "Number of maps in _AllMapBackgrounds.xml is not equal to number of files in Xml\\MapBackgrounds");

            foreach (var map in HeroesIcons.MapBackgrounds().GetMapsList())
            {
                if (HeroesIcons.MapBackgrounds().GetMapBackground(map) == null)
                    assertMessages.Add($"No image found for {map}");
            }
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
            Assert.IsFalse(HeroesIcons.MapBackgrounds().GetMapBackgroundFontGlowColor("Battlefield of Eternity") == Colors.Black);
            Assert.IsTrue(HeroesIcons.MapBackgrounds().GetMapBackgroundFontGlowColor("asdf") == Colors.Black);
        }
    }
}
