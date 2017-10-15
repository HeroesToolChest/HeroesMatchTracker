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

        [TestMethod]
        public void GetMapNameTranslation()
        {
            Assert.IsTrue(HeroesIcons.MapBackgrounds().MapNameTranslation("工业园区", out string mapName));
            Assert.IsTrue(mapName == "Industrial District");

            Assert.IsTrue(HeroesIcons.MapBackgrounds().MapNameTranslation("Ich glaub, es hakt", out mapName));
            Assert.IsTrue(mapName == "Pull Party");

            Assert.IsFalse(HeroesIcons.MapBackgrounds().MapNameTranslation(string.Empty, out mapName));
            Assert.IsTrue(mapName == string.Empty);

            Assert.IsFalse(HeroesIcons.MapBackgrounds().MapNameTranslation(null, out mapName));
            Assert.IsTrue(mapName == string.Empty);

            Assert.IsFalse(HeroesIcons.MapBackgrounds().MapNameTranslation("asdf", out mapName));
            Assert.IsTrue(mapName == null);
        }

        [TestMethod]
        public void GetMapNameByMapAlternativeName()
        {
            Assert.IsTrue(HeroesIcons.MapBackgrounds().GetMapNameByMapAlternativeName("ControlPoints") == "Sky Temple");
            Assert.IsTrue(HeroesIcons.MapBackgrounds().GetMapNameByMapAlternativeName("Shrines") == "Infernal Shrines");

            Assert.IsTrue(HeroesIcons.MapBackgrounds().GetMapNameByMapAlternativeName(string.Empty) == string.Empty);
            Assert.IsTrue(HeroesIcons.MapBackgrounds().GetMapNameByMapAlternativeName(null) == string.Empty);
            Assert.IsNull(HeroesIcons.MapBackgrounds().GetMapNameByMapAlternativeName("asdf"));
        }
    }
}
