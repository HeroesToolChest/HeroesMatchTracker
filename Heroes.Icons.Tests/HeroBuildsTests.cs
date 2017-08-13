using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using Heroes.Icons.Models;
using System.Windows.Media.Imaging;

namespace Heroes.Icons.Tests
{
    [TestClass]
    public class HeroBuildsTests : HeroesIconsBase
    {
        [TestMethod]
        public void HeroeTalentsTest()
        {
            List<string> assertMessages = new List<string>();
            int segment = 5;

            foreach (int build in HeroesIcons.GetListOfHeroesBuilds())
            {
                HeroesIcons.LoadHeroesBuild(build);

                var heroes = HeroesIcons.Heroes().GetListOfHeroes(build);

                foreach (string hero in heroes)
                {
                    var allTalents = HeroesIcons.HeroBuilds().GetHeroTalents(hero);

                    if (allTalents == null)
                    {
                        assertMessages.Add($"[{build}] [{hero}] No talents found");
                        continue;
                    }
                    if (allTalents[TalentTier.Level1] == null || allTalents[TalentTier.Level1].Count < 1)
                        assertMessages.Add($"[{build}] [{hero}] No Level 1 talents");
                    if (allTalents[TalentTier.Level4] == null || allTalents[TalentTier.Level4].Count < 1)
                        assertMessages.Add($"[{build}] [{hero}] No Level 4 talents");
                    if (allTalents[TalentTier.Level7] == null || allTalents[TalentTier.Level7].Count < 1)
                        assertMessages.Add($"[{build}] [{hero}] No Level 7 talents");
                    if (allTalents[TalentTier.Level10] == null || allTalents[TalentTier.Level10].Count < 1)
                        assertMessages.Add($"[{build}] [{hero}] No Level 10 talents");
                    if (allTalents[TalentTier.Level13] == null || allTalents[TalentTier.Level13].Count < 1)
                        assertMessages.Add($"[{build}] [{hero}] No Level 13 talents");
                    if (allTalents[TalentTier.Level16] == null || allTalents[TalentTier.Level16].Count < 1)
                        assertMessages.Add($"[{build}] [{hero}] No Level 16 talents");
                    if (allTalents[TalentTier.Level20] == null || allTalents[TalentTier.Level20].Count < 1)
                        assertMessages.Add($"[{build}] [{hero}] No Level 20 talents");

                    // loop through each talent tier
                    foreach (var talentTier in allTalents)
                    {
                        if (talentTier.Key == TalentTier.Old)
                            continue;

                        // loop through each talent
                        foreach (Talent talent in talentTier.Value)
                        {
                            BitmapImage talentImage = talent.GetIcon();

                            if (talentImage.UriSource.Segments[segment].ToString() == HeroesBase.NoTalentIconPick || talentImage.UriSource.Segments[segment].ToString() == HeroesBase.NoTalentIconFound)
                                assertMessages.Add($"[{build}] Talent image not found for {talent} [{talentTier.Key.ToString()}]");

                            if (string.IsNullOrEmpty(talent.ReferenceName))
                                assertMessages.Add($"[{build}] [{hero}] [{talent.Name}] No reference name");

                            if (string.IsNullOrEmpty(talent.TooltipDescriptionName))
                                assertMessages.Add($"[{build}] [{hero}] [{talent.Name}] No tooltip description name");

                            // tooltips
                            TalentTooltip talentTooltip = talent.Tooltip;

                            // full
                            if (string.IsNullOrEmpty(talentTooltip.Full))
                            {
                                assertMessages.Add($"[{build}] Full tooltip not found for {talent} [{talentTier.Key.ToString()}]");
                            }
                            else
                            {
                                string strippedText = TalentTooltipStripNonText(talentTooltip.Full);

                                if (NonValidCharsCheck(strippedText))
                                    assertMessages.Add($"[{build}] Invalid chars in FULL tooltip {talent} [{talentTier.Key.ToString()}]{Environment.NewLine}{strippedText}{Environment.NewLine}");
                            }

                            // short
                            if (string.IsNullOrEmpty(talentTooltip.Short))
                            {
                                assertMessages.Add($"[{build}] Short tooltip not found for {talent} [{talentTier.Key.ToString()}]");
                            }
                            else
                            {
                                string strippedText = TalentTooltipStripNonText(talentTooltip.Short);

                                if (NonValidCharsCheck(strippedText))
                                    assertMessages.Add($"[{build}] Invalid chars in SHORT tooltip {talent} [{talentTier.Key.ToString()}]{Environment.NewLine}{strippedText}{Environment.NewLine}");
                            }


                            //// GetHeroTalent()
                            //var talentNoPick = heroesIcons.HeroBuilds().GetHeroTalent(hero, TalentTier.Level1, string.Empty);
                            //if (talentNoPick.Name != "No pick")
                            //    assertMessages.Add($"[{build}] Method GetHeroTalent() did not return No pick");
                            //if (talentNoPick.GetIcon().UriSource.Segments[segment].ToString() != HeroesBase.NoTalentIconPick)
                            //    assertMessages.Add($"[{build}] Empty reference name did not return No pick talent icon");

                            //talentNoPick = heroesIcons.HeroBuilds().GetHeroTalent(hero, TalentTier.Level1, "xwekrj2k3lj4ks");

                            //if (talentNoPick.Name != "xwekrj2k3lj4ks")
                            //    assertMessages.Add($"[{build}] Method GetHeroTalent() did not return same reference name");
                            //if (talentNoPick.GetIcon().UriSource.Segments[segment].ToString() != HeroesBase.NoTalentIconFound)
                            //    assertMessages.Add($"[{build}] Unknown reference name did not return NoTalentIconFound");
                        }
                    }
                }
            }

            AssertFailMessage(assertMessages);
        }

        [TestMethod]
        public void GetHeroTalentsTest()
        {
            var allTalents = HeroesIcons.HeroBuilds().GetHeroTalents("Abathur");
            Assert.IsNotNull(allTalents);

            allTalents = HeroesIcons.HeroBuilds().GetHeroTalents("asdf");
            Assert.IsNull(allTalents);
        }

        [TestMethod]
        public void GetHeroTalentTest()
        {
            int segment = 5;

            Talent talentNoPick = HeroesIcons.HeroBuilds().GetHeroTalent("Abathur", TalentTier.Level1, string.Empty);
            Assert.IsTrue(talentNoPick.Name == "No pick");
            Assert.IsTrue(talentNoPick.GetIcon().UriSource.Segments[segment].ToString() == HeroesBase.NoTalentIconPick);

            talentNoPick = HeroesIcons.HeroBuilds().GetHeroTalent("Abathur", TalentTier.Level1, "xwekrj2k3lj4ks");
            Assert.IsTrue(talentNoPick.Name == "xwekrj2k3lj4ks");
            Assert.IsTrue(talentNoPick.GetIcon().UriSource.Segments[segment].ToString() == HeroesBase.NoTalentIconFound);
        }
    }
}
