using Heroes.Icons.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;

namespace Heroes.Icons.Tests
{
    [TestClass]
    public class HeroesTests : HeroesIconsBase
    {
        [TestMethod]
        public void HeroTest()
        {
            List<string> assertMessages = new List<string>();
            int segment = 4;

            foreach (int build in HeroesIcons.GetListOfHeroesBuilds())
            {
                HeroesIcons.LoadHeroesBuild(build);

                var heroes = HeroesIcons.Heroes().GetListOfHeroes(build);

                foreach (string hero in heroes)
                {
                    Hero heroInfo = HeroesIcons.Heroes().GetHeroInfo(hero);

                    if (string.IsNullOrEmpty(heroInfo.AltName))
                        Assert.Fail($"[{build}] [{hero}] alt name is null or emtpy");

                    if (string.IsNullOrEmpty(heroInfo.AttributeId))
                        Assert.Fail($"[{build}] [{hero}] attributeId is null or emtpy");

                    if (string.IsNullOrEmpty(heroInfo.Description))
                        Assert.Fail($"[{build}] [{hero}] description is null or emtpy");

                    if (heroInfo.Type == HeroType.Unknown)
                        Assert.Fail($"[{build}] [{hero}] type is unknown");

                    if (heroInfo.Difficulty == HeroDifficulty.Unknown)
                        Assert.Fail($"[{build}] [{hero}] difficulty is unknown");

                    if (heroInfo.BuildAvailable == 0)
                        Assert.Fail($"[{build}] [{hero}] buildavailable is 0");

                    if (heroInfo.GetLeaderboardPortrait().UriSource.Segments[segment].ToString() != $"storm_ui_ingame_hero_leaderboard_{GetUniqueHeroName(heroInfo.AltName.ToLower())}.dds")
                        assertMessages.Add($"[{build}] Leaderboard portrait not found for {hero}");

                    if (heroInfo.GetLoadingPortrait().UriSource.Segments[segment].ToString() != $"storm_ui_ingame_hero_loadingscreen_{GetUniqueHeroName(heroInfo.AltName.ToLower())}.dds")
                        assertMessages.Add($"[{build}] Loading portrait not found for {hero}");

                    if (heroInfo.GetPortrait().UriSource.Segments[segment].ToString() != $"storm_ui_ingame_heroselect_btn_{GetUniqueHeroName(heroInfo.AltName.ToLower())}.dds")
                        assertMessages.Add($"[{build}] Hero portrait not found for {hero}");

                    if (heroInfo.Franchise == HeroFranchise.Unknown)
                        assertMessages.Add($"[{build}] Unknown franchise for {hero}");

                    var heroRoles = heroInfo.Roles;
                    if (heroRoles[0] == HeroRole.Unknown)
                        assertMessages.Add($"[{build}] Unknown hero role for {hero}");
                    if (heroRoles.Count > 1 && heroRoles[0] != HeroRole.Multiclass)
                        assertMessages.Add($"[{build}] Hero {hero} has multiple roles but first role is NOT Multiclass");
                }
            }

            AssertFailMessage(assertMessages);
        }

        [TestMethod]
        public void GetHeroInfoTest()
        {
            Hero heroInfo = HeroesIcons.Heroes().GetHeroInfo(string.Empty);
            Assert.IsTrue(heroInfo.Name == string.Empty);
            Assert.IsTrue(heroInfo.Franchise == HeroFranchise.Unknown);
            Assert.IsTrue(heroInfo.HeroPortrait == new Uri($@"{ApplicationIconsPath}\HeroPortraits\{NoPortraitFound}", UriKind.Absolute));
            Assert.IsTrue(heroInfo.LoadingPortrait == new Uri($@"{ApplicationIconsPath}\HeroLoadingScreenPortraits\{NoLoadingScreenFound}", UriKind.Absolute));
            Assert.IsTrue(heroInfo.LeaderboardPortrait == new Uri($@"{ApplicationIconsPath}\HeroLeaderboardPortraits\{NoLeaderboardFound}", UriKind.Absolute));

            heroInfo = HeroesIcons.Heroes().GetHeroInfo("Anubarak");
            Assert.IsTrue(heroInfo.Name == "Anub'arak");
            Assert.IsTrue(heroInfo.Franchise == HeroFranchise.Warcraft);
        }

        [TestMethod]
        public void HeroNameTranslationTest()
        {
            HeroesIcons.Heroes().HeroNameTranslation("АБАТУР", out string heroEnglish);
            Assert.IsTrue("Abathur" == heroEnglish);

            Assert.IsTrue(HeroesIcons.Heroes().HeroNameTranslation("Abathur", out heroEnglish));
            Assert.IsTrue("Abathur" == heroEnglish);
        }

        [TestMethod]
        public void GetRealHeroNameFromAttributeIdTest()
        {
            Assert.IsTrue(HeroesIcons.Heroes().GetRealHeroNameFromAttributeId("Abat") == "Abathur");
            Assert.IsTrue(HeroesIcons.Heroes().GetRealHeroNameFromAttributeId("Anub") == "Anub'arak");
            Assert.IsTrue(HeroesIcons.Heroes().GetRealHeroNameFromAttributeId(string.Empty) == string.Empty);
            Assert.IsTrue(HeroesIcons.Heroes().GetRealHeroNameFromAttributeId(null) == string.Empty);
            Assert.IsTrue(HeroesIcons.Heroes().GetRealHeroNameFromAttributeId("asdf") == null);
        }

        [TestMethod]
        public void GetRealHeroNameFromAltNameTest()
        {
            Assert.IsTrue(HeroesIcons.Heroes().GetRealHeroNameFromAltName("Anubarak") == "Anub'arak");
            Assert.IsTrue(HeroesIcons.Heroes().GetRealHeroNameFromAltName("Abathur") == "Abathur");
            Assert.IsTrue(HeroesIcons.Heroes().GetRealHeroNameFromAttributeId(string.Empty) == string.Empty);
            Assert.IsTrue(HeroesIcons.Heroes().GetRealHeroNameFromAttributeId(null) == string.Empty);
            Assert.IsTrue(HeroesIcons.Heroes().GetRealHeroNameFromAttributeId("asdf") == null);
        }

        [TestMethod]
        public void HeroExistsTest()
        {
            Assert.IsTrue(HeroesIcons.Heroes().HeroExists("Anubarak"));
            Assert.IsTrue(HeroesIcons.Heroes().HeroExists("Anub'arak"));

            Assert.IsFalse(HeroesIcons.Heroes().HeroExists("asdf"));
        }
    }
}
