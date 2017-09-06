using Heroes.Icons.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Windows.Media.Imaging;

namespace Heroes.Icons.Tests
{
    [TestClass]
    public class HeroBuildsTests : HeroesIconsBase
    {
        [TestMethod]
        public void HeroesTalentsTest()
        {
            List<string> assertMessages = new List<string>();
            int segment = 5;

            foreach (int build in HeroesIcons.GetListOfHeroesBuilds().ConvertAll(x => Int32.Parse(x)))
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

                    int talent1 = 0;
                    int talent4 = 0;
                    int talent7 = 0;
                    int talent10 = 0;
                    int talent13 = 0;
                    int talent16 = 0;
                    int talent20 = 0;

                    foreach (var talent in allTalents.Values)
                    {
                        if (talent.Tier == TalentTier.Old)
                            continue;

                        if (talent.Tier == TalentTier.Level1)
                            talent1++;
                        else if (talent.Tier == TalentTier.Level4)
                            talent4++;
                        else if(talent.Tier == TalentTier.Level7)
                            talent7++;
                        else if(talent.Tier == TalentTier.Level10)
                            talent10++;
                        else if(talent.Tier == TalentTier.Level13)
                            talent13++;
                        else if(talent.Tier == TalentTier.Level16)
                            talent16++;
                        else if(talent.Tier == TalentTier.Level20)
                            talent20++;

                        BitmapImage talentImage = talent.GetIcon();

                        if (talentImage.UriSource.Segments[segment].ToString() == HeroesBase.NoTalentIconPick || talentImage.UriSource.Segments[segment].ToString() == HeroesBase.NoTalentIconFound)
                            assertMessages.Add($"[{build}] [{hero}] [{talent}] Talent image not found");

                        if (string.IsNullOrEmpty(talent.ReferenceName))
                            assertMessages.Add($"[{build}] [{hero}] [{talent}] No reference name");

                        if (string.IsNullOrEmpty(talent.TooltipDescriptionName))
                            assertMessages.Add($"[{build}] [{hero}] [{talent}] No tooltip description name");

                        // tooltips
                        TalentTooltip talentTooltip = talent.Tooltip;

                        // full
                        if (string.IsNullOrEmpty(talentTooltip.Full))
                        {
                            assertMessages.Add($"[{build}] [{hero}] [{talent}] Full tooltip not found");
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
                            assertMessages.Add($"[{build}] [{hero}] [{talent}] Short tooltip not found");
                        }
                        else
                        {
                            string strippedText = TalentTooltipStripNonText(talentTooltip.Short);

                            if (NonValidCharsCheck(strippedText))
                                assertMessages.Add($"[{build}] [{hero}] [{talent}] Invalid chars in SHORT tooltip{Environment.NewLine}{strippedText}{Environment.NewLine}");
                        }
                    }

                    if (talent1 < 1)
                        assertMessages.Add($"[{build}] [{hero}] No Level 1 talents");
                    if (talent4 < 1)
                        assertMessages.Add($"[{build}] [{hero}] No Level 4 talents");
                    if (talent7 < 1)
                        assertMessages.Add($"[{build}] [{hero}] No Level 7 talents");
                    if (talent10 < 1)
                        assertMessages.Add($"[{build}] [{hero}] No Level 10 talents");
                    if (talent13 < 1)
                        assertMessages.Add($"[{build}] [{hero}] No Level 13 talents");
                    if (talent16 < 1)
                        assertMessages.Add($"[{build}] [{hero}] No Level 16 talents");
                    if (talent20 < 1)
                        assertMessages.Add($"[{build}] [{hero}] No Level 20 talents");
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
