using Heroes.Icons.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace Heroes.Icons.Tests
{
    [TestClass]
    public class HeroesBuildsTests : HeroesIconsBase
    {
        [TestMethod]
        public void HeroesTests()
        {
            List<string> assertMessages = new List<string>();

            foreach (int build in HeroesIcons.GetListOfHeroesBuilds().ConvertAll(x => int.Parse(x)))
            {
                HeroesIcons.LoadHeroesBuild(build);

                var heroes = HeroesIcons.HeroBuilds().GetListOfHeroes(build);

                foreach (var hero in heroes)
                {
                    Hero heroInfo = HeroesIcons.HeroBuilds().GetHeroInfo(hero);

                    if (string.IsNullOrEmpty(heroInfo.Name))
                        assertMessages.Add($"[{build}] [{hero}] name is null or emtpy");

                    if (string.IsNullOrEmpty(heroInfo.ShortName))
                        assertMessages.Add($"[{build}] [{hero}] short name is null or emtpy");

                    if (build >= HeroesBase.HeroUnitsAddedBuild && string.IsNullOrEmpty(heroInfo.UnitName))
                        assertMessages.Add($"[{build}] [{hero}] unit name is null or emtpy");

                    if (string.IsNullOrEmpty(heroInfo.AttributeId))
                        assertMessages.Add($"[{build}] [{hero}] attributeId is null or emtpy");

                    if (build >= HeroesBase.DescriptionsAddedBuild && string.IsNullOrEmpty(heroInfo.Description))
                        assertMessages.Add($"[{build}] [{hero}] description is null or emtpy");

                    if (heroInfo.Type == HeroType.Unknown)
                        assertMessages.Add($"[{build}] [{hero}] type is unknown");

                    if (heroInfo.Difficulty == HeroDifficulty.Unknown)
                        assertMessages.Add($"[{build}] [{hero}] difficulty is unknown");

                    if (heroInfo.BuildAvailable == 0)
                        assertMessages.Add($"[{build}] [{hero}] build available is 0");

                    if (heroInfo.Franchise == HeroFranchise.Unknown)
                        assertMessages.Add($"[{build}] Unknown franchise for {hero}");

                    var heroRoles = heroInfo.Roles;
                    if (heroRoles[0] == HeroRole.Unknown)
                        assertMessages.Add($"[{build}] Unknown hero role for {hero}");
                    if (heroRoles.Count > 1 && heroRoles[0] != HeroRole.Multiclass)
                        assertMessages.Add($"[{build}] Hero {hero} has multiple roles but first role is NOT Multiclass");

                    if (!heroInfo.HeroPortrait.EndsWith($"{HeroPortraitPrefix}{GetUniqueHeroName(heroInfo.ShortName.ToLowerInvariant())}.dds"))
                        assertMessages.Add($"[{build}] Hero portrait string not found for {hero}");

                    if (!heroInfo.LoadingPortrait.EndsWith($"{LoadingPortraitPrefix}{GetUniqueHeroName(heroInfo.ShortName.ToLowerInvariant())}.dds"))
                        assertMessages.Add($"[{build}] Loading portrait string not found for {hero}");

                    if (!heroInfo.LeaderboardPortrait.EndsWith($"{LeaderboardPortraitPrefix}{GetUniqueHeroName(heroInfo.ShortName.ToLowerInvariant())}.dds"))
                        assertMessages.Add($"[{build}] Leaderboard portrait string not found for {hero}");

                    if (heroInfo.GetHeroPortrait() == null)
                        assertMessages.Add($"[{build}] Hero portrait stream not found for {hero}");

                    if (heroInfo.GetLoadingPortrait() == null)
                        assertMessages.Add($"[{build}] Loading portrait stream not found for {hero}");

                    if (heroInfo.GetLeaderboardPortrait() == null)
                        assertMessages.Add($"[{build}] Leaderboard portrait stream not found for {hero}");
                }
            }

            AssertFailMessage(assertMessages);
        }

        [TestMethod]
        public void GetHeroInfoTest()
        {
            Hero heroInfo = HeroesIcons.HeroBuilds().GetHeroInfo(string.Empty);
            Assert.IsTrue(heroInfo.Name == string.Empty);
            Assert.IsTrue(heroInfo.Franchise == HeroFranchise.Unknown);
            Assert.IsTrue(heroInfo.HeroPortrait.Contains(HeroesBase.NoPortraitFound));
            Assert.IsTrue(heroInfo.LeaderboardPortrait.Contains(HeroesBase.NoLeaderboardFound));
            Assert.IsTrue(heroInfo.LoadingPortrait.Contains(HeroesBase.NoLoadingScreenFound));
            Assert.IsTrue(heroInfo.GetHeroPortrait() != null);
            Assert.IsTrue(heroInfo.GetLeaderboardPortrait() != null);
            Assert.IsTrue(heroInfo.GetLoadingPortrait() != null);

            heroInfo = HeroesIcons.HeroBuilds().GetHeroInfo(null);
            Assert.IsTrue(heroInfo.Name == null);
            Assert.IsTrue(heroInfo.Franchise == HeroFranchise.Unknown);
            Assert.IsTrue(heroInfo.HeroPortrait.Contains(HeroesBase.NoPortraitPick));
            Assert.IsTrue(heroInfo.LeaderboardPortrait.Contains(HeroesBase.NoLeaderboardPick));
            Assert.IsTrue(heroInfo.LoadingPortrait.Contains(HeroesBase.NoLoadingScreenPick));
            Assert.IsTrue(heroInfo.GetHeroPortrait() != null);
            Assert.IsTrue(heroInfo.GetLeaderboardPortrait() != null);
            Assert.IsTrue(heroInfo.GetLoadingPortrait() != null);

            heroInfo = HeroesIcons.HeroBuilds().GetHeroInfo("Anubarak");
            Assert.IsTrue(heroInfo.Name == "Anub'arak");
            Assert.IsTrue(heroInfo.Franchise == HeroFranchise.Warcraft);
        }

        [TestMethod]
        public void GetRealHeroNameFromAttributeIdTest()
        {
            Assert.IsTrue(HeroesIcons.HeroBuilds().GetRealHeroNameFromAttributeId("Abat") == "Abathur");
            Assert.IsTrue(HeroesIcons.HeroBuilds().GetRealHeroNameFromAttributeId("Anub") == "Anub'arak");
            Assert.IsTrue(HeroesIcons.HeroBuilds().GetRealHeroNameFromAttributeId(string.Empty) == string.Empty);
            Assert.IsTrue(HeroesIcons.HeroBuilds().GetRealHeroNameFromAttributeId(null) == string.Empty);
            Assert.IsTrue(HeroesIcons.HeroBuilds().GetRealHeroNameFromAttributeId("asdf") == null);
        }

        [TestMethod]
        public void HeroUnitNameTest()
        {
            Assert.IsTrue(HeroesIcons.HeroBuilds().GetRealHeroNameFromHeroUnitName("HeroAbathur") == "Abathur");
            Assert.IsTrue(HeroesIcons.HeroBuilds().GetRealHeroNameFromHeroUnitName(string.Empty) == string.Empty);
            Assert.IsTrue(HeroesIcons.HeroBuilds().GetRealHeroNameFromHeroUnitName(null) == string.Empty);
            Assert.IsTrue(HeroesIcons.HeroBuilds().GetRealHeroNameFromHeroUnitName("asdf") == null);
        }

        [TestMethod]
        public void GetRealHeroNameFromShortNameTest()
        {
            Assert.IsTrue(HeroesIcons.HeroBuilds().GetRealHeroNameFromShortName("Anubarak") == "Anub'arak");
            Assert.IsTrue(HeroesIcons.HeroBuilds().GetRealHeroNameFromShortName("Abathur") == "Abathur");
            Assert.IsTrue(HeroesIcons.HeroBuilds().GetRealHeroNameFromShortName(string.Empty) == string.Empty);
            Assert.IsTrue(HeroesIcons.HeroBuilds().GetRealHeroNameFromShortName(null) == string.Empty);
            Assert.IsTrue(HeroesIcons.HeroBuilds().GetRealHeroNameFromShortName("asdf") == null);
        }

        [TestMethod]
        public void HeroExistsTest()
        {
            Assert.IsTrue(HeroesIcons.HeroBuilds().HeroExists("Anubarak"));
            Assert.IsTrue(HeroesIcons.HeroBuilds().HeroExists("Anub'arak"));

            Assert.IsFalse(HeroesIcons.HeroBuilds().HeroExists("asdf"));
        }

        //[TestMethod]
        //public void HeroesTalentsTest()
        //{
        //    List<string> assertMessages = new List<string>();

        //    foreach (int build in HeroesIcons.GetListOfHeroesBuilds().ConvertAll(x => int.Parse(x)))
        //    {
        //        HeroesIcons.LoadHeroesBuild(build);

        //        var heroes = HeroesIcons.Heroes().GetListOfHeroes(build);

        //        foreach (string hero in heroes)
        //        {
        //            var allTalents = HeroesIcons.HeroBuilds().GetHeroTalents(hero);

        //            if (allTalents == null)
        //            {
        //                assertMessages.Add($"[{build}] [{hero}] No talents found");
        //                continue;
        //            }

        //            int talent1 = 0;
        //            int talent4 = 0;
        //            int talent7 = 0;
        //            int talent10 = 0;
        //            int talent13 = 0;
        //            int talent16 = 0;
        //            int talent20 = 0;

        //            foreach (var talent in allTalents.Values)
        //            {
        //                if (talent.Tier == TalentTier.Old)
        //                    continue;

        //                if (talent.Tier == TalentTier.Level1)
        //                    talent1++;
        //                else if (talent.Tier == TalentTier.Level4)
        //                    talent4++;
        //                else if (talent.Tier == TalentTier.Level7)
        //                    talent7++;
        //                else if (talent.Tier == TalentTier.Level10)
        //                    talent10++;
        //                else if (talent.Tier == TalentTier.Level13)
        //                    talent13++;
        //                else if (talent.Tier == TalentTier.Level16)
        //                    talent16++;
        //                else if (talent.Tier == TalentTier.Level20)
        //                    talent20++;

        //                BitmapImage talentImage = talent.GetIcon();

        //                if (talentImage.UriSource.Segments[segment].ToString() == HeroesBase.NoTalentIconPick || talentImage.UriSource.Segments[segment].ToString() == HeroesBase.NoTalentIconFound)
        //                    assertMessages.Add($"[{build}] [{hero}] [{talent}] Talent image not found");

        //                if (string.IsNullOrEmpty(talent.ReferenceName))
        //                    assertMessages.Add($"[{build}] [{hero}] [{talent}] No reference name");

        //                if (string.IsNullOrEmpty(talent.TooltipDescriptionName))
        //                    assertMessages.Add($"[{build}] [{hero}] [{talent}] No tooltip description name");

        //                // tooltips
        //                TalentTooltip talentTooltip = talent.Tooltip;

        //                // full
        //                if (string.IsNullOrEmpty(talentTooltip.Full))
        //                {
        //                    assertMessages.Add($"[{build}] [{hero}] [{talent}] Full tooltip not found");
        //                }
        //                else
        //                {
        //                    string strippedText = TalentTooltipStripNonText(talentTooltip.Full);

        //                    if (NonValidCharsCheck(strippedText))
        //                        assertMessages.Add($"[{build}] [{hero}] [{talent}] Invalid chars in FULL tooltip{Environment.NewLine}{strippedText}{Environment.NewLine}");
        //                }

        //                // short
        //                if (string.IsNullOrEmpty(talentTooltip.Short))
        //                {
        //                    assertMessages.Add($"[{build}] [{hero}] [{talent}] Short tooltip not found");
        //                }
        //                else
        //                {
        //                    string strippedText = TalentTooltipStripNonText(talentTooltip.Short);

        //                    if (NonValidCharsCheck(strippedText))
        //                        assertMessages.Add($"[{build}] [{hero}] [{talent}] Invalid chars in SHORT tooltip{Environment.NewLine}{strippedText}{Environment.NewLine}");
        //                }
        //            }

        //            if (talent1 < 1)
        //                assertMessages.Add($"[{build}] [{hero}] No Level 1 talents");
        //            if (talent4 < 1)
        //                assertMessages.Add($"[{build}] [{hero}] No Level 4 talents");
        //            if (talent7 < 1)
        //                assertMessages.Add($"[{build}] [{hero}] No Level 7 talents");
        //            if (talent10 < 1)
        //                assertMessages.Add($"[{build}] [{hero}] No Level 10 talents");
        //            if (talent13 < 1)
        //                assertMessages.Add($"[{build}] [{hero}] No Level 13 talents");
        //            if (talent16 < 1)
        //                assertMessages.Add($"[{build}] [{hero}] No Level 16 talents");
        //            if (talent20 < 1)
        //                assertMessages.Add($"[{build}] [{hero}] No Level 20 talents");
        //        }
        //    }

        //    AssertFailMessage(assertMessages);
        //}
    }
}
