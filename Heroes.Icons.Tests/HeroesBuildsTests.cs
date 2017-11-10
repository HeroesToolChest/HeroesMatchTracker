using Heroes.Icons.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
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
                        Assert.Fail($"[{build}] [{hero}] name is null or emtpy");

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

                    if (!heroInfo.HeroPortrait.EndsWith($"{HeroPortraitPrefix}{GetUniqueHeroName(heroInfo.ShortName.ToLowerInvariant())}.png"))
                        assertMessages.Add($"[{build}] Hero portrait string not found for {hero}");

                    if (!heroInfo.LoadingPortrait.EndsWith($"{LoadingPortraitPrefix}{GetUniqueHeroName(heroInfo.ShortName.ToLowerInvariant())}.png"))
                        assertMessages.Add($"[{build}] Loading portrait string not found for {hero}");

                    if (!heroInfo.LeaderboardPortrait.EndsWith($"{LeaderboardPortraitPrefix}{GetUniqueHeroName(heroInfo.ShortName.ToLowerInvariant())}.png"))
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
        public void HeroAbilitiesTest()
        {
            List<string> assertMessages = new List<string>();

            foreach (int build in HeroesIcons.GetListOfHeroesBuilds().ConvertAll(x => int.Parse(x)))
            {
                if (build < 58623)
                    continue;

                HeroesIcons.LoadHeroesBuild(build);

                var heroes = HeroesIcons.HeroBuilds().GetListOfHeroes(build);

                foreach (var hero in heroes)
                {
                    Hero heroInfo = HeroesIcons.HeroBuilds().GetHeroInfo(hero);

                    if (heroInfo.Abilities.Values.Count < 1)
                    {
                        assertMessages.Add($"[{build}] [{hero}] No abilities found");
                        continue;
                    }

                    if (heroInfo.GetTierAbilities(AbilityTier.Basic).Count < 1)
                        assertMessages.Add($"[{build}] [{hero}] No basic abilities");
                    if (heroInfo.GetTierAbilities(AbilityTier.Heroic).Count < 1)
                        assertMessages.Add($"[{build}] [{hero}] No heroic abilities");

                    // test a no pick (empty)
                    Ability testAbility = heroInfo.GetAbility(string.Empty);
                    if (testAbility != null)
                        assertMessages.Add($"[{build}] [{hero}] no pick (empty) ability is not null");

                    // test a no pick (null)
                    testAbility = heroInfo.GetAbility(string.Empty);
                    if (testAbility != null)
                        assertMessages.Add($"[{build}] [{hero}] no pick (null) ability is not null");

                    // test a not found
                    testAbility = heroInfo.GetAbility("asdf");
                    if (testAbility != null)
                        assertMessages.Add($"[{build}] [{hero}] not found ability is not null");

                    foreach (var ability in heroInfo.Abilities.Values)
                    {
                        if (string.IsNullOrEmpty(ability.Name))
                            Assert.Fail($"[{build}] [{hero}] [{ability}] name is null or emtpy");

                        if (string.IsNullOrEmpty(ability.ReferenceName))
                            assertMessages.Add($"[{build}] [{hero}] [{ability}] ability reference name is null or emtpy");

                        if (string.IsNullOrEmpty(ability.TooltipDescriptionName) && ability.Name != "[None]")
                            assertMessages.Add($"[{build}] [{hero}] [{ability}] talent tooltip description name is null or emtpy");

                        // tooltips
                        TalentTooltip talentTooltip = ability.Tooltip;

                        // full
                        if (string.IsNullOrEmpty(talentTooltip.Full) && ability.Name != "[None]")
                        {
                            assertMessages.Add($"[{build}] [{hero}] [{ability}] Full tooltip is null or empty");
                        }
                        else
                        {
                            string strippedText = TalentTooltipStripNonText(talentTooltip.Full);

                            if (NonValidCharsCheck(strippedText))
                                assertMessages.Add($"[{build}] [{hero}] [{ability}] Invalid chars in FULL tooltip{Environment.NewLine}{strippedText}{Environment.NewLine}");
                        }

                        // short
                        if (string.IsNullOrEmpty(talentTooltip.Short) && ability.Name != "[None]")
                        {
                            assertMessages.Add($"[{build}] [{hero}] [{ability}] Short tooltip is null or empty");
                        }
                        else
                        {
                            string strippedText = TalentTooltipStripNonText(talentTooltip.Short);

                            if (NonValidCharsCheck(strippedText))
                                assertMessages.Add($"[{build}] [{hero}] [{ability}] Invalid chars in SHORT tooltip{Environment.NewLine}{strippedText}{Environment.NewLine}");
                        }

                        if (ability.GetIcon() == null && ability.Name != "[None]")
                            assertMessages.Add($"[{build}] [{hero}] [{ability}] Icon stream is null");

                        testAbility = heroInfo.GetAbility(ability.ReferenceName);
                        if (testAbility == null || testAbility.Name == ability.ReferenceName)
                            assertMessages.Add($"[{build}] [{hero}] [{ability}] GetAbility() failed to return correct info");
                    }
                }
            }

            AssertFailMessage(assertMessages);
        }

        [TestMethod]
        public void HeroTalentsTest()
        {
            List<string> assertMessages = new List<string>();

            foreach (int build in HeroesIcons.GetListOfHeroesBuilds().ConvertAll(x => int.Parse(x)))
            {
                HeroesIcons.LoadHeroesBuild(build);

                var heroes = HeroesIcons.HeroBuilds().GetListOfHeroes(build);

                foreach (var hero in heroes)
                {
                    Hero heroInfo = HeroesIcons.HeroBuilds().GetHeroInfo(hero);

                    if (heroInfo.Talents.Values.Count < 1)
                    {
                        assertMessages.Add($"[{build}] [{hero}] No talents found");
                        continue;
                    }

                    if (heroInfo.GetTierTalents(TalentTier.Level1).Count < 1)
                        assertMessages.Add($"[{build}] [{hero}] No Level 1 talents");
                    if (heroInfo.GetTierTalents(TalentTier.Level4).Count < 1)
                        assertMessages.Add($"[{build}] [{hero}] No Level 4 talents");
                    if (heroInfo.GetTierTalents(TalentTier.Level7).Count < 1)
                        assertMessages.Add($"[{build}] [{hero}] No Level 7 talents");
                    if (heroInfo.GetTierTalents(TalentTier.Level10).Count < 1)
                        assertMessages.Add($"[{build}] [{hero}] No Level 10 talents");
                    if (heroInfo.GetTierTalents(TalentTier.Level13).Count < 1)
                        assertMessages.Add($"[{build}] [{hero}] No Level 13 talents");
                    if (heroInfo.GetTierTalents(TalentTier.Level16).Count < 1)
                        assertMessages.Add($"[{build}] [{hero}] No Level 16 talents");
                    if (heroInfo.GetTierTalents(TalentTier.Level20).Count < 1)
                        assertMessages.Add($"[{build}] [{hero}] No Level 20 talents");

                    // test a no pick (empty)
                    Talent testTalent = heroInfo.GetTalent(string.Empty);
                    if (testTalent.Name != "No pick" || testTalent.GetIcon() == null)
                        assertMessages.Add($"[{build}] [{hero}] no pick (empty) talent has incorrect name or icon steam is null");

                    // test a no pick (null)
                    testTalent = heroInfo.GetTalent(null);
                    if (testTalent.Name != "No pick" || testTalent.GetIcon() == null)
                        assertMessages.Add($"[{build}] [{hero}] not pick (null) talent has incorrect name or icon steam is null");

                    // test a not found
                    testTalent = heroInfo.GetTalent("asdf");
                    if (testTalent.Name != "asdf" || testTalent.GetIcon() == null)
                        assertMessages.Add($"[{build}] [{hero}] not found talent has incorrect name or icon steam is null");

                    foreach (var talent in heroInfo.Talents.Values)
                    {
                        if (talent.Tier == TalentTier.Old)
                            continue;

                        if (string.IsNullOrEmpty(talent.Name) && talent.Name != "No pick")
                            Assert.Fail($"[{build}] [{hero}] [{talent}] name is null or emtpy");

                        if (string.IsNullOrEmpty(talent.ReferenceName))
                            assertMessages.Add($"[{build}] [{hero}] [{talent}] talent reference name is null or emtpy");

                        if (string.IsNullOrEmpty(talent.TooltipDescriptionName))
                            assertMessages.Add($"[{build}] [{hero}] [{talent}] talent tooltip description name is null or emtpy");

                        // tooltips
                        TalentTooltip talentTooltip = talent.Tooltip;

                        // full
                        if (string.IsNullOrEmpty(talentTooltip.Full))
                        {
                            assertMessages.Add($"[{build}] [{hero}] [{talent}] Full tooltip is null or empty");
                        }
                        else
                        {
                            string strippedText = TalentTooltipStripNonText(talentTooltip.Full);

                            if (NonValidCharsCheck(strippedText))
                                assertMessages.Add($"[{build}] [{hero}] [{talent}] Invalid chars in FULL tooltip{Environment.NewLine}{strippedText}{Environment.NewLine}");
                        }

                        // short
                        if (string.IsNullOrEmpty(talentTooltip.Short))
                        {
                            assertMessages.Add($"[{build}] [{hero}] [{talent}] Short tooltip is null or empty");
                        }
                        else
                        {
                            string strippedText = TalentTooltipStripNonText(talentTooltip.Short);

                            if (NonValidCharsCheck(strippedText))
                                assertMessages.Add($"[{build}] [{hero}] [{talent}] Invalid chars in SHORT tooltip{Environment.NewLine}{strippedText}{Environment.NewLine}");
                        }

                        if (talent.GetIcon() == null)
                            assertMessages.Add($"[{build}] [{hero}] [{talent}] Icon stream is null");

                        testTalent = heroInfo.GetTalent(talent.ReferenceName);
                        if (testTalent == null || testTalent.Name == talent.ReferenceName || testTalent.Name == "No pick")
                            assertMessages.Add($"[{build}] [{hero}] [{talent}] GetTalent() failed to return correct info");
                    }
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

        [TestMethod]
        public void GetPatchNotesFromBuildTest()
        {
            Assert.IsNotNull(HeroesIcons.HeroBuilds().GetPatchNotes(57797));
            Assert.IsNull(HeroesIcons.HeroBuilds().GetPatchNotes(0));
        }

        [TestMethod]
        public void GetListOfHeroesTest()
        {
            Assert.IsTrue(HeroesIcons.HeroBuilds().GetListOfHeroes(57797).Count > 1);
            Assert.IsTrue(HeroesIcons.HeroBuilds().GetListOfHeroes(0).Count == 0);
        }
    }
}
